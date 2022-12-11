namespace DomainLibrary.CarRentalPeriod;

public interface ICarRentalPeriodValidationService
{
    void ValidateBookingNumber(string bookingNumber);
    void ValidateCarRegistrationNumber(string carRegistrationNumber);
    void ValidateOdometerAtReturnOfRentalPeriod(int odometerAtStart, int odometerAtReturn);
    void ValidateOdometerAtStartOfRentalPeriod(int odometerAtStartOfRentalPeriod);
    void ValidatePersonalInformationNumber(string personalIdentityNumber);
    void ValidateRentalPeriodReturnTimeStamp(DateTime rentalPeriodStart, DateTime rentalPeriodStartEnd);
    void ValidateTimeStamp(DateTime timeStamp);
}