namespace Clac.UI.ViewModels;

using System.Linq;
using Clac.Core;


public class CalculatorViewModel
{
    private string _currentInput = "";
    private readonly RpnProcessor _processor = new();

    public string[] StackDisplay => _processor.Stack.ToArray()
    .Select(x => x.ToString())
    .ToArray();
    public string CurrentInput
    {
        get => _currentInput;
        set => _currentInput = value;
    }

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
        if (tokens.IsSuccessful)
        {
            _processor.Process(tokens.Value);
            _currentInput = "";
        }
    }
}