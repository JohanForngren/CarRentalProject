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
    public void Valid_personal_information_number_throws_no_exceptions()
    {
        Assert.That(() => _sut.ValidatePersonalInformationNumber(ValidPersonalInformationNumber),
            Throws.Nothing);
    }

    [Test]
    public void Invalid_personal_information_number_throws_ArgumentException()
    {
        Assert.That(() => _sut.ValidatePersonalInformationNumber("invalid"), Throws.ArgumentException);
    }

    [Test]
    public void Valid_car_registration_number_throws_no_exceptions()
    {
        const string validArgument = "valid";
        Assert.That(() => _sut.ValidateCarRegistrationNumber(validArgument), Throws.Nothing);
    }

    [Test]
    public void Invalid_car_registration_number_throws_ArgumentException()
    {
        Assert.That(() => _sut.ValidateCarRegistrationNumber(""), Throws.ArgumentException);
    }

    [Test]
    public void Valid_booking_number_throws_no_exceptions()
    {
        const string validArgument = "valid";
        Assert.That(() => _sut.ValidateBookingNumber(validArgument), Throws.Nothing);
    }

    [Test]
    public void Invalid_booking_number_throws_ArgumentException()
    {
        Assert.That(() => _sut.ValidateBookingNumber(""), Throws.ArgumentException);
    }

    [Test]
    public void Valid_timestamp_throws_no_exceptions()
    {
        Assert.That(() => _sut.ValidateTimeStamp(ValidTestCaseData.Now), Throws.Nothing);
    }

    [Test]
    public void Timestamps_in_the_future_throws_ArgumentException()
    {
        Assert.Multiple(() =>
        {
            Assert.That(() => _sut.ValidateTimeStamp(ValidTestCaseData.Now.AddYears(1000)), Throws.ArgumentException);
            Assert.That(() =>
                _sut.ValidateRentalPeriodReturnTimeStamp(ValidTestCaseData.Now,
                    ValidTestCaseData.Now.Add(TimeSpan.FromHours(1))), Throws.ArgumentException);
        });
    }

    [Test]
    public void Valid_return_timestamps_throws_no_exceptions()
    {
        Assert.That(
            () => _sut.ValidateRentalPeriodReturnTimeStamp(
                ValidTestCaseData.Now.Subtract(TimeSpan.FromHours(25)),
                ValidTestCaseData.Now), Throws.Nothing);
    }

    [Test]
    public void Return_timestamp_not_later_than_start_timestamp_throws_ArgumentException()
    {
        Assert.Multiple(() =>
        {
            Assert.That(() =>
                    _sut.ValidateRentalPeriodReturnTimeStamp(ValidTestCaseData.Now, ValidTestCaseData.Now),
                Throws.ArgumentException);
            Assert.That(() =>
                _sut.ValidateRentalPeriodReturnTimeStamp(ValidTestCaseData.Now,
                    ValidTestCaseData.Now.Subtract(TimeSpan.FromHours(1))), Throws.ArgumentException);
        });
    }

    [Test]
    [TestCase(100, 200)]
    [TestCase(100, 100)]
    public void Valid_return_odometer_value_throws_no_exceptions(int validOdometerAtStart, int validOdometerAtReturn)
    {
        Assert.That(
            () => _sut.ValidateOdometerAtReturnOfRentalPeriod(validOdometerAtStart,
                validOdometerAtReturn), Throws.Nothing);
    }

    [Test]
    public void Odometer_Value_is_smaller_than_start_value_throws_ArgumentException()
    {
        Assert.That(() =>
            _sut.ValidateOdometerAtReturnOfRentalPeriod(200, 100), Throws.ArgumentException);
    }

    [Test]
    public void Valid_odometer_value_at_start_of_rental_period()
    {
        const int validArgument = 100;
        Assert.That(() => _sut.ValidateOdometerAtStartOfRentalPeriod(validArgument), Throws.Nothing);
    }

    [Test]
    public void Negative_odometer_start_value_throws_ArgumentException()
    {
        Assert.That(() => _sut.ValidateOdometerAtStartOfRentalPeriod(-1), Throws.ArgumentException);
    }
}