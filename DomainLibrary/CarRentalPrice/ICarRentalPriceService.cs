using DomainLibrary.CarRentalPeriod.Models;

namespace DomainLibrary.CarRentalPrice;

public interface ICarRentalPriceService
{
    public int CalculateTotalPriceInMinorCurrency(ICarRentalPeriodBaseModel carRentalPeriodBaseModel,
        TimeSpan totalRentalPeriodTimeSpan,
        int totalRentalPeriodDistanceInKilometers);

    public Task<int> GetBaseRatePerDayInMinorCurrencyAsync();
    public Task<int> GetBaseRatePerKilometerInMinorCurrencyAsync();
}