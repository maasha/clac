namespace Clac.UI.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using DotNext;
using Clac.Core.History;
using Clac.Core.Rpn;
using Clac.UI.Configuration;
using Clac.UI.Helpers;
using Clac.UI.Models;
using Clac.Core.Services;

public class CalculatorViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private static readonly string CurrentInputPropertyName = nameof(CurrentInput);

    private string _currentInput = "";
    private string? _errorMessage = null;
    private readonly Processor _processor = new();
    private IPersistence _persistence = null!;
    private StackAndInputHistory _history = new();
    private readonly Dictionary<string, List<string>> _errors = [];
    public ObservableCollection<StackLineItem> DisplayItems { get; }

    public Core.Rpn.Stack Stack => _processor.Stack;

    private bool _showScrollBar = false;
    public bool ShowScrollBar
    {
        get => _showScrollBar;
        private set
        {
            if (_showScrollBar != value)
            {
                _showScrollBar = value;
                OnPropertyChanged();
            }
        }
    }

    public CalculatorViewModel(IPersistence persistence)
    {
        _persistence = persistence;
        DisplayItems = [];
        LoadHistory();

        if (_history.IsEmpty)
        {
            InitializeEmptyDisplayItems();
        }
    }

    private void LoadHistory()
    {
        var result = _persistence.Load();
        if (!result.IsSuccessful)
            _errorMessage = result.Error.Message;
        if (result.Value == null)
            return;
        _history = result.Value;
        LoadLastHistory();
    }

    private void LoadLastHistory()
    {
        var result = _history.Last();

        if (!result.IsSuccessful)
        {
            _errorMessage = "Failed to load history";
            return;
        }
        _processor.RestoreStack(result.Value.stack);
        UpdateDisplayItems();
        OnPropertyChanged(nameof(StackDisplay));
    }

    public string[] StackDisplay => GetStackDisplay();

    private string[] GetStackDisplay()
    {
        return [.. _processor.Stack.ToArray().Select(x => x.ToString(CultureInfo.InvariantCulture))];
    }

    public string CurrentInput
    {
        get => _currentInput;
        set
        {
            if (_currentInput != value)
                SetCurrentInputAndClearErrors(value);
        }
    }

    public bool HasError => !string.IsNullOrEmpty(_errorMessage);
    public string ErrorMessage => _errorMessage ?? "";
    public bool CanUndo => !HasError && _history.CanUndo;

    private const double ErrorLineHeight = 50.0;
    public double WindowHeight => SettingsManager.UI.WindowHeight + (HasError ? ErrorLineHeight : 0);
    public double WindowMinHeight => SettingsManager.UI.WindowMinHeight + (HasError ? ErrorLineHeight : 0);

    public static double DisplayHeight => (SettingsManager.UI.DisplayLines * SettingsManager.UI.LineHeight) + SettingsManager.UI.BorderThickness;

    public void AppendToInput(string value)
    {
        _currentInput += value;
        ClearInputErrorsAndNotify();
    }

    public void DeleteFromInput()
    {
        if (string.IsNullOrEmpty(_currentInput))
            return;

        RemoveLastChars();
        ClearInputErrorsAndNotify();
    }

    public void Enter()
    {
        if (string.IsNullOrWhiteSpace(_currentInput))
            return;

        ClearErrors(CurrentInputPropertyName);

        var tokens = ParseInput();
        if (!tokens.IsSuccessful)
            return;

        var stackBeforeProcessing = _processor.Stack;
        _history.Push(stackBeforeProcessing, _currentInput);
        _persistence.Save(_history);

        var result = ProcessTokens(tokens.Value);
        if (result == null)
            return;

        ClearSuccessState();
    }

    public void Undo()
    {
        if (!CanUndo)
            return;

        var result = _history.Pop();
        if (!result.IsSuccessful)
            return;
        _persistence.Save(_history);
        SetCurrentInputAndClearErrors(result.Value.input);
        _processor.RestoreStack(result.Value.stack);
        UpdateDisplayItems();
        OnPropertyChanged(nameof(StackDisplay));
    }

    public void Clear()
    {
        SetCurrentInputAndClearErrors("clear()");
        Enter();
    }

    private void InitializeEmptyDisplayItems()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        DisplayItems.Clear();

        for (int lineNum = displayLines; lineNum >= 1; lineNum--)
            DisplayItems.Add(new StackLineItem($"{lineNum}:", ""));
    }

    private void UpdateDisplayItems()
    {
        var stack = GetStackDisplay();
        int displayLines = SettingsManager.UI.DisplayLines;
        int maxDecimalPartLength = GetMaxDecimalPartLength(stack);
        int totalLines = Math.Max(displayLines, stack.Length);

        var context = new DisplayItemContext
        {
            Stack = stack,
            TotalLines = totalLines,
            MaxDecimalPartLength = maxDecimalPartLength
        };

        PopulateDisplayItems(context);
        UpdateScrollBarVisibility(stack.Length, displayLines);
    }

    private static int GetMaxDecimalPartLength(string[] stack)
    {
        var visibleValues = stack.Where(v => !string.IsNullOrEmpty(v)).ToArray();
        return visibleValues.Length > 0
            ? DisplayFormatter.GetMaxDecimalPartLength(visibleValues)
            : 0;
    }

    private void PopulateDisplayItems(DisplayItemContext context)
    {
        DisplayItems.Clear();

        for (int lineNum = context.TotalLines; lineNum >= 1; lineNum--)
            AddDisplayItemForLine(context, lineNum);
    }

    private struct DisplayItemContext
    {
        public string[] Stack { get; init; }
        public int TotalLines { get; init; }
        public int MaxDecimalPartLength { get; init; }
    }

    private static string GetStackValue(string[] stack, int lineNum)
    {
        int stackIndex = stack.Length - lineNum;
        return stackIndex >= 0 ? stack[stackIndex] : "";
    }

    private void AddDisplayItemForLine(DisplayItemContext context, int lineNum)
    {
        string value = GetStackValue(context.Stack, lineNum);
        string lineNumber = DisplayFormatter.FormatLineNumber(lineNum, context.TotalLines);
        string formattedValue = string.IsNullOrEmpty(value)
            ? ""
            : DisplayFormatter.FormatValue(value, context.MaxDecimalPartLength);

        DisplayItems.Add(new StackLineItem(lineNumber, formattedValue));
    }

    private void UpdateScrollBarVisibility(int stackLength, int displayLines)
    {
        ShowScrollBar = stackLength > displayLines;
    }

    public bool HasErrors => _errors.Any();

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            return Enumerable.Empty<string>();

        return _errors[propertyName];
    }

    private void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName))
            _errors[propertyName] = [];

        if (!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName))
            OnErrorsChanged(propertyName);
    }

    private void SetErrorMessageAndNotify(string message)
    {
        _errorMessage = message;
        AddError(CurrentInputPropertyName, message);
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(WindowHeight));
        OnPropertyChanged(nameof(WindowMinHeight));
    }

    private void ClearInputErrorsAndNotify()
    {
        ClearErrors(CurrentInputPropertyName);
        _errorMessage = null;
        OnPropertyChanged(CurrentInputPropertyName);
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(WindowHeight));
        OnPropertyChanged(nameof(WindowMinHeight));
    }

    private void SetCurrentInputAndClearErrors(string value)
    {
        _currentInput = value;
        ClearErrors(CurrentInputPropertyName);
        _errorMessage = null;
        OnPropertyChanged(CurrentInputPropertyName);
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(WindowHeight));
        OnPropertyChanged(nameof(WindowMinHeight));
    }

    private void ClearInputAndUpdateDisplay()
    {
        _currentInput = "";
        UpdateDisplayItems();
        OnPropertyChanged(CurrentInputPropertyName);
        OnPropertyChanged(nameof(StackDisplay));
    }

    private void ClearSuccessState()
    {
        _errorMessage = null;
        ClearInputAndUpdateDisplay();
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(WindowHeight));
        OnPropertyChanged(nameof(WindowMinHeight));
    }
    private bool HasPrefixedSpace()
    {
        if (_currentInput.Length < 2)
            return false;

        var lastChar = _currentInput[_currentInput.Length - 1];
        var validOperator = _processor.OperatorRegistry.GetOperator(lastChar.ToString());
        return validOperator.IsSuccessful && _currentInput[_currentInput.Length - 2] == ' ';
    }

    private void RemoveLastChars()
    {
        var hasPrefixedSpace = HasPrefixedSpace();
        var charsToRemove = hasPrefixedSpace ? 2 : 1;
        _currentInput = _currentInput.Substring(0, _currentInput.Length - charsToRemove);
    }

    private Result<List<Token>> ParseInput()
    {
        var tokens = _processor.Parser.Parse(_currentInput);
        if (!tokens.IsSuccessful)
            SetErrorMessageAndNotify(tokens.Error.Message);
        return tokens;
    }

    private Result<double>? ProcessTokens(List<Token> tokens)
    {
        var result = _processor.Process(tokens);
        if (!result.IsSuccessful)
        {
            SetErrorMessageAndNotify(result.Error.Message);
            ClearInputAndUpdateDisplay();
            return null;
        }
        return result;
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
