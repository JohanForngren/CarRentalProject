using DomainLibrary.CarRentalTypes;

namespace ExampleImplementations;

public sealed class DummyCarRentalTypesProvider : ICarRentalTypesProvider
{
    public Task<string> GetPriceEquationAsync(string carRentalType)
    {
        var carRentalTypes = GetCarRentalTypes().Result;
        if (carRentalTypes.TryGetValue(carRentalType, out var value)) return Task.FromResult(value);

        throw new KeyNotFoundException("Car rental type not found");
    }

    private static Task<Dictionary<string, string>> GetCarRentalTypes()
    {
        Dictionary<string, string> carRentalTypes = new()
        {
            // ReSharper disable StringLiteralTypo
            {"Småbil", "basDygnsHyra * antalDygn"},
            {"Kombi", "basDygnsHyra * antalDygn * 1.3 + basKmPris * antalKm"},
            {"Lastbil", "basDygnsHyra * antalDygn * 1.5 + basKmPris * antalKm * 1.5"}
            // ReSharper restore StringLiteralTypo
        };
        return Task.FromResult(carRentalTypes);
    }
}