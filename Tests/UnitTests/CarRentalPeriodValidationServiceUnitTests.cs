using DomainLibrary.CarRentalPeriod;
using DomainLibrary.Helpers;
using Moq;

namespace Tests.UnitTests;

[TestFixture]
public sealed class CarRentalPeriodValidationServiceUnitTests
{
    [SetUp]
    public void SetUp()
    {
        var iPersonalInformationNumberHelperMock = new Mock<IPersonalInformationNumberHelper>();
        iPersonalInformationNumberHelperMock
            .Setup(mock => mock.ValidatePersonalInformationNumber(ValidPersonalInformationNumber))
            .Returns(true);
        iPersonalInformationNumberHelperMock.Setup(mock =>
                mock.ValidatePersonalInformationNumber(It.IsNotIn(ValidPersonalInformationNumber)))
            .Returns(false);
        _sut = new CarRentalPeriodValidationService(iPersonalInformationNumberHelperMock.Object);
    }

    private const string ValidPersonalInformationNumber = "valid";
    private ICarRentalPeriodValidationService _sut = null!;

    [Test]
    public void Return_valid_personal_information_number()
    {
        Assert.That(() => _sut.ValidatePersonalInformationNumber(ValidPersonalInformationNumber),
            Is.EqualTo(ValidPersonalInformationNumber));
    }

    [Test]
    public void Invalid_personal_information_number_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.ValidatePersonalInformationNumber("invalid"));
    }

    [Test]
    public void Return_valid_car_registration_number()
    {
        const string validArgument = "valid";
        Assert.That(() => _sut.ValidateCarRegistrationNumber(validArgument), Is.EqualTo(validArgument));
    }

    [Test]
    public void Invalid_car_registration_number_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.ValidateCarRegistrationNumber(""));
    }

    [Test]
    public void Return_valid_booking_number()
    {
        const string validArgument = "valid";
        Assert.That(() => _sut.ValidateBookingNumber(validArgument), Is.EqualTo(validArgument));
    }

    [Test]
    public void Invalid_booking_number_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.ValidateBookingNumber(""));
    }

    [Test]
    public void Return_valid_timestamp()
    {
        Assert.That(() => _sut.ValidateTimeStamp(ValidTestCaseData.Now), Is.EqualTo(ValidTestCaseData.Now));
    }

    [Test]
    public void Now_is_returned_if_timestamp_argument_is_null()
    {
        Assert.That(() => _sut.ValidateTimeStamp(null), Is.EqualTo(DateTime.Now).Within(1).Seconds);
    }

    [Test]
    public void Timestamps_in_the_future_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.ValidateTimeStamp(ValidTestCaseData.Now.AddYears(1000)));
        Assert.Throws<ArgumentException>(() =>
            _sut.ValidateAndCalculateTotalRentalPeriodTimeSpan(ValidTestCaseData.Now,
                ValidTestCaseData.Now.Add(TimeSpan.FromHours(1))));
    }

    [Test]
    public void Timespans_are_calculated()
    {
        Assert.That(
            () => _sut.ValidateAndCalculateTotalRentalPeriodTimeSpan(
                ValidTestCaseData.Now.Subtract(TimeSpan.FromHours(25)),
                ValidTestCaseData.Now), Is.EqualTo(TimeSpan.FromHours(25)));
    }

    [Test]
    public void Return_timestamp_not_later_than_start_timestamp_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _sut.ValidateAndCalculateTotalRentalPeriodTimeSpan(ValidTestCaseData.Now, ValidTestCaseData.Now));
        Assert.Throws<ArgumentException>(() =>
            _sut.ValidateAndCalculateTotalRentalPeriodTimeSpan(ValidTestCaseData.Now,
                ValidTestCaseData.Now.Subtract(TimeSpan.FromHours(1))));
    }

    [Test]
    public void Total_rental_distances_in_kilometers_are_calculated()
    {
        const int validOdometerAtStart = 100;
        const int validOdometerAtReturn = 100;
        Assert.Multiple(() =>
        {
            Assert.That(
                () => _sut.ValidateAndCalculateTotalRentalPeriodDistanceInKilometers(validOdometerAtStart,
                    validOdometerAtReturn), Is.EqualTo(validOdometerAtReturn - validOdometerAtStart));
            Assert.That(
                () => _sut.ValidateAndCalculateTotalRentalPeriodDistanceInKilometers(validOdometerAtStart,
                    validOdometerAtReturn + 100), Is.EqualTo(validOdometerAtReturn - validOdometerAtStart + 100));
        });
    }

    [Test]
    public void Odometer_return_value_is_smaller_than_start_value_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _sut.ValidateAndCalculateTotalRentalPeriodDistanceInKilometers(200, 100));
    }

    [Test]
    public void Return_valid_odometer_value_at_start_of_rental_period()
    {
        const int validArgument = 100;
        Assert.That(() => _sut.ValidateOdometerAtStartOfRentalPeriod(validArgument), Is.EqualTo(validArgument));
    }

    [Test]
    public void Negative_odometer_start_value_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.ValidateOdometerAtStartOfRentalPeriod(-1));
    }
}