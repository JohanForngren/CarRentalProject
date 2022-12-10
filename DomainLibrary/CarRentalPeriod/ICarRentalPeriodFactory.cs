using DomainLibrary.CarRentalPeriod.Models;

namespace DomainLibrary.CarRentalPeriod;

public interface ICarRentalPeriodFactory
{
    /// <inheritdoc cref="GetCarRentalPeriodReturnedModelAsync" />
    /// <exception cref="KeyNotFoundException">If carRentalType is not found.</exception>
    Task<CarRentalPeriodStartedModel> GetCarRentalPeriodStartedModelAsync(string bookingNumber,
        string carRegistrationNumber,
        string personalIdentityNumber, string carRentalType, int odometerAtStartOfRentalPeriod,
        DateTime? timeStamp = null);

    /// <note>This method will NOT clean data, i.e. <code>trim(argument)</code></note>
    /// <exception cref="ArgumentException">For invalid arguments, along with a (sort of) explanatory message.</exception>
    Task<CarRentalPeriodReturnedModel> GetCarRentalPeriodReturnedModelAsync(
        CarRentalPeriodStartedModel carRentalPeriodStartedModel, DateTime dateTimeAtEndOfRentalPeriod,
        int odometerAtReturn);
}