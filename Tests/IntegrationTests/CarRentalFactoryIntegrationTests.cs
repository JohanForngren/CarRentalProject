using DomainLibrary.CarRentalPeriod;
using DomainLibrary.CarRentalPrice;
using DomainLibrary.CarRentalTypes;
using DomainLibrary.Helpers;
using ExampleImplementations;

namespace Tests.IntegrationTests;

public sealed partial class CarRentalFactoryIntegrationTests
{
    private ICarRentalPeriodValidationService _carRentalPeriodValidationService = null!;
    private ICarRentalPriceService _carRentalPriceService = null!;
    private ICarRentalTypesService _carRentalTypesService = null!;
    private IPersonalInformationNumberHelper _personalInformationNumberHelper = null!;
    private CarRentalPriceService _priceHelper = null!;
    private CarRentalPeriodFactory _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _personalInformationNumberHelper =
            new PersonalInformationNumberHelper();
        _carRentalPriceService = new CarRentalPriceService();
        _carRentalPeriodValidationService = new CarRentalPeriodValidationService(_personalInformationNumberHelper);
        _carRentalTypesService = new CarRentalTypesService(new DummyCarRentalTypesProvider());
        _priceHelper = new CarRentalPriceService();
        _sut =
            new CarRentalPeriodFactory(_carRentalPeriodValidationService, _carRentalTypesService, _priceHelper);
    }


    [Test]
    [TestCaseSource(nameof(_invalidArgumentTestCases))]
    public void Invalid_argument_throws_ArgumentException(string bookingNumber, string carRegistrationNumber,
        string personalIdentityNumber, string carRentalType, int odometer, DateTime? timeStamp = null)
    {
        Assert.That(
            () => _sut.GetCarRentalPeriodStartedModelAsync(bookingNumber, carRegistrationNumber,
                personalIdentityNumber,
                carRentalType, odometer, timeStamp),
            Throws.ArgumentException);
    }


    [Test]
    [TestCaseSource(nameof(_mapsArgumentsAndCalculatesTotalsForReturnedCarReturnCarTestCases))]
    public async Task Maps_arguments_and_calculates_totals_for_returned_car(string carRentalType,
        Func<int, int, int> expectedTotalPriceFunction)
    {
        var carRentalPeriodStartedModel = await _sut.GetCarRentalPeriodStartedModelAsync(
            ValidTestCaseData.BookingNumber, ValidTestCaseData.CarRegistrationNumber,
            ValidTestCaseData.PersonalIdentityNumber, carRentalType, ValidTestCaseData.OdometerAtStart,
            ValidTestCaseData.TimeStampAtStart);

        var carRentalPeriodReturnedModel = await _sut.GetCarRentalPeriodReturnedModelAsync(carRentalPeriodStartedModel,
            ValidTestCaseData.TimeStampAtReturn, ValidTestCaseData.OdometerAtReturn);

        var carRentalTypePriceEquation =
            await _carRentalTypesService.GetCarRentalTypePriceEquationAsync(carRentalType);
        var baseRatePerDayInMinorCurrency = await _carRentalPriceService.GetBaseRatePerDayInMinorCurrencyAsync();
        var baseRatePerKilometerInMinorCurrency =
            await _carRentalPriceService.GetBaseRatePerKilometerInMinorCurrencyAsync();
        var expectedTotalPrice =
            expectedTotalPriceFunction(baseRatePerDayInMinorCurrency, baseRatePerKilometerInMinorCurrency);
        Assert.Multiple(() =>
        {
            Assert.That(carRentalPeriodReturnedModel.DateTimeAtStartOfRentalPeriod,
                Is.EqualTo(ValidTestCaseData.TimeStampAtStart));
            Assert.That(carRentalPeriodReturnedModel.BookingNumber, Is.EqualTo(ValidTestCaseData.BookingNumber));
            Assert.That(carRentalPeriodReturnedModel.CarRegistrationNumber,
                Is.EqualTo(ValidTestCaseData.CarRegistrationNumber));
            Assert.That(carRentalPeriodReturnedModel.PersonalIdentityNumber,
                Is.EqualTo(ValidTestCaseData.PersonalIdentityNumber));
            Assert.That(carRentalPeriodReturnedModel.CarRentalType, Is.EqualTo(carRentalType));
            Assert.That(carRentalPeriodReturnedModel.OdometerAtStartOfRentalPeriod,
                Is.EqualTo(ValidTestCaseData.OdometerAtStart));
            Assert.That(carRentalPeriodReturnedModel.CarRentalPriceEquation, Is.EqualTo(carRentalTypePriceEquation));
            Assert.That(carRentalPeriodReturnedModel.BaseRatePerDayInMinorCurrency,
                Is.EqualTo(baseRatePerDayInMinorCurrency));
            Assert.That(carRentalPeriodReturnedModel.BaseRatePerKilometerInMinorCurrency,
                Is.EqualTo(baseRatePerKilometerInMinorCurrency));
            Assert.That(carRentalPeriodReturnedModel.TotalRentalPeriodTimeSpan,
                Is.EqualTo(ValidTestCaseData.ExpectedTotalTimeSpan));
            Assert.That(carRentalPeriodReturnedModel.TotalRentalPeriodDistanceInKilometers,
                Is.EqualTo(ValidTestCaseData.ExpectedTotalRentalPeriodDistanceInKilometers));
            Assert.That(carRentalPeriodReturnedModel.TotalPriceInMinorCurrency, Is.EqualTo(expectedTotalPrice));
        });
    }
}