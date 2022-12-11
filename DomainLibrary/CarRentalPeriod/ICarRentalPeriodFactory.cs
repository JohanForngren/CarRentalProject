using DomainLibrary.CarRentalPeriod.Models;

namespace DomainLibrary.CarRentalPeriod;

public interface ICarRentalPeriodFactory
{
    /// <inheritdoc cref="GetCarRentalPeriodReturnedModelAsync" />
    /// <exception cref="KeyNotFoundException">If carRentalType is not found.</exception>
    Task<ICarRentalPeriodStartedModel> GetCarRentalPeriodStartedModelAsync(string bookingNumber,
        string carRegistrationNumber,
        string personalIdentityNumber, string carRentalType, int odometerAtStartOfRentalPeriod,
        DateTime startOfRentalPeriod);

    /// <summary>
    ///     This overload will sett StartOfRentalPeriod = DateTime.Now
    /// </summary>
    /// <inheritdoc cref="GetCarRentalPeriodReturnedModelAsync" />
    /// <exception cref="KeyNotFoundException">If carRentalType is not found.</exception>
    Task<ICarRentalPeriodStartedModel> GetCarRentalPeriodStartedModelAsync(string bookingNumber,
        string carRegistrationNumber,
        string personalIdentityNumber, string carRentalType, int odometerAtStartOfRentalPeriod);

    /// <note>This method will NOT clean data, i.e. <code>trim(argument)</code></note>
    /// <exception cref="ArgumentException">For invalid arguments, along with a (sort of) explanatory message.</exception>
    Task<ICarRentalPeriodReturnedModel> GetCarRentalPeriodReturnedModelAsync(
        ICarRentalPeriodStartedModel carRentalPeriodStartedModel, DateTime endOfRentalPeriod,
        int odometerAtReturn);
}