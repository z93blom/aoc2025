using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Commands;

public record struct ArgumentExpressions([StringSyntax(StringSyntaxAttribute.Regex)] params string[] Expressions);