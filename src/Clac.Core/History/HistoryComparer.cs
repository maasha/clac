using System.Linq;
using Clac.Core.Rpn;

namespace Clac.Core.History;

public static class HistoryComparer
{
    public static bool AreEqual(StackAndInputHistory? first, StackAndInputHistory second)
    {
        if (first == null)
            return false;

        var (firstStacks, firstInputs) = ExtractHistoryArrays(first);
        var (secondStacks, secondInputs) = ExtractHistoryArrays(second);

        if (!LengthsMatch(firstStacks, firstInputs, secondStacks, secondInputs))
            return false;

        return StacksMatch(firstStacks, secondStacks) && InputsMatch(firstInputs, secondInputs);
    }

    private static (Stack[] stacks, string[] inputs) ExtractHistoryArrays(StackAndInputHistory history)
    {
        return (history.StackHistory.ToArray(), history.InputHistory.ToArray());
    }

    private static bool LengthsMatch(Stack[] firstStacks, string[] firstInputs, Stack[] secondStacks, string[] secondInputs)
    {
        return firstStacks.Length == secondStacks.Length && firstInputs.Length == secondInputs.Length;
    }

    private static bool StacksMatch(Stack[] firstStacks, Stack[] secondStacks)
    {
        for (int i = 0; i < firstStacks.Length; i++)
        {
            if (!StacksEqual(firstStacks[i], secondStacks[i]))
                return false;
        }
        return true;
    }

    private static bool StacksEqual(Stack firstStack, Stack secondStack)
    {
        var firstStackData = firstStack.ToArray();
        var secondStackData = secondStack.ToArray();
        return firstStackData.SequenceEqual(secondStackData);
    }

    private static bool InputsMatch(string[] firstInputs, string[] secondInputs)
    {
        return firstInputs.SequenceEqual(secondInputs);
    }
}

