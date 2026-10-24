namespace Lab3.Core;

public sealed class UnknownCodeException(string what, string code)
    : Exception($"{what} с кодом '{code}' не существует");