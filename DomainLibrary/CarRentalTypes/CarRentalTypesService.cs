namespace DomainLibrary.CarRentalTypes;

public sealed class CarRentalTypesService : ICarRentalTypesService
{
    private readonly ICarRentalTypesProvider _carRentalTypesProvider;

    public CarRentalTypesService(ICarRentalTypesProvider carRentalTypesProvider)
    {
        _carRentalTypesProvider = carRentalTypesProvider;
    }

    public async Task<string> GetCarRentalTypePriceEquationAsync(string carType)
    {
        return await _carRentalTypesProvider.GetPriceEquationAsync(carType);
    }
}