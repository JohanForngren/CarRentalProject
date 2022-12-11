using DomainLibrary.CarRentalPeriod.Models;

namespace DomainLibrary.CarRentalPrice;

public interface ICarRentalPriceService
{
    public int CalculateTotalPriceInMinorCurrency(ICarRentalPeriodStartedModel carRentalPeriodStartedModel,
        TimeSpan totalRentalPeriodTimeSpan,
        int totalRentalPeriodDistanceInKilometers);

    public Task<int> GetBaseRatePerDayInMinorCurrencyAsync();
    public Task<int> GetBaseRatePerKilometerInMinorCurrencyAsync();
}