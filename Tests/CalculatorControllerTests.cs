using System.Net;
using FluentAssertions;

namespace Tests;

public class CalculatorControllerTests : BaseTests
{
    [TestCase("70", "1.75", HttpStatusCode.OK, "Normal")]
    [TestCase("50", "1.75", HttpStatusCode.OK, "Under")]
    [TestCase("90", "1.75", HttpStatusCode.OK, "Over")]
    [TestCase("105", "1.75", HttpStatusCode.OK, "Obese")]
    [TestCase("140", "1.75", HttpStatusCode.OK, "Extrem")]
    [TestCase("70,0", "1,75", HttpStatusCode.OK, "Normal")]
    [TestCase(" 70 ", "1.75", HttpStatusCode.OK, "Normal")]
    [TestCase("70", " 1.75 ", HttpStatusCode.OK, "Normal")]
    [TestCase(" ", "1.75", HttpStatusCode.InternalServerError, "Incorrect value for name Mass")]
    [TestCase("70", " ", HttpStatusCode.InternalServerError, "Incorrect value for name Height")]
    [TestCase(null, "1.75", HttpStatusCode.InternalServerError, "Incorrect value for name Mass")]
    [TestCase("70", null, HttpStatusCode.InternalServerError, "Incorrect value for name Height")]
    [TestCase("abc", "1.75", HttpStatusCode.InternalServerError, "Incorrect value for name Mass")]
    [TestCase("70", "xyz", HttpStatusCode.InternalServerError, "Incorrect value for name Height")]
    [TestCase("0", "1.75", HttpStatusCode.InternalServerError, "Value shoud be greather 0, but side Mass has value 0")]
    [TestCase("70", "0", HttpStatusCode.InternalServerError, "Value shoud be greather 0, but side Height has value 0")]
    [TestCase("-70", "1.75", HttpStatusCode.InternalServerError,
        "Value shoud be greather 0, but side Mass has value -70")]
    [TestCase("70", "-1.75", HttpStatusCode.InternalServerError,
        "Value shoud be greather 0, but side Height has value -1,75")]
    [TestCase("", "1.75", HttpStatusCode.InternalServerError, "Incorrect value for name Mass")]
    [TestCase("70", "", HttpStatusCode.InternalServerError, "Incorrect value for name Height")]
    public async Task Get_BMI_ReturnsExpectedResult(string? mass, string? height, HttpStatusCode expectedStatusCode,
        string expectedResponseSubstring)
    {
        // Act
        var response = await _client.GetAsync($"/Calculator?m={mass}&h={height}");
        var responseString = await response.Content.ReadAsStringAsync();
        var isResponseStringContainsExpected =
            responseString.Contains(expectedResponseSubstring, StringComparison.OrdinalIgnoreCase);

        // Assert
        response.StatusCode.Should().Be(expectedStatusCode);
        isResponseStringContainsExpected.Should().BeTrue();
    }

    [Test]
    public async Task Get_BMI_ShouldReturnInternalServerError_WhenParametersIsMissing()
    {
        // Act
        var response = await _client.GetAsync("/Calculator");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Test]
    public async Task Get_BMI_ShouldReturnInternalServerError_WhenHeightParameterIsMissing()
    {
        // Act
        var response = await _client.GetAsync("/Calculator?m=70");
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        responseString.Should().Contain("Incorrect value for name Height");
    }

    [Test]
    public async Task Get_BMI_ShouldReturnInternalServerError_WhenMassParameterIsMissing()
    {
        // Act
        var response = await _client.GetAsync("/Calculator?h=70");
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        responseString.Should().Contain("Incorrect value for name Mass");
    }
}