using AngouriMath;
using DomainLibrary.CarRentalPeriod.Models;
using DomainLibrary.CarRentalPrice;

namespace ExampleImplementations;

public sealed class CarRentalPriceService : ICarRentalPriceService
{
    public int CalculateTotalPriceInMinorCurrency(ICarRentalPeriodStartedModel carRentalPeriodStartedModel,
        TimeSpan totalRentalPeriodTimeSpan,
        int totalRentalPeriodDistanceInKilometers)
    {
        Entity entity = carRentalPeriodStartedModel.CarRentalPriceEquation;

        // This implementation assumes that we want to charge by fractional days
        // If started days is desired, just use Math.Ceiling() on TotalDays
        var totalDaysFractional = totalRentalPeriodTimeSpan.TotalDays;

        // ReSharper disable StringLiteralTypo
        var minorPrice = entity
            .Substitute("basDygnsHyra", carRentalPeriodStartedModel.BaseRatePerDayInMinorCurrency)
            .Substitute("antalDygn", totalDaysFractional)
            .Substitute("basKmPris", carRentalPeriodStartedModel.BaseRatePerKilometerInMinorCurrency)
            .Substitute("antalKm", totalRentalPeriodDistanceInKilometers)
            .EvalNumerical();
        // ReSharper restore StringLiteralTypo

        return (int) minorPrice;
    }

    public Task<int> GetBaseRatePerDayInMinorCurrencyAsync()
    {
        return Task.FromResult(19900);
    }

    public Task<int> GetBaseRatePerKilometerInMinorCurrencyAsync()
    {
        return Task.FromResult(200);
    }
}