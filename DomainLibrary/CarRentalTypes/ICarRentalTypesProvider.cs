namespace DomainLibrary.CarRentalTypes;

public interface ICarRentalTypesProvider
{
    /// <exception cref="KeyNotFoundException">If carRentalType is not found.</exception>
    Task<string> GetPriceEquationAsync(string carRentalType);
}