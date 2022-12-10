namespace DomainLibrary.CarRentalTypes;

public interface ICarRentalTypesService
{
    /// <inheritdoc cref="ICarRentalTypesProvider.GetPriceEquationAsync" />
    Task<string> GetCarRentalTypePriceEquationAsync(string carType);
}