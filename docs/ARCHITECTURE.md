# Clac - RPN Calculator Architecture

## Overview
Clac is a Reverse Polish Notation (RPN) calculator application built with .NET 9 and Avalonia UI. The architecture follows a clean separation between the core business logic and the user interface.

## Architecture Layers

### 1. Clac.Core (Domain Layer)
The core domain layer contains all business logic and is independent of the UI framework.

#### Key Classes:

**Token** (Abstract Record)
- Represents parsed input tokens
- Three concrete types:
  - `NumberToken`: Contains a numeric value
  - `OperatorToken`: Contains an operator symbol
  - `CommandToken`: Contains a command string

**RpnStack**
- Manages the calculator stack
- Operations: Push, Pop, Peek, Clear, Swap
- Mathematical operations: Sum, Sqrt, Pow, Reciprocal
- All operations return `Result<T>` for error handling

**RpnParser**
- Parses string input into tokens
- Validates input before parsing
- Uses `Operator` class to identify operators
- Returns `Result<List<Token>>` for error handling

**RpnEvaluator**
- Evaluates binary operations (+, -, *, /)
- Handles division by zero errors
- Returns `Result<double>` for error handling

**RpnProcessor**
- Orchestrates the RPN calculation process
- Processes tokens sequentially
- Manages stack operations
- Handles commands (clear, pop, swap, sum, sqrt, pow, reciprocal)
- Returns `Result<double>` for error handling

**Operator** (Static)
- Maps string symbols to `OperatorSymbol` enum
- Validates operator symbols
- Extensible for adding new operators

**OperatorSymbol** (Enum)
- Defines supported operators: Add, Subtract, Multiply, Divide

### 2. Clac.UI (Presentation Layer)
The UI layer uses Avalonia framework and follows MVVM pattern.

#### Key Classes:

**CalculatorViewModel**
- Main ViewModel implementing `INotifyPropertyChanged` and `INotifyDataErrorInfo`
- Bridges UI and Core logic
- **UI Framework Agnostic**: No references to Avalonia types (uses `bool ShowScrollBar` instead of `ScrollBarVisibility`)
- Manages:
  - Current input string
  - Stack display
  - Error messages
  - Display formatting
  - Window height (computed property based on error state)
- Uses `RpnProcessor` for calculations
- Uses `DisplayFormatter` for formatting
- Uses `SettingsManager` for configuration

**Models:**
- **KeyboardKey**: Represents a keyboard button (Label, Value, Type, ColumnSpan)
- **StackLineItem**: Represents a line in the stack display (LineNumber, FormattedValue)

**Configuration:**
- **UISettings**: Contains UI configuration (window size, display lines, etc.)
- **SettingsManager**: Static class providing access to settings

**Helpers:**
- **DisplayFormatter**: Static utility for formatting stack display values

**Enums:**
- **KeyType**: Defines key types (Number, Operator, Function, Command, Enter)

**Views (Avalonia UserControls):**
- **MainWindow**: Root window (uses data binding for window height)
- **CalculatorView**: Main calculator container
- **DisplayView**: Stack display area (converts ViewModel boolean to ScrollBarVisibility)
- **InputView**: Input field
- **KeyboardView**: Keyboard container (sets ViewModel on child views via dependency property)
- **KeyboardKeyView**: Individual key view (receives ViewModel via dependency property)

## Design Patterns

### 1. Result Pattern
Used throughout the Core layer for error handling instead of exceptions:
- `Result<T>` from DotNext library
- `IsSuccessful` property indicates success/failure
- `Value` property contains the result
- `Error` property contains exception on failure

### 2. MVVM (Model-View-ViewModel)
- **Model**: Core domain classes (Clac.Core)
- **View**: Avalonia XAML views
- **ViewModel**: `CalculatorViewModel` bridges View and Model
- **Compliance**: ViewModel is UI framework agnostic (uses `bool ShowScrollBar` instead of `ScrollBarVisibility`)
- **Data Binding**: Window height and scroll bar visibility controlled via ViewModel properties
- **ViewModel Access**: Views receive ViewModel via dependency properties (no tree traversal in production)

### 3. Factory Pattern
- `Token` uses static factory methods: `CreateNumber`, `CreateOperator`, `CreateCommand`

### 4. Command Pattern
- `RpnProcessor` uses dictionary of command handlers for extensibility

## Data Flow

1. **User Input** → `CalculatorViewModel.AppendToInput()` or `DeleteFromInput()`
2. **Enter Pressed** → `CalculatorViewModel.Enter()`
3. **Parsing** → `RpnParser.Parse()` converts string to tokens
4. **Processing** → `RpnProcessor.Process()` executes tokens
5. **Evaluation** → `RpnEvaluator.Evaluate()` for operators, `RpnStack` methods for commands
6. **Display Update** → `CalculatorViewModel` updates `DisplayItems` collection
7. **UI Update** → Avalonia data binding updates views

## Error Handling

- Core layer uses `Result<T>` pattern for all operations
- ViewModel converts `Result<T>` errors to error messages
- ViewModel implements `INotifyDataErrorInfo` for validation errors
- Error messages displayed in UI

## Dependencies

- **Clac.Core**: No external dependencies (except DotNext for Result pattern)
- **Clac.UI**: Depends on Clac.Core and Avalonia UI framework
- **ViewModel Independence**: CalculatorViewModel has no references to Avalonia types, ensuring testability and framework independence

## Extensibility

1. **Adding Operators**: Extend `Operator` class and `OperatorSymbol` enum
2. **Adding Commands**: Add handler to `RpnProcessor._commandHandlers` dictionary
3. **UI Customization**: Modify `UISettings` and XAML views
4. **Adding Keyboard Keys**: Add to `KeyboardView` initialization and set ViewModel property

## MVVM Compliance

The application follows MVVM principles with the following characteristics:

- ✅ **ViewModel Independence**: ViewModel has no UI framework dependencies
- ✅ **Data Binding**: View state controlled via ViewModel properties bound in XAML
- ✅ **Explicit ViewModel Passing**: Views receive ViewModel via dependency properties
- ✅ **Separation of Concerns**: View handles UI-specific conversions (e.g., boolean to ScrollBarVisibility)
- ⚠️ **Minor Issues**: Some views access configuration directly; could be improved with ViewModel properties

