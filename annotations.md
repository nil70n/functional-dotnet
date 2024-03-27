# Functional C#

## What are Functions
A Function in functional programming is an instruction that has no scope and no global state.
- Output depends entirely on the input (No side effects)
- A pure function can call only pure functions, otherwise, the function itself is impure.

### First Class Function
A function that can be assigned to a variable, like a value

### Higher Order Functions
Accepts a function as an argument or return a function as a return value

## Static Imports
```c#
  using static FuncCsharp.OrderCalculator;
```
- A class can be imported statically to only import the static methods of that class
  - Enforces usage of static functions, leading to a better functional approach
  - Can lead to namespace pollution if used liberally

## Expression-bodied members
```c#
public int Total => Items.Sum(x => x.Price);
```
Make functions more concise

## Local Functions
```c#
public double DecimalRate {
  get {
    double Convert(int rate) => rate / 100;
    return Convert(Rate);
  }
}
```
- Local functions can help with organization
  - Useful if a function is called only in one place
  - The utility of local functions in functional programming is questionable

## Immutability
- Objects and values NEVER change and variables are NEVER reassigned
- Avoid mutations - value changes in-place
- Eliminates many complexities caused by mutating the state of an application
- Concurrency becomes easier
- Reduces unintended coupling

### Libraries and C# 9
- Immutable.NET
  - .NET Standard 1.3
  - .NET Core 1.0 and greater
  - .NET Framework 4.6 and greater
- System.Immutable.Collections
  - .NET 5.0 and greater
  - .NET Core 1.0 and greater
- Record Types, Init Only Setters, and With Expressions
  - C# 9.0/.NET 5.0

## Referential Transparency
- A function is referentially transparent if it can be replaced with its value without changing the program's behavior
- Function must be pure and use immutable data structures
