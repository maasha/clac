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

### Option 1: Generic History Class (✅ IMPLEMENTED)

Created a generic `History<T>` class to eliminate duplication:

```csharp
public class History<T>
{
    private readonly int _maxHistorySize;
    private readonly List<T> _history = [];
    private readonly Func<T, T>? _cloneFunc;
    private readonly Func<T, bool>? _validateFunc;

    public History(Func<T, T>? cloneFunc = null, Func<T, bool>? validateFunc = null, int maxHistorySize = 100)
    {
        _cloneFunc = cloneFunc;
        _validateFunc = validateFunc;
        _maxHistorySize = maxHistorySize;
    }
    
    public int Count => _history.Count;
    
    public Result<bool> Push(T item)
    {
        if (!IsValid(item))
            return CreateValidationError();
        var itemToAdd = CloneIfNeeded(item);
        _history.Add(itemToAdd);
        EnforceMaxHistorySize();
        return new Result<bool>(true);
    }
    
    public Result<T> Pop()
    {
        if (_history.Count == 0)
            return new Result<T>(new InvalidOperationException(HistoryIsEmpty));
        var value = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        return new Result<T>(value);
    }
    
    public bool CanUndo => _history.Count > 0;  // Property, not method
    
    // ... private helper methods
}
```

**Implementation Notes:**
- Error message is hardcoded as `HistoryIsEmpty` (not configurable)
- Wrapper classes (`RpnStackHistory`, `RpnInputHistory`) handle error message translation
- `CanUndo` is a property (not a method) for better C# idioms
- Clean code principles applied (extracted methods, clear naming)

**Current Usage:**
- `RpnStackHistory` wraps `History<RpnStack>` with clone function ✅
- `RpnInputHistory` wraps `History<string>` with validation function ✅

### Option 2: Synchronized History Pair

Create a class that keeps both histories synchronized:

```csharp
public class SynchronizedHistory
{
    private readonly History<RpnStack> _stackHistory;
    private readonly History<string> _inputHistory;
    
    public bool CanUndo => _stackHistory.CanUndo;  // Property, not method
    
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

**Status:** Not yet implemented - planned for Step 4

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

### ✅ Step 1: Create generic `History<T>` class
**Status:** COMPLETE

- Created `History<T>` class with:
  - Generic type support
  - Optional `cloneFunc` for deep cloning
  - Optional `validateFunc` for input validation
  - Configurable `maxHistorySize` (default: 100)
  - Hardcoded error message: `HistoryIsEmpty`
  - `CanUndo` as a property (not method)
- Comprehensive test coverage in `HistoryTests.cs`
- Clean code principles applied (extracted methods, clear naming)

### ✅ Step 2: Refactor `RpnStackHistory` to use `History<RpnStack>`
**Status:** COMPLETE

- `RpnStackHistory` now uses composition with `History<RpnStack>`
- Configured with `CloneStack` function for deep cloning
- Wraps `Pop()` to translate error message to `HistoryStackIsEmpty`
- Delegates `Count`, `Push`, and `CanUndo` to internal `History<RpnStack>`
- `CanUndo` changed from method to property for consistency
- Reduced from 51 lines to 39 lines
- Removed 11 redundant tests (kept only 2 wrapper-specific tests)
- All tests passing (205 total: 90 core + 115 UI)

**Implementation Details:**
- Error message translation handled in `Pop()` wrapper
- Clone function passed via constructor
- Public API unchanged (no breaking changes)

### ✅ Step 3: Refactor `RpnInputHistory` to use `History<string>`
**Status:** COMPLETE

- `RpnInputHistory` now uses composition with `History<string>`
- Configured with validation function: `input => !string.IsNullOrWhiteSpace(input)`
- Wraps `Pop()` to translate error message to `HistoryInputIsEmpty`
- Delegates `Count`, `Push`, and `CanUndo` to internal `History<string>`
- Added `CanUndo` property (was missing before)
- Reduced from 34 lines to 31 lines
- Removed 5 redundant tests (kept only 2 wrapper-specific tests)
- All tests passing (200 total: 85 core + 115 UI)

**Implementation Details:**
- Error message translation handled in `Pop()` wrapper
- Validation function passed via constructor
- Public API unchanged (no breaking changes)
- Added namespace declaration for consistency

### ⏳ Step 4: Create `SynchronizedHistory` class
**Status:** PENDING

- Create class that manages both `History<RpnStack>` and `History<string>`
- Ensure atomic operations (rollback on failure)
- Provide synchronized `Push` and `Pop` methods

### ⏳ Step 5: Update `CalculatorViewModel` to use `SynchronizedHistory`
**Status:** PENDING

- Replace separate `_stackHistory` and `_inputHistory` with single `SynchronizedHistory`
- Update `Enter()` and `Undo()` methods

### ⏳ Step 6: Update all tests to work with new structure
**Status:** PENDING

- Update `CalculatorViewModelTests` if needed
- Ensure all tests pass

### ⏳ Step 7: Remove old `RpnStackHistory` and `RpnInputHistory` classes
**Status:** PENDING

- After `SynchronizedHistory` is implemented and tested
- Consider if wrapper classes are still needed or can be removed entirely

## Benefits

- **DRY**: Single implementation of history logic ✅ Achieved
- **Maintainability**: Changes to history behavior only need to be made in one place ✅ Achieved
- **Reliability**: Synchronized operations prevent history desynchronization bugs ⏳ Pending Step 4
- **Testability**: Each component can be tested independently ✅ Achieved
- **Extensibility**: Easy to add new history types if needed ✅ Achieved

## Progress Summary

**Completed:**
- ✅ Generic `History<T>` class created and tested
- ✅ `RpnStackHistory` refactored to use `History<RpnStack>`
- ✅ `RpnInputHistory` refactored to use `History<string>`
- ✅ Test suite cleaned up (removed redundant tests from both wrapper classes)
- ✅ Code quality improvements (Clean Code principles applied)
- ✅ Both wrapper classes now use composition pattern consistently

**Remaining:**
- ⏳ Create `SynchronizedHistory` class
- ⏳ Update `CalculatorViewModel` to use `SynchronizedHistory`
- ⏳ Final cleanup and removal of wrapper classes (if appropriate)

