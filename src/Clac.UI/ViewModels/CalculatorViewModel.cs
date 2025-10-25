namespace Clac.UI.ViewModels;

using System.ComponentModel;
using System.Linq;
using Clac.Core;


public class CalculatorViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string _currentInput = "";
    private string? _errorMessage = null;
    private readonly RpnProcessor _processor = new();


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

        OnPropertyChanged(nameof(CurrentInput));
        OnPropertyChanged(nameof(StackDisplay));
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
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