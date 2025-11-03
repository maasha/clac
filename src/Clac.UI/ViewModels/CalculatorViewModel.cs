namespace Clac.UI.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls.Primitives;
using Clac.Core;
using Clac.UI.Configuration;
using Clac.UI.Helpers;
using Clac.UI.Models;


public class CalculatorViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string _currentInput = "";
    private string? _errorMessage = null;
    private readonly RpnProcessor _processor = new();

    /// <summary>
    /// Gets the collection of items to display in the stack view.
    /// </summary>
    public ObservableCollection<StackLineItem> DisplayItems { get; }

    private ScrollBarVisibility _scrollBarVisibility = ScrollBarVisibility.Hidden;
    /// <summary>
    /// Gets the scrollbar visibility for the display.
    /// </summary>
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

    public string[] StackDisplay => _processor.Stack.ToArray()
    .Select(x => x.ToString())
    .ToArray();

    public string CurrentInput
    {
        get => _currentInput;
        set => _currentInput = value;
    }

    public bool HasError => !string.IsNullOrEmpty(_errorMessage);
    public string ErrorMessage => _errorMessage ?? "";

    /// <summary>
    /// Enters the current input into the calculator.
    /// If the input is valid, the current input is cleared and the result is
    /// displayed.
    /// 
    /// If the input is invalid, the current input is not cleared and the error
    /// is displayed.
    /// </summary>
    public void Enter()
    {
        if (string.IsNullOrWhiteSpace(_currentInput))
            return;

        var tokens = RpnParser.Parse(_currentInput);

        if (!tokens.IsSuccessful)
        {
            _errorMessage = tokens.Error.Message;
            return;
        }

        var result = _processor.Process(tokens.Value);

        if (!result.IsSuccessful)
        {
            _errorMessage = result.Error.Message;
            return;
        }

        _errorMessage = null;
        _currentInput = "";

        UpdateDisplayItems();
        OnPropertyChanged(nameof(CurrentInput));
        OnPropertyChanged(nameof(StackDisplay));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
    }

    /// <summary>
    /// Initializes the display items with empty lines based on configured display lines.
    /// </summary>
    private void InitializeDisplayItems()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        DisplayItems.Clear();

        for (int lineNum = displayLines; lineNum >= 1; lineNum--)
        {
            DisplayItems.Add(new StackLineItem
            {
                LineNumber = $"{lineNum}:",
                FormattedValue = ""
            });
        }
    }

    /// <summary>
    /// Updates the display items to reflect the current stack state.
    /// </summary>
    private void UpdateDisplayItems()
    {
        var stack = StackDisplay;
        int displayLines = SettingsManager.UI.DisplayLines;

        var visibleValues = stack.Where(v => !string.IsNullOrEmpty(v)).ToArray();
        int maxIntegerPartLength = visibleValues.Length > 0
            ? DisplayFormatter.GetMaxIntegerPartLength(visibleValues)
            : 0;

        // Determine how many lines to show: at least displayLines, or all stack items if more
        int totalLines = Math.Max(displayLines, stack.Length);

        DisplayItems.Clear();

        for (int lineNum = totalLines; lineNum >= 1; lineNum--)
        {
            // Calculate which stack position this line should display
            int stackIndex = stack.Length - lineNum;
            string value = stackIndex >= 0 ? stack[stackIndex] : "";

            DisplayItems.Add(new StackLineItem
            {
                LineNumber = DisplayFormatter.FormatLineNumber(lineNum, totalLines),
                FormattedValue = string.IsNullOrEmpty(value)
                    ? ""
                    : DisplayFormatter.FormatValue(value, maxIntegerPartLength)
            });
        }

        // Show scrollbar only if stack has more values than display lines
        ScrollBarVisibility = stack.Length > displayLines
            ? ScrollBarVisibility.Auto
            : ScrollBarVisibility.Hidden;
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}