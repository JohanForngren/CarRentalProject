using CarRentalTypes;
using DomainLibrary.CarRentalPeriod.Models;
using DomainLibrary.CarRentalPrice;
using Moq;

namespace Tests.UnitTests;

[TestFixture]
public class CarRentalPriceServiceUnitTests
{
    [Test]
    public void Calculates_total_price_in_minor_currency()
    {
        ICarRentalPriceService sut = new CarRentalPriceService();

        // ReSharper disable StringLiteralTypo
        const string carRentalPriceEquation = "basDygnsHyra * antalDygn * 1.5 + basKmPris * antalKm * 1.5";
        // ReSharper restore StringLiteralTypo
        var totalRentalPeriodTimeSpan = TimeSpan.FromHours(25);
        const int totalRentalPeriodDistanceInKilometers = 20;
        var expectedTotalPrice = (int) (
            ValidTestCaseData.BaseRatePerDayInMinorCurrency * totalRentalPeriodTimeSpan.TotalDays * 1.5 +
            ValidTestCaseData.BaseRatePerKilometerInMinorCurrency * totalRentalPeriodDistanceInKilometers * 1.5);

        var carRentalPeriodStartedModel = new Mock<ICarRentalPeriodBaseModel>();
        carRentalPeriodStartedModel.SetupGet(mock => mock.BaseRatePerDayInMinorCurrency)
            .Returns(ValidTestCaseData.BaseRatePerDayInMinorCurrency);
        carRentalPeriodStartedModel.SetupGet(mock => mock.BaseRatePerKilometerInMinorCurrency)
            .Returns(ValidTestCaseData.BaseRatePerKilometerInMinorCurrency);
        carRentalPeriodStartedModel.SetupGet(mock => mock.CarRentalPriceEquation).Returns(carRentalPriceEquation);

        Assert.That(() => sut.CalculateTotalPriceInMinorCurrency(carRentalPeriodStartedModel.Object,
            totalRentalPeriodTimeSpan,
            totalRentalPeriodDistanceInKilometers), Is.EqualTo(expectedTotalPrice));
    }
}