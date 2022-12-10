namespace DomainLibrary.CarRentalPeriod.Models;

public class CarRentalPeriodReturnedModel : CarRentalPeriodBaseModel
{
    public required TimeSpan TotalRentalPeriodTimeSpan { get; init; }
    public required int TotalRentalPeriodDistanceInKilometers { get; init; }
    public required int TotalPriceInMinorCurrency { get; init; }
}