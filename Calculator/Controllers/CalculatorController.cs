using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    public CalculatorController()
    {
    }

    [HttpGet()]
    public string Get([FromQuery] string? m, [FromQuery] string? h)
    {
        return new Calculator().Calc(m, h).ToString();
    }
}

public class Calculator
{
    public BMI Calc(string? strM, string? strH)
    {
        double m = ConvertToNumber(strM, "Mass");
        double h = ConvertToNumber(strH, "Height");

        double index = m / Math.Pow(h, 2.0);
        return GetBMI(index);
    }

    private BMI GetBMI(double index)
    {
        if (index < 18.5)
            return BMI.Under;
        if (index >= 18.5 && index < 25.0)
            return BMI.Normal;
        if (index >= 25.0 && index < 30)
            return BMI.Over;
        if (index >= 30.0 && index < 35)
            return BMI.Obese;
        return BMI.Extrem;
    }

    public static double ConvertToNumber(string? strValue, string name)
    {
        strValue = strValue?.Replace(",", ".");
        double result = 0;
        if (!double.TryParse(strValue, CultureInfo.InvariantCulture, out result))
        {
            throw new InvalidOperationException(
                $"Incorrect value for name {name}. Service can not convert {strValue} to double type");
        }

        if (result <= 0)
        {
            throw new ArgumentException($"Value shoud be greather 0, but side {name} has value {result}");
        }

        return result;
    }
}

public enum BMI
{
    Under,
    Normal,
    Over,
    Obese,
    Extrem
}
