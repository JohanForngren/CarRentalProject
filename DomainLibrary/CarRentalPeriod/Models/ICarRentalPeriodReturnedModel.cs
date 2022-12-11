namespace DomainLibrary.CarRentalPeriod.Models;

public interface ICarRentalPeriodReturnedModel : ICarRentalPeriodBaseModel
{
    TimeSpan TotalRentalPeriodTimeSpan { get; init; }
    int TotalRentalPeriodDistanceInKilometers { get; init; }
    int TotalPriceInMinorCurrency { get; init; }
}