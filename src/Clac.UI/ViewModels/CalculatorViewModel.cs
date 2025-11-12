namespace Clac.UI.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls.Primitives;
using DotNext;
using Clac.Core;
using Clac.UI.Configuration;
using Clac.UI.Helpers;
using Clac.UI.Models;


public class CalculatorViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    private string _currentInput = "";
    private string? _errorMessage = null;
    private readonly RpnProcessor _processor = new();
    private readonly Dictionary<string, List<string>> _errors = new();

    public ObservableCollection<StackLineItem> DisplayItems { get; }

    private ScrollBarVisibility _scrollBarVisibility = ScrollBarVisibility.Hidden;
    public ScrollBarVisibility ScrollBarVisibility
    {
        get => _scrollBarVisibility;
        private set
        {
            if (_scrollBarVisibility != value)
            {
                _scrollBarVisibility = value;
                OnPropertyChanged();
            }
        }
    }

    public CalculatorViewModel()
    {
        DisplayItems = new ObservableCollection<StackLineItem>();
        InitializeDisplayItems();
    }

    public string[] StackDisplay => [.. _processor.Stack.ToArray().Select(x => x.ToString())];

    public string CurrentInput
    {
        get => _currentInput;
        set
        {
            if (_currentInput != value)
            {
                SetCurrentInputAndClearErrors(value);
            }
        }
    }

    public bool HasError => !string.IsNullOrEmpty(_errorMessage);
    public string ErrorMessage => _errorMessage ?? "";

    public void AppendToInput(string value)
    {
        _currentInput += value;
        ClearErrors(nameof(CurrentInput));
        _errorMessage = null;
        OnPropertyChanged(nameof(CurrentInput));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
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

        ClearErrors(nameof(CurrentInput));

        var tokens = ParseInput();
        if (!tokens.IsSuccessful)
            return;

        var result = ProcessTokens(tokens.Value);
        if (result == null)
            return;

        ClearSuccessState();
    }

    private void InitializeDisplayItems()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        DisplayItems.Clear();

        for (int lineNum = displayLines; lineNum >= 1; lineNum--)
        {
            DisplayItems.Add(new StackLineItem($"{lineNum}:", ""));
        }
    }

    private void UpdateDisplayItems()
    {
        var stack = StackDisplay;
        int displayLines = SettingsManager.UI.DisplayLines;

        var visibleValues = stack.Where(v => !string.IsNullOrEmpty(v)).ToArray();
        int maxIntegerPartLength = visibleValues.Length > 0
            ? DisplayFormatter.GetMaxIntegerPartLength(visibleValues)
            : 0;

        int totalLines = Math.Max(displayLines, stack.Length);

        DisplayItems.Clear();

        for (int lineNum = totalLines; lineNum >= 1; lineNum--)
        {
            int stackIndex = stack.Length - lineNum;
            string value = stackIndex >= 0 ? stack[stackIndex] : "";

            string lineNumber = DisplayFormatter.FormatLineNumber(lineNum, totalLines);
            string formattedValue = string.IsNullOrEmpty(value)
                ? ""
                : DisplayFormatter.FormatValue(value, maxIntegerPartLength);

            DisplayItems.Add(new StackLineItem(lineNumber, formattedValue));
        }

        ScrollBarVisibility = stack.Length > displayLines
            ? ScrollBarVisibility.Auto
            : ScrollBarVisibility.Hidden;
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
            _errors[propertyName] = new List<string>();

        if (!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (_errors.ContainsKey(propertyName))
        {
            _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }

    private void SetErrorMessageAndNotify(string message)
    {
        _errorMessage = message;
        AddError(nameof(CurrentInput), message);
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
    }

    private void ClearInputErrorsAndNotify()
    {
        ClearErrors(nameof(CurrentInput));
        _errorMessage = null;
        OnPropertyChanged(nameof(CurrentInput));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
    }

    private void SetCurrentInputAndClearErrors(string value)
    {
        _currentInput = value;
        ClearErrors(nameof(CurrentInput));
        _errorMessage = null;
        OnPropertyChanged(nameof(CurrentInput));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
    }

    private void ClearInputAndUpdateDisplay()
    {
        _currentInput = "";
        UpdateDisplayItems();
        OnPropertyChanged(nameof(CurrentInput));
        OnPropertyChanged(nameof(StackDisplay));
    }

    private void ClearSuccessState()
    {
        _errorMessage = null;
        ClearInputAndUpdateDisplay();
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
    }

    private bool HasPrefixedSpace()
    {
        if (_currentInput.Length < 2)
            return false;

        var lastChar = _currentInput[_currentInput.Length - 1];
        return Operator.IsValidOperator(lastChar.ToString())
            && _currentInput[_currentInput.Length - 2] == ' ';
    }

    private void RemoveLastChars()
    {
        var hasPrefixedSpace = HasPrefixedSpace();
        var charsToRemove = hasPrefixedSpace ? 2 : 1;
        _currentInput = _currentInput.Substring(0, _currentInput.Length - charsToRemove);
    }

    private Result<List<Token>> ParseInput()
    {
        var tokens = RpnParser.Parse(_currentInput);
        if (!tokens.IsSuccessful)
        {
            SetErrorMessageAndNotify(tokens.Error.Message);
        }
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
