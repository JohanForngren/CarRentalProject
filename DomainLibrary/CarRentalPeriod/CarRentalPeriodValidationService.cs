using DomainLibrary.Helpers;

namespace DomainLibrary.CarRentalPeriod;

public sealed class CarRentalPeriodValidationService : ICarRentalPeriodValidationService
{
    private readonly IPersonalInformationNumberHelper _personalInformationNumberHelper;

    public CarRentalPeriodValidationService(IPersonalInformationNumberHelper personalInformationNumberHelper)
    {
        _personalInformationNumberHelper = personalInformationNumberHelper;
    }


    public string ValidatePersonalInformationNumber(string personalIdentityNumber)
    {
        if (_personalInformationNumberHelper.ValidatePersonalInformationNumber(personalIdentityNumber))
            return personalIdentityNumber;

        throw new ArgumentException("Non-valid personal information number", nameof(personalIdentityNumber));
    }

    public string ValidateCarRegistrationNumber(string carRegistrationNumber)
    {
        if (string.IsNullOrWhiteSpace(carRegistrationNumber))
            throw new ArgumentException("Cannot be null or whitespace",
                nameof(carRegistrationNumber));

        return carRegistrationNumber;
    }

    public string ValidateBookingNumber(string bookingNumber)
    {
        if (string.IsNullOrWhiteSpace(bookingNumber))
            throw new ArgumentException("Cannot be null or whitespace", nameof(bookingNumber));

        return bookingNumber;
    }

    public DateTime ValidateTimeStamp(DateTime? timeStamp)
    {
        if (timeStamp == null) return DateTime.Now;

        if (timeStamp > DateTime.Now)
            throw new ArgumentException("Future timestamps are not allowed", nameof(timeStamp));

        return (DateTime) timeStamp;
    }

    public TimeSpan ValidateAndCalculateTotalRentalPeriodTimeSpan(DateTime dateTimeAtStartOfRentalPeriod,
        DateTime dateTimeAtEndOfRentalPeriod)
    {
        if (dateTimeAtEndOfRentalPeriod <= dateTimeAtStartOfRentalPeriod)
            throw new ArgumentException("End timestamp must be later than start timestamp",
                nameof(dateTimeAtEndOfRentalPeriod));
        if (dateTimeAtEndOfRentalPeriod > DateTime.Now)
            throw new ArgumentException("Future returns not allowed", nameof(dateTimeAtEndOfRentalPeriod));
        return dateTimeAtEndOfRentalPeriod - dateTimeAtStartOfRentalPeriod;
    }

    public int ValidateAndCalculateTotalRentalPeriodDistanceInKilometers(int odometerAtStart, int odometerAtReturn)
    {
        if (odometerAtReturn < odometerAtStart)
            throw new ArgumentException("Odometer return value cannot be less than odometer start value",
                nameof(odometerAtReturn));
        return odometerAtReturn - odometerAtStart;
    }

    public int ValidateOdometerAtStartOfRentalPeriod(int odometerAtStartOfRentalPeriod)
    {
        if (odometerAtStartOfRentalPeriod < 0)
            throw new ArgumentException("Non-positive odometer value", nameof(odometerAtStartOfRentalPeriod));
        return odometerAtStartOfRentalPeriod;
    }
}