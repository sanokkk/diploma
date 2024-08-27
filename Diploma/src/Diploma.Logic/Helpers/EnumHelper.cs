using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Diploma.Domain.Models;

namespace Diploma.Logic.Helpers;

public static class EnumHelper
{
    public static string GetNormalizedName(string enumName)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var ch in enumName)
        {
            if (char.IsUpper(ch) && builder.Length > 0)
            {
                builder.Append(' ').Append(ch);
            }
            else
            {
                builder.Append(ch);
            }
        }

        return builder.ToString();
    }

    public static ConditionState GetStateByIndex(int index)
    {
        switch (index)
        {
            case 1:
                return ConditionState.Идеальное;
            case 2:
                return ConditionState.Хорошее;
            case 3:
                return ConditionState.Нормальное;
            case 4:
                return ConditionState.Плохое;
            case 5:
            default:
                return ConditionState.Чрезвычайное;
        }
    }
}