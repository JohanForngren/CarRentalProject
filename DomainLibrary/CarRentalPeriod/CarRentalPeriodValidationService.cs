using DomainLibrary.Helpers;

namespace DomainLibrary.CarRentalPeriod;

public sealed class CarRentalPeriodValidationService : ICarRentalPeriodValidationService
{
    private readonly IPersonalInformationNumberHelper _personalInformationNumberHelper;

    public CarRentalPeriodValidationService(IPersonalInformationNumberHelper personalInformationNumberHelper)
    {
        _personalInformationNumberHelper = personalInformationNumberHelper;
    }


    public void ValidatePersonalInformationNumber(string personalIdentityNumber)
    {
        if (!_personalInformationNumberHelper.ValidatePersonalInformationNumber(personalIdentityNumber))
            throw new ArgumentException("Non-valid personal information number", nameof(personalIdentityNumber));
    }

    public void ValidateCarRegistrationNumber(string carRegistrationNumber)
    {
        if (string.IsNullOrWhiteSpace(carRegistrationNumber))
            throw new ArgumentException("Cannot be null or whitespace",
                nameof(carRegistrationNumber));
    }

    public void ValidateBookingNumber(string bookingNumber)
    {
        if (string.IsNullOrWhiteSpace(bookingNumber))
            throw new ArgumentException("Cannot be null or whitespace", nameof(bookingNumber));
    }

    public void ValidateTimeStamp(DateTime timeStamp)
    {
        if (timeStamp > DateTime.Now)
            throw new ArgumentException("Future timestamps are not allowed", nameof(timeStamp));
    }

    public void ValidateOdometerAtStartOfRentalPeriod(int odometerAtStartOfRentalPeriod)
    {
        if (odometerAtStartOfRentalPeriod < 0)
            throw new ArgumentException("Non-positive odometer value", nameof(odometerAtStartOfRentalPeriod));
    }

    public void ValidateRentalPeriodReturnTimeStamp(DateTime rentalPeriodStart,
        DateTime rentalPeriodStartEnd)
    {
        if (rentalPeriodStartEnd <= rentalPeriodStart)
            throw new ArgumentException("Return timestamp must be later than start timestamp",
                nameof(rentalPeriodStartEnd));
        if (rentalPeriodStartEnd > DateTime.Now)
            throw new ArgumentException("Future returns not allowed", nameof(rentalPeriodStartEnd));
    }

    public void ValidateOdometerAtReturnOfRentalPeriod(int odometerAtStart, int odometerAtReturn)
    {
        if (odometerAtReturn < odometerAtStart)
            throw new ArgumentException("Odometer return value cannot be less than odometer start value",
                nameof(odometerAtReturn));
    }
}