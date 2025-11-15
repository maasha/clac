# UNDO Implementation Plan

## Overview
Stack history implementation is complete. This plan focuses on adding **Input History** functionality to complement the existing stack history.

## Architecture: Input History Module

### Input History Module
- **Data structure**: `Stack<string>` or similar
- **Methods**: `SaveInput()`, `UndoInput()`, `CanUndo()`
- **Isolated**: Works alongside existing stack history
- **Responsibility**: Track and restore input strings that led to each stack state

### Coordination (CalculatorViewModel)
- Stack history is already implemented and working
- Need to save input string along with stack state after successful operations
- When UNDO is pressed, restore both stack state (existing) and input string (new)
- Keep both histories synchronized
- Minimal coupling: only at the coordination level

## Implementation: Add Input History

### Implementation Tasks
1. Create `InputHistory` class/module
2. Save input string along with stack state after successful operations
3. Modify `Undo()` method to restore both stack and input
4. Input history shows "how we got to this stack state"

### When to Save Input History
- After successful `Enter()` that modifies stack
- After operations: `+`, `-`, `*`, `/`, `sqrt()`, `pow()`, `reciprocal()`, `sum()`, `swap()`, `pop()`
- **Not saved**: 
  - `clear()` command
  - Failed operations (errors)
  - Operations that don't modify stack

### History Entry Concept
- Each entry represents: (StackState, InputThatCreatedThisState)
- UNDO restores both the stack AND the input that led to it
- The input represents the transformation, not something to continue editing
- Input history entries must be synchronized with stack history entries (same count, saved together)

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
- **Testable independently**: Input history module can be unit tested separately
- **Clear separation of concerns**: Input history has single responsibility
- **Swappable implementations**: Can change implementation without affecting stack history

### Synchronization
- Input history entries must be saved together with stack history entries
- Both histories must be undone together
- Same number of entries in both stacks
- Coordination happens at ViewModel level (minimal coupling)

## Notes
- This is an RPN calculator - remember stack operations, not just input strings
- Button commands (sum(), sqrt(), etc.) should be saved as their command string
- Consider history depth limits to prevent unbounded growth
- Stack history is already implemented and working

## Example

### Stack History + Input History
**Purpose:**
- Stack history: The state of the stack at each point (already implemented)
- Input history: The input/command that transformed the stack from the previous state to the current state (to be implemented)

**Example: "1 2 3" → Enter → sum() → UNDO**

**Step 1: Initial state**
- Stack: []
- Input: ""

**Step 2: User types "1 2 3", presses Enter**
- Stack: [1, 2, 3]
- History entry: Stack [1, 2, 3], Input "1 2 3" (this input transformed [] → [1, 2, 3])

**Step 3: User clicks sum() button**
- Stack: [6]
- History entry: Stack [6], Input "sum()" (this input transformed [1, 2, 3] → [6])

**Step 4: User presses UNDO**
- Restore previous history entry
- Stack: [1, 2, 3] ✓
- Input: "1 2 3" ✓ (shows what led to this stack state)

**This makes sense because:**
- The input history shows how we got to the current stack state
- When undoing, we restore both the stack and the input that created it
- The input represents the transformation, not something to continue editing
- So each history entry is: (StackState, InputThatCreatedThisState)
