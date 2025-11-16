# History Refactoring Plan

## Code Review Analysis

### Problem 1: Code Duplication

`RpnStackHistory` and `RpnInputHistory` have significant code duplication:

**Shared Code:**
- `_maxHistorySize = 100` constant
- `Count` property
- `Push()` method pattern (add item, enforce max size)
- `Pop()` method pattern (check empty, get last item, remove, return)
- `EnforceMaxHistorySize()` method (identical implementation)
- `CanUndo()` logic (RpnInputHistory doesn't expose it, but has the same logic)

**Differences:**
- Type: `RpnStack` vs `string`
- Cloning: RpnStackHistory clones stacks; RpnInputHistory doesn't (strings are immutable)
- Validation: RpnInputHistory checks for empty strings
- Error messages: Different error messages

### Problem 2: Synchronization Risk

In `CalculatorViewModel.Enter()` (lines 105-106):
```csharp
_stackHistory.Push(stackBeforeProcessing);
_inputHistory.Push(_currentInput);
```
If the second push fails, they're out of sync.

In `CalculatorViewModel.Undo()` (lines 120, 124):
```csharp
var snapshotResult = _stackHistory.Pop();
// ... 
var inputResult = _inputHistory.Pop();
```
If the first pop succeeds but the second fails, they're out of sync. There's no guarantee they have the same count.

## Refactoring Recommendations

### Option 1: Generic History Class (Recommended)

Create a generic `History<T>` class to eliminate duplication:

```csharp
public class History<T>
{
    private readonly int _maxHistorySize = 100;
    private readonly List<T> _history = [];
    private readonly Func<T, T>? _cloneFunc;
    private readonly Func<T, bool>? _validateFunc;
    private readonly string _emptyErrorMessage;
    
    public int Count => _history.Count;
    
    public History(string emptyErrorMessage, Func<T, T>? cloneFunc = null, Func<T, bool>? validateFunc = null)
    {
        _emptyErrorMessage = emptyErrorMessage;
        _cloneFunc = cloneFunc;
        _validateFunc = validateFunc;
    }
    
    public Result<bool> Push(T item)
    {
        if (_validateFunc != null && !_validateFunc(item))
            return new Result<bool>(false);
            
        var itemToAdd = _cloneFunc != null ? _cloneFunc(item) : item;
        _history.Add(itemToAdd);
        EnforceMaxHistorySize();
        return new Result<bool>(true);
    }
    
    public Result<T> Pop()
    {
        if (_history.Count == 0)
            return new Result<T>(new InvalidOperationException(_emptyErrorMessage));
        var value = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        return new Result<T>(value);
    }
    
    public bool CanUndo() => _history.Count > 0;
    
    private void EnforceMaxHistorySize()
    {
        if (_history.Count > _maxHistorySize)
            _history.RemoveAt(0);
    }
}
```

Then:
- `RpnStackHistory` becomes `History<RpnStack>` with a clone function
- `RpnInputHistory` becomes `History<string>` with a validation function

### Option 2: Synchronized History Pair

Create a class that keeps both histories synchronized:

```csharp
public class SynchronizedHistory
{
    private readonly History<RpnStack> _stackHistory;
    private readonly History<string> _inputHistory;
    
    public bool CanUndo() => _stackHistory.CanUndo();
    
    public Result<bool> Push(RpnStack stack, string input)
    {
        var stackResult = _stackHistory.Push(stack);
        if (!stackResult.IsSuccessful)
            return stackResult;
            
        var inputResult = _inputHistory.Push(input);
        if (!inputResult.IsSuccessful)
        {
            // Rollback: pop the stack we just added
            _stackHistory.Pop();
            return inputResult;
        }
        
        return new Result<bool>(true);
    }
    
    public Result<(RpnStack stack, string input)> Pop()
    {
        var stackResult = _stackHistory.Pop();
        if (!stackResult.IsSuccessful)
            return new Result<(RpnStack, string)>(stackResult.Error);
            
        var inputResult = _inputHistory.Pop();
        if (!inputResult.IsSuccessful)
        {
            // Rollback: push the stack back
            _stackHistory.Push(stackResult.Value);
            return new Result<(RpnStack, string)>(inputResult.Error);
        }
        
        return new Result<(RpnStack, string)>((stackResult.Value, inputResult.Value));
    }
}
```

### Option 3: Combined History Entry

Store both values together in a single history:

```csharp
public class HistoryEntry
{
    public RpnStack Stack { get; init; }
    public string Input { get; init; }
}

public class CombinedHistory
{
    private readonly History<HistoryEntry> _history;
    // ... methods that always keep stack and input together
}
```

## Recommended Approach

**Combine Option 1 and Option 2:**
1. Create a generic `History<T>` class to eliminate duplication
2. Create a `SynchronizedHistory` class that uses two `History<T>` instances and ensures atomic operations

This approach provides:
- ✅ No duplication (DRY principle)
- ✅ Guaranteed synchronization
- ✅ Type safety
- ✅ Testability of each component independently

## Implementation Steps

1. Create generic `History<T>` class
2. Refactor `RpnStackHistory` to use `History<RpnStack>`
3. Refactor `RpnInputHistory` to use `History<string>`
4. Create `SynchronizedHistory` class
5. Update `CalculatorViewModel` to use `SynchronizedHistory`
6. Update all tests to work with new structure
7. Remove old `RpnStackHistory` and `RpnInputHistory` classes

## Benefits

- **DRY**: Single implementation of history logic
- **Maintainability**: Changes to history behavior only need to be made in one place
- **Reliability**: Synchronized operations prevent history desynchronization bugs
- **Testability**: Each component can be tested independently
- **Extensibility**: Easy to add new history types if needed

