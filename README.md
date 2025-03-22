# WPF Calculator Application

# 📌 Overview

This is a WPF (Windows Presentation Foundation) calculator application following the MVVM (Model-View-ViewModel) pattern. The application provides both Standard and Programmer calculator modes, utilizing modern C# features and best practices for WPF development.

# 🏗️ Project Architecture

Main Components

MainWindow.xaml

The primary UI window of the calculator.

Contains a menu bar with File, View, and Help options.

Supports two calculator views:

Standard Calculator View

Programmer Calculator View

Fixed window size: 320x500, centered on the screen.

Light gray background (#F0F0F0).

MainWindow.xaml.cs

Code-behind for MainWindow.xaml.

Initializes CalculatorViewModel.

Handles keyboard input:

Enter → Equals (=)

Escape → Clear (C)

Manages menu item interactions (Exit, About, etc.).

CalculatorViewModel.cs

Implements core business logic.

Uses INotifyPropertyChanged for UI updates.

Key components:

CalculatorEngine: Performs all calculations.

MemoryManager: Manages memory operations.

NumberBaseConverter: Handles number base conversions.

Properties:

Display: Current calculator output.

UseDigitGrouping: Formats numbers with separators.

IsStandardMode/IsProgrammerMode: Tracks calculator mode.

SelectedBase: Current number base (for Programmer mode).

UseOperationPrecedence: Enables/disables operator precedence.

# ✨ Features

## 🔢 Standard Calculator Features

✅ Basic arithmetic operations (+, -, ×, ÷)
✅ Memory operations: MC, MR, MS, M+, M-
✅ Advanced operations:

Square root

Square (x²)

Reciprocal (1/x)

Negate (+/-)

Percentage (%)
✅ Clear and backspace functionality

## 💻 Programmer Calculator Features

✅ Number base conversions: Decimal, Binary, Octal, Hexadecimal
✅ Bitwise operations: AND, OR, XOR, NOT, Shift Left, Shift Right
✅ Programmer-specific functions

## 🔧 General Features

✅ Keyboard support for operations
✅ Copy/Cut/Paste functionality
✅ Settings persistence (remembers user preferences)
✅ Error handling with user-friendly messages
✅ Digit grouping option (e.g., 1,000,000 vs 1000000)
✅ Operation precedence toggle (e.g., 2 + 3 × 4 → 14 or 20)

# 🛠️ Implementation Details

## ⚡ Command Pattern

All operations are implemented as commands.

Commands are initialized in InitializeCommands().

Each command has its own execution method.

Commands are bound to UI elements via XAML.

## ⚙️ Settings Management

Settings are loaded on startup.

Preferences are saved automatically.

Tracks:

Digit grouping preference

Calculator mode (Standard/Programmer)

Selected number base

Operation precedence setting

## 🚨 Error Handling

Comprehensive error management throughout.

Displays user-friendly messages.

Ensures graceful recovery from errors.

## 🔄 Data Binding & UI Updates

Uses WPF data binding for real-time updates.

Implements INotifyPropertyChanged to reflect changes.

Uses value converters for number formatting.

## 🧠 Memory Management

Supports multiple memory slots.

Memory operations: Store, Recall, Clear, Add, Subtract.

Observable memory values for UI synchronization.
