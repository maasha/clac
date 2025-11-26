using Clac.Core.Rpn;
using Clac.Core.Operations;
using Clac.Core.Functions;
using static Clac.Core.ErrorMessages;
using Xunit.Sdk;

namespace Clac.Core.Tests.Rpn;

public class ProcessorTests
{
    private readonly Processor _processor;
    private readonly OperatorRegistry _operatorRegistry;
    private readonly FunctionRegistry _functionRegistry;
    private readonly Parser _parser;
    public ProcessorTests()
    {
        _operatorRegistry = new DefaultOperatorRegistry();
        _functionRegistry = new DefaultFunctionRegistry();
        _processor = new Processor(_operatorRegistry, _functionRegistry);
        _parser = new Parser(_operatorRegistry);
    }

    [Fact]
    public void Process_EmptyTokenList_ShouldNotUpdateStack()
    {
        var tokens = new List<Token>();
        var stackLength = _processor.Stack.Count;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains(NoResultOnStack, result.Error.Message);
        Assert.Equal(stackLength, _processor.Stack.Count);
    }

    [Fact]
    public void Process_NumberToken_ShouldPushOntoStack()
    {
        var tokens = _parser.Parse("1 2 3").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, _processor.Stack.Count);
        Assert.Equal([1, 2, 3], _processor.Stack.ToArray());
    }

    [Fact]
    public void Process_OperatorTokenWithLessThanTwoNumbers_ShouldReturnError()
    {
        var tokens = _parser.Parse("1 +").Value;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void Process_OperatorTokenWithLessThanTwoNumbers_ShouldPreserveStackState()
    {
        _processor.Process(_parser.Parse("5 3 +").Value);
        Assert.Single(_processor.Stack.ToArray());
        Assert.Equal(8, _processor.Stack.Peek().Value);

        var tokens = _parser.Parse("+").Value;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
        Assert.Single(_processor.Stack.ToArray());
        Assert.Equal(8, _processor.Stack.Peek().Value);
    }


    [Fact]
    public void Process_OperatorToken_ShouldPopTwoNumbersAndPushResult()
    {
        var tokens = _parser.Parse("1 2 +").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(3, _processor.Stack.Peek().Value);
    }

    [Fact]
    public void Process_SimpleAddition_ShouldReturnCorrectResult()
    {
        var tokens = _parser.Parse("2 3 +").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void Process_ComplexExpression_ShouldReturnCorrectResult()
    {
        var tokens = _parser.Parse("5 3 - 2 *").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(4, result.Value);
    }

    [Fact]
    public void Process_DivisionByZero_ShouldReturnError()
    {
        var tokens = _parser.Parse("5 0 /").Value;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.IsType<DivideByZeroException>(result.Error);
    }

    [Fact]
    public void Process_ConsecutiveCalls_ShouldMaintainStack()
    {
        _processor.Process(_parser.Parse("1 2").Value);
        Assert.Equal(2, _processor.Stack.Count);

        var result = _processor.Process(_parser.Parse("+").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
    }

    [Fact]
    public void Process_ClearCommand_ShouldClearStack()
    {
        _processor.Process(_parser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(_parser.Parse("clear()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, _processor.Stack.Count);
    }

    [Fact]
    public void Process_PopCommand_ShouldPopLastElementFromStack()
    {
        _processor.Process(_parser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(_parser.Parse("pop()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, _processor.Stack.Count);
        Assert.Equal(3, result.Value);
        Assert.Equal([1, 2], _processor.Stack.ToArray());
    }

    [Fact]
    public void Process_SwapCommand_ShouldSwapLastTwoNumbersOfStack()
    {
        _processor.Process(_parser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(_parser.Parse("swap()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, _processor.Stack.Count);
        Assert.Equal([1, 3, 2], _processor.Stack.ToArray());
    }

    [Fact]
    public void Process_SumCommand_ShouldClearStackAndPushSum()
    {
        _processor.Process(_parser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(_parser.Parse("sum()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(6, _processor.Stack.Peek().Value);
        Assert.Equal(6, result.Value);
    }

    [Fact]
    public void Process_SqrtCommand_ShouldCalculateSquareRootOfLastElementAndPushResult()
    {
        _processor.Process(_parser.Parse("4").Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(4, _processor.Stack.Peek().Value);
        var result = _processor.Process(_parser.Parse("sqrt()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(2, _processor.Stack.Peek().Value);
    }

    [Fact]
    public void Process_SqrtCommand_WithNegativeNumber_ShouldReturnError()
    {
        _processor.Process(_parser.Parse("-1").Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(-1, _processor.Stack.Peek().Value);
        var result = _processor.Process(_parser.Parse("sqrt()").Value);
        Assert.False(result.IsSuccessful);
        Assert.Contains(InvalidNegativeSquareRoot, result.Error.Message);
    }

    [Fact]
    public void Process_PowCommand_ShouldCalculatePowerOfLastTwoNumbersAndPushResult()
    {
        _processor.Process(_parser.Parse("2 3").Value);
        Assert.Equal(2, _processor.Stack.Count);
        Assert.Equal(2, _processor.Stack.ToArray()[0]);
        Assert.Equal(3, _processor.Stack.ToArray()[1]);
        var result = _processor.Process(_parser.Parse("pow()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(8, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(8, _processor.Stack.Peek().Value);
    }

    [Fact]
    public void Process_recipCommand_ShouldCalculaterecipOfLastElementAndPushResult()
    {
        _processor.Process(_parser.Parse("4").Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(4, _processor.Stack.Peek().Value);
        var result = _processor.Process(_parser.Parse("recip()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0.25, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(0.25, _processor.Stack.Peek().Value);
    }

    [Fact]
    public void RestoreStack_ShouldReplaceStackFromGivenSnapshot()
    {
        _processor.Process(_parser.Parse("1 2 3").Value);
        var snapshot = new Stack();
        snapshot.Push(10);
        snapshot.Push(20);

        _processor.RestoreStack(snapshot);

        Assert.Equal([10, 20], _processor.Stack.ToArray());
    }

    public class OperatorRegistryTests : ProcessorTests
    {

        [Fact]
        public void Process_WithEmptyOperatorRegistry_ShouldReturnErrorForOperator()
        {
            var registry = new OperatorRegistry();
            var processor = new Processor(registry);

            var result = processor.OperatorRegistry.IsValidOperator("+");
            Assert.False(result.IsSuccessful);
            Assert.Contains("Operator '+' not found", result.Error.Message);
        }

        [Fact]
        public void Process_WithOperatorRegistry_ShouldReturnTrueForOperator()
        {
            var registry = new OperatorRegistry();
            registry.Register(new AddOperator());
            var processor = new Processor(registry);

            var result = processor.OperatorRegistry.IsValidOperator("+");
            Assert.True(result.IsSuccessful);
        }

        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("/")]
        public void Process_ShouldHaveDefaultOperatorRegistry(string symbol)
        {
            var processor = new Processor();
            var result = processor.OperatorRegistry.IsValidOperator(symbol);
            Assert.True(result.IsSuccessful);
        }
    }
    public class FunctionRegistryTests : ProcessorTests
    {
        [Fact]
        public void Process_WithEmptyFunctionRegistry_ShouldReturnErrorForFunction()
        {
            var registry = new FunctionRegistry();
            var processor = new Processor(null, registry);

            var result = processor.FunctionRegistry.IsValidFunction("pop");
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void Process_WithFunctionRegistry_ShouldReturnTrueForFunction()
        {
            var registry = new FunctionRegistry();
            registry.Register(new PopFunction());
            var processor = new Processor(null, registry);

            var result = processor.FunctionRegistry.IsValidFunction("pop");
            Assert.True(result.IsSuccessful);
        }

        [Theory]
        [InlineData("pop")]
        [InlineData("swap")]
        [InlineData("sum")]
        [InlineData("sqrt")]
        [InlineData("pow")]
        [InlineData("recip")]
        [InlineData("clear")]
        public void Process_ShouldHaveDefaultFunctionRegistry(string functionName)
        {
            var processor = new Processor();
            var result = processor.FunctionRegistry.IsValidFunction(functionName);
            Assert.True(result.IsSuccessful);
        }
    }
}
