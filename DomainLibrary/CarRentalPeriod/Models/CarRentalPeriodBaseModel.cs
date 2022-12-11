namespace DomainLibrary.CarRentalPeriod.Models;

public abstract class CarRentalPeriodBaseModel : ICarRentalPeriodBaseModel
{
    public required string BookingNumber { get; init; }
    public required string CarRegistrationNumber { get; init; }
    public required string PersonalIdentityNumber { get; init; }
    public required string CarRentalType { get; init; }
    public required DateTime StartOfRentalPeriod { get; init; }
    public int OdometerAtStartOfRentalPeriod { get; init; }
    public required string CarRentalPriceEquation { get; init; }
    public required int BaseRatePerDayInMinorCurrency { get; init; }
    public required int BaseRatePerKilometerInMinorCurrency { get; init; }
}