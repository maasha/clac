# UNDO Implementation Plan

## Overview
Implement UNDO functionality in two steps:
1. **Stack History Only** (first implementation)
2. **Add Input History** (second implementation)

## Architecture: Isolated Modules

### Stack History Module
- **Data structure**: `Stack<StackSnapshot>` or similar
- **Methods**: `SaveStackState()`, `UndoStack()`, `CanUndo()`, `Clear()`
- **Isolated**: No knowledge of input history
- **Responsibility**: Track and restore stack states

### Input History Module
- **Data structure**: `Stack<string>`
- **Methods**: `SaveInput()`, `UndoInput()`, `CanUndo()`, `Clear()`
- **Isolated**: No knowledge of stack history
- **Responsibility**: Track and restore input strings

### Coordination (CalculatorViewModel)
- Saves both together after successful `Enter()` operations
- Undoes both together when UNDO button is pressed
- Keeps both histories synchronized
- Minimal coupling: only at the coordination level

## Step 1: Stack History Only

### Implementation Tasks
1. Create `StackHistory` class/module
2. Save stack snapshot after successful operations in `Enter()`
3. Implement `Undo()` method in ViewModel
4. Wire UNDO button to `Undo()` method
5. Handle edge cases (no history, clear command, etc.)

### When to Save History
- After successful `Enter()` that modifies stack
- After operations: `+`, `-`, `*`, `/`, `sqrt()`, `pow()`, `reciprocal()`, `sum()`, `swap()`, `pop()`
- **Not saved**: 
  - `clear()` command (clears history)
  - Failed operations (errors)
  - Operations that don't modify stack

### Stack Snapshot Requirements
- Need to capture complete stack state
- Should be a deep copy/snapshot (not reference)
- Consider using `RpnStack.Clone()` or similar

## Step 2: Add Input History

### Implementation Tasks
1. Create `InputHistory` class/module
2. Save input string along with stack state
3. Modify `Undo()` to restore both stack and input
4. Input history shows "how we got to this stack state"

### History Entry Concept
- Each entry represents: (StackState, InputThatCreatedThisState)
- UNDO restores both the stack AND the input that led to it
- The input represents the transformation, not something to continue editing

### Example Workflow
1. User types "1 2 3", presses Enter
   - Stack: [1, 2, 3]
   - Input: "" (cleared)
   - **History entry**: Stack [1, 2, 3], Input "1 2 3"

2. User clicks sum() button
   - Stack: [6]
   - Input: "" (still empty)
   - **History entry**: Stack [6], Input "sum()"

3. User presses UNDO
   - Restore: Stack [1, 2, 3], Input "1 2 3"
   - Shows what led to this stack state

## Design Principles

### Isolation Benefits
- **Testable independently**: Each module can be unit tested separately
- **Incremental development**: Can implement stack history first, add input later
- **Clear separation of concerns**: Each module has single responsibility
- **Swappable implementations**: Can change implementation without affecting the other

### Synchronization
- Both histories must be saved together
- Both histories must be undone together
- Same number of entries in both stacks
- Coordination happens at ViewModel level (minimal coupling)

## Notes
- This is an RPN calculator - remember stack operations, not just input strings
- Button commands (sum(), sqrt(), etc.) should be saved as their command string
- Consider history depth limits to prevent unbounded growth
- Clear history when `clear()` command is executed

## Example

Stack History + Input History (Corrected Understanding)
Purpose:
Stack history: The state of the stack at each point
Input history: The input/command that transformed the stack from the previous state to the current state
Example: "1 2 3" → Enter → sum() → UNDO
Step 1: Initial state
Stack: []
Input: ""
Step 2: User types "1 2 3", presses Enter
Stack: [1, 2, 3]
History entry: Stack [1, 2, 3], Input "1 2 3" (this input transformed [] → [1, 2, 3])
Step 3: User clicks sum() button
Stack: [6]
History entry: Stack [6], Input "sum()" (this input transformed [1, 2, 3] → [6])
Step 4: User presses UNDO
Restore previous history entry
Stack: [1, 2, 3] ✓
Input: "1 2 3" ✓ (shows what led to this stack state)
This makes sense because:
The input history shows how we got to the current stack state
When undoing, we restore both the stack and the input that created it
The input represents the transformation, not something to continue editing
So each history entry is: (StackState, InputThatCreatedThisState)