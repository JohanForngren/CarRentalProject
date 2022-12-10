namespace DomainLibrary.CarRentalPeriod.Models;

public interface ICarRentalPeriodBaseModel
{
    string CarRentalPriceEquation { get; }

    // ReSharper disable CommentTypo
    /// <summary>
    ///     basDygnsHyra
    /// </summary>
    // ReSharper restore CommentTypo
    int BaseRatePerDayInMinorCurrency { get; }

    // ReSharper disable CommentTypo
    /// <summary>
    ///     basKmPris
    /// </summary>
    // ReSharper restore CommentTypo
    int BaseRatePerKilometerInMinorCurrency { get; }
}