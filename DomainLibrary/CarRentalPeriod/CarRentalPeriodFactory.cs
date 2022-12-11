using DomainLibrary.CarRentalPeriod.Models;
using DomainLibrary.CarRentalPrice;
using DomainLibrary.CarRentalTypes;

namespace DomainLibrary.CarRentalPeriod;

public sealed class CarRentalPeriodFactory : ICarRentalPeriodFactory
{
    private readonly ICarRentalPeriodValidationService _carRentalPeriodValidationService;
    private readonly ICarRentalPriceService _carRentalPriceService;
    private readonly ICarRentalTypesService _carRentalTypesService;

    public CarRentalPeriodFactory(ICarRentalPeriodValidationService carRentalPeriodValidationService,
        ICarRentalTypesService carRentalTypesService, ICarRentalPriceService carRentalPriceService)
    {
        _carRentalPeriodValidationService = carRentalPeriodValidationService;
        _carRentalTypesService = carRentalTypesService;
        _carRentalPriceService = carRentalPriceService;
    }

    public async Task<ICarRentalPeriodStartedModel> GetCarRentalPeriodStartedModelAsync(string bookingNumber,
        string carRegistrationNumber, string personalIdentityNumber, string carRentalType,
        int odometerAtStartOfRentalPeriod,
        DateTime startOfRentalPeriod)
    {
        _carRentalPeriodValidationService.ValidateBookingNumber(bookingNumber);
        _carRentalPeriodValidationService.ValidateCarRegistrationNumber(carRegistrationNumber);
        _carRentalPeriodValidationService.ValidatePersonalInformationNumber(personalIdentityNumber);
        _carRentalPeriodValidationService.ValidateOdometerAtStartOfRentalPeriod(odometerAtStartOfRentalPeriod);
        _carRentalPeriodValidationService.ValidateTimeStamp(startOfRentalPeriod);

        // This will throw KeyNotFoundException if carRentalType is invalid, no need to validate it separately
        var carRentalPriceEquation = await _carRentalTypesService.GetCarRentalTypePriceEquationAsync(carRentalType);

        ICarRentalPeriodStartedModel carRentalPeriodStartedModel = new CarRentalPeriodStartedModel
        {
            BookingNumber = bookingNumber,
            CarRegistrationNumber = carRegistrationNumber,
            PersonalIdentityNumber = personalIdentityNumber,
            StartOfRentalPeriod = startOfRentalPeriod,
            OdometerAtStartOfRentalPeriod = odometerAtStartOfRentalPeriod,
            CarRentalType = carRentalType,
            CarRentalPriceEquation = carRentalPriceEquation,
            BaseRatePerDayInMinorCurrency = await _carRentalPriceService.GetBaseRatePerDayInMinorCurrencyAsync(),
            BaseRatePerKilometerInMinorCurrency =
                await _carRentalPriceService.GetBaseRatePerKilometerInMinorCurrencyAsync()
        };
        return carRentalPeriodStartedModel;
    }

    public async Task<ICarRentalPeriodStartedModel> GetCarRentalPeriodStartedModelAsync(string bookingNumber,
        string carRegistrationNumber,
        string personalIdentityNumber, string carRentalType, int odometerAtStartOfRentalPeriod)
    {
        var startOfRentalPeriod = DateTime.Now;
        return await GetCarRentalPeriodStartedModelAsync(bookingNumber,
            carRegistrationNumber,
            personalIdentityNumber, carRentalType, odometerAtStartOfRentalPeriod, startOfRentalPeriod);
    }

    public Task<ICarRentalPeriodReturnedModel> GetCarRentalPeriodReturnedModelAsync(
        ICarRentalPeriodStartedModel carRentalPeriodStartedModel, DateTime endOfRentalPeriod,
        int odometerAtReturn)
    {
        _carRentalPeriodValidationService.ValidateRentalPeriodReturnTimeStamp(
            carRentalPeriodStartedModel.StartOfRentalPeriod, endOfRentalPeriod);
        var totalRentalPeriodTimeSpan = CalculateTotalRentalPeriod(carRentalPeriodStartedModel, endOfRentalPeriod);

        _carRentalPeriodValidationService.ValidateOdometerAtReturnOfRentalPeriod(
            carRentalPeriodStartedModel.OdometerAtStartOfRentalPeriod, odometerAtReturn);
        var totalRentalPeriodDistanceInKilometers =
            CalculateTotalRentalPeriodDistanceInKilometers(carRentalPeriodStartedModel, odometerAtReturn);

        var totalPriceInMinorCurrency = _carRentalPriceService.CalculateTotalPriceInMinorCurrency(
            carRentalPeriodStartedModel,
            totalRentalPeriodTimeSpan, totalRentalPeriodDistanceInKilometers);

        ICarRentalPeriodReturnedModel carRentalPeriodReturnedModel = new CarRentalPeriodReturnedModel
        {
            TotalRentalPeriodTimeSpan = totalRentalPeriodTimeSpan,
            TotalRentalPeriodDistanceInKilometers = totalRentalPeriodDistanceInKilometers,
            TotalPriceInMinorCurrency = totalPriceInMinorCurrency,

            // Mapping values from carRentalPeriodStartedModel:
            BookingNumber = carRentalPeriodStartedModel.BookingNumber,
            CarRegistrationNumber = carRentalPeriodStartedModel.CarRegistrationNumber,
            PersonalIdentityNumber = carRentalPeriodStartedModel.PersonalIdentityNumber,
            CarRentalType = carRentalPeriodStartedModel.CarRentalType,
            CarRentalPriceEquation = carRentalPeriodStartedModel.CarRentalPriceEquation,
            StartOfRentalPeriod = carRentalPeriodStartedModel.StartOfRentalPeriod,
            OdometerAtStartOfRentalPeriod = carRentalPeriodStartedModel.OdometerAtStartOfRentalPeriod,
            BaseRatePerDayInMinorCurrency = carRentalPeriodStartedModel.BaseRatePerDayInMinorCurrency,
            BaseRatePerKilometerInMinorCurrency = carRentalPeriodStartedModel.BaseRatePerKilometerInMinorCurrency
        };

        return Task.FromResult(carRentalPeriodReturnedModel);
    }

    private static int CalculateTotalRentalPeriodDistanceInKilometers(
        ICarRentalPeriodStartedModel carRentalPeriodStartedModel, int odometerAtReturn)
    {
        var totalRentalPeriodDistanceInKilometers =
            odometerAtReturn - carRentalPeriodStartedModel.OdometerAtStartOfRentalPeriod;
        return totalRentalPeriodDistanceInKilometers;
    }

    private static TimeSpan CalculateTotalRentalPeriod(ICarRentalPeriodStartedModel carRentalPeriodStartedModel,
        DateTime endOfRentalPeriod)
    {
        var totalRentalPeriodTimeSpan =
            endOfRentalPeriod - carRentalPeriodStartedModel.StartOfRentalPeriod;
        return totalRentalPeriodTimeSpan;
    }
}