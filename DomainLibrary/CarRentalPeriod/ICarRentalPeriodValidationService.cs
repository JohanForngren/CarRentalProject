namespace DomainLibrary.CarRentalPeriod;

public interface ICarRentalPeriodValidationService
{
    string ValidatePersonalInformationNumber(string personalIdentityNumber);
    string ValidateCarRegistrationNumber(string carRegistrationNumber);
    string ValidateBookingNumber(string bookingNumber);

    /// <returns>DateTime.Now if timeStamp == null </returns>
    DateTime ValidateTimeStamp(DateTime? timeStamp);

    TimeSpan ValidateAndCalculateTotalRentalPeriodTimeSpan(DateTime dateTimeAtStartOfRentalPeriod,
        DateTime dateTimeAtEndOfRentalPeriod);

    int ValidateAndCalculateTotalRentalPeriodDistanceInKilometers(int odometerAtStart, int odometerAtReturn);
    int ValidateOdometerAtStartOfRentalPeriod(int odometerAtStartOfRentalPeriod);
}