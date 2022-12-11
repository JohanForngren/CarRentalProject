namespace DomainLibrary.CarRentalPeriod.Models;

public sealed class CarRentalPeriodReturnedModel : CarRentalPeriodBaseModel, ICarRentalPeriodReturnedModel
{
    public required TimeSpan TotalRentalPeriodTimeSpan { get; init; }
    public required int TotalRentalPeriodDistanceInKilometers { get; init; }
    public required int TotalPriceInMinorCurrency { get; init; }
}