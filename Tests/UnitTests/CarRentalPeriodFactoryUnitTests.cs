using DomainLibrary.CarRentalPeriod;
using DomainLibrary.CarRentalPeriod.Models;
using DomainLibrary.CarRentalPrice;
using DomainLibrary.CarRentalTypes;
using Moq;

namespace Tests.UnitTests;

[TestFixture]
public sealed class CarRentalPeriodFactoryUnitTests
{
    [SetUp]
    public void SetUp()
    {
        var iCarRentalTypesServiceMock = new Mock<ICarRentalTypesService>();
        iCarRentalTypesServiceMock
            .Setup(mock => mock.GetCarRentalTypePriceEquationAsync(ValidTestCaseData.CarRentalType))
            .Returns(Task.FromResult(ValidCarRentalTypeEquation));
        iCarRentalTypesServiceMock.Setup(mock =>
                mock.GetCarRentalTypePriceEquationAsync(It.IsNotIn(ValidTestCaseData.CarRentalType)))
            .ThrowsAsync(new KeyNotFoundException());
        var iCarRentalPriceServiceMock = new Mock<ICarRentalPriceService>();
        iCarRentalPriceServiceMock.Setup(mock => mock.GetBaseRatePerDayInMinorCurrencyAsync())
            .ReturnsAsync(BaseRatePerDayInMinorCurrency);
        iCarRentalPriceServiceMock.Setup(mock => mock.GetBaseRatePerKilometerInMinorCurrencyAsync())
            .ReturnsAsync(BaseRatePerKilometerInMinorCurrency);
        iCarRentalPriceServiceMock
            .Setup(mock => mock.CalculateTotalPriceInMinorCurrency(It.IsAny<ICarRentalPeriodStartedModel>(),
                It.IsAny<TimeSpan>(), It.IsAny<int>())).Returns(TotalPriceInMinorCurrency);

        var iCarRentalPeriodValidationServiceMock = new Mock<ICarRentalPeriodValidationService>();

        _sut =
            new CarRentalPeriodFactory(iCarRentalPeriodValidationServiceMock.Object, iCarRentalTypesServiceMock.Object,
                iCarRentalPriceServiceMock.Object);
    }

    private ICarRentalPeriodFactory _sut = null!;
    private const string ValidCarRentalTypeEquation = "valid equation";
    private const int BaseRatePerDayInMinorCurrency = 100;
    private const int BaseRatePerKilometerInMinorCurrency = 20;
    private const int TotalPriceInMinorCurrency = 123456789;

    [Test]
    public void Invalid_car_rental_type_throws_KeyNotFoundException()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _sut.GetCarRentalPeriodStartedModelAsync(ValidTestCaseData.BookingNumber,
                ValidTestCaseData.CarRegistrationNumber, ValidTestCaseData.PersonalIdentityNumber, "invalid",
                ValidTestCaseData.OdometerAtStart, ValidTestCaseData.Now));
    }

    [Test]
    public async Task Car_rental_period_start_values_maps()
    {
        var model = await _sut.GetCarRentalPeriodStartedModelAsync(ValidTestCaseData.BookingNumber,
            ValidTestCaseData.CarRegistrationNumber,
            ValidTestCaseData.PersonalIdentityNumber, ValidTestCaseData.CarRentalType,
            ValidTestCaseData.OdometerAtStart,
            ValidTestCaseData.Now);

        Assert.Multiple(() =>
        {
            Assert.That(model.StartOfRentalPeriod, Is.EqualTo(ValidTestCaseData.Now));
            Assert.That(model.BookingNumber, Is.EqualTo(ValidTestCaseData.BookingNumber));
            Assert.That(model.CarRegistrationNumber, Is.EqualTo(ValidTestCaseData.CarRegistrationNumber));
            Assert.That(model.PersonalIdentityNumber, Is.EqualTo(ValidTestCaseData.PersonalIdentityNumber));
            Assert.That(model.CarRentalType, Is.EqualTo(ValidTestCaseData.CarRentalType));
            Assert.That(model.OdometerAtStartOfRentalPeriod, Is.EqualTo(ValidTestCaseData.OdometerAtStart));
            Assert.That(model.CarRentalPriceEquation, Is.EqualTo(ValidCarRentalTypeEquation));
            Assert.That(model.BaseRatePerDayInMinorCurrency, Is.EqualTo(BaseRatePerDayInMinorCurrency));
            Assert.That(model.BaseRatePerKilometerInMinorCurrency, Is.EqualTo(BaseRatePerKilometerInMinorCurrency));
        });
    }

    [Test]
    public async Task Car_rental_period_return_values_maps()
    {
        var startTimeStamp = ValidTestCaseData.Now.Subtract(TimeSpan.FromHours(25));
        var totalRentalPeriodTimeSpan = ValidTestCaseData.Now - startTimeStamp;
        var carRentalPeriodStartedModel = await _sut.GetCarRentalPeriodStartedModelAsync(
            ValidTestCaseData.BookingNumber,
            ValidTestCaseData.CarRegistrationNumber,
            ValidTestCaseData.PersonalIdentityNumber, ValidTestCaseData.CarRentalType,
            ValidTestCaseData.OdometerAtStart,
            startTimeStamp);
        var model = await _sut.GetCarRentalPeriodReturnedModelAsync(carRentalPeriodStartedModel, ValidTestCaseData.Now,
            ValidTestCaseData.OdometerAtReturn);
        Assert.Multiple(() =>
        {
            Assert.That(model.TotalPriceInMinorCurrency, Is.EqualTo(TotalPriceInMinorCurrency));
            Assert.That(model.TotalRentalPeriodTimeSpan, Is.EqualTo(totalRentalPeriodTimeSpan));
            Assert.That(model.TotalRentalPeriodDistanceInKilometers,
                Is.EqualTo(ValidTestCaseData.ExpectedTotalRentalPeriodDistanceInKilometers));

            // Mappings from GetCarRentalPeriodStartedModelAsync:
            Assert.That(model.StartOfRentalPeriod, Is.EqualTo(startTimeStamp));
            Assert.That(model.BookingNumber, Is.EqualTo(ValidTestCaseData.BookingNumber));
            Assert.That(model.CarRegistrationNumber, Is.EqualTo(ValidTestCaseData.CarRegistrationNumber));
            Assert.That(model.PersonalIdentityNumber, Is.EqualTo(ValidTestCaseData.PersonalIdentityNumber));
            Assert.That(model.CarRentalType, Is.EqualTo(ValidTestCaseData.CarRentalType));
            Assert.That(model.OdometerAtStartOfRentalPeriod, Is.EqualTo(ValidTestCaseData.OdometerAtStart));
            Assert.That(model.CarRentalPriceEquation, Is.EqualTo(ValidCarRentalTypeEquation));
            Assert.That(model.BaseRatePerDayInMinorCurrency, Is.EqualTo(BaseRatePerDayInMinorCurrency));
            Assert.That(model.BaseRatePerKilometerInMinorCurrency, Is.EqualTo(BaseRatePerKilometerInMinorCurrency));
        });
    }

    [Test]
    public async Task Start_of_rental_periods_is_set_to_now_if_timestamp_argument_is_null()
    {
        var model = await _sut.GetCarRentalPeriodStartedModelAsync(
            ValidTestCaseData.BookingNumber,
            ValidTestCaseData.CarRegistrationNumber,
            ValidTestCaseData.PersonalIdentityNumber, ValidTestCaseData.CarRentalType,
            ValidTestCaseData.OdometerAtStart);
        Assert.That(() => model.StartOfRentalPeriod, Is.EqualTo(DateTime.Now).Within(1).Seconds);
    }
}