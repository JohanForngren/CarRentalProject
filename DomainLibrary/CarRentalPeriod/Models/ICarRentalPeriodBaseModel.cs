namespace DomainLibrary.CarRentalPeriod.Models;

public interface ICarRentalPeriodBaseModel
{
    string BookingNumber { get; init; }
    string CarRegistrationNumber { get; init; }
    string PersonalIdentityNumber { get; init; }
    string CarRentalType { get; init; }
    DateTime DateTimeAtStartOfRentalPeriod { get; init; }
    int OdometerAtStartOfRentalPeriod { get; init; }
    string CarRentalPriceEquation { get; init; }

    // ReSharper disable CommentTypo
    /// <summary>
    ///     basDygnsHyra
    /// </summary>
    // ReSharper restore CommentTypo
    int BaseRatePerDayInMinorCurrency { get; init; }

    // ReSharper disable CommentTypo
    /// <summary>
    ///     basKmPris
    /// </summary>
    // ReSharper restore CommentTypo
    int BaseRatePerKilometerInMinorCurrency { get; init; }
}