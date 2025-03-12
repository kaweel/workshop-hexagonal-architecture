using Xunit;
using Common.Helper;

namespace Common.Test;

public class DateTimeHelperTest
{
    [Theory]
    [InlineData("2025-02-17T00:00:00Z", false)]
    [InlineData("2025-02-18T00:00:00Z", false)]
    [InlineData("2025-02-19T00:00:00Z", false)]
    [InlineData("2025-02-20T00:00:00Z", false)]
    [InlineData("2025-02-21T00:00:00Z", false)]
    [Trait("Category","UnitTest")]
    public void IsWeekend_When_CurrentDateIsWorkingDate_ShouldReturnFalse(string strDate, bool expected)
    {
        var dateTime = DateTime.Parse(strDate).ToUniversalTime();

        var actual = DateTimeHelper.IsWeekend(dateTime);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-02-15T00:00:00Z", true)]
    [InlineData("2025-02-16T00:00:00Z", true)]
    [InlineData("2025-02-22T00:00:00Z", true)]
    [InlineData("2025-02-23T00:00:00Z", true)]
    [Trait("Category","UnitTest")]
    public void IsWeekend_When_CurrentDateIsNotWorkingDate_ShouldReturnTrue(string strDate, bool expected)
    {
        var dateTime = DateTime.Parse(strDate).ToUniversalTime();

        var actual = DateTimeHelper.IsWeekend(dateTime);

        Assert.Equal(expected, actual);
    }
}