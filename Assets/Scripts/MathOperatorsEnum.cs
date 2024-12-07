
using System;

public enum MathOperatorsEnum
{
    SUBTRACTION = 0,
    ADDITION = 1,
    MULTIPLICATION = 2,
}

public static class MathOperatorsEnumExtensions
{
    public static string ToSymbol(this MathOperatorsEnum mathOperator)
    {
        return mathOperator switch
        {
            MathOperatorsEnum.SUBTRACTION => "-",
            MathOperatorsEnum.ADDITION => "+",
            MathOperatorsEnum.MULTIPLICATION => "x",
            _ => throw new ArgumentOutOfRangeException(nameof(mathOperator), $"Unsupported math operator: {mathOperator}")
        };
    }
}