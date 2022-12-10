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

    public async Task<CarRentalPeriodStartedModel> GetCarRentalPeriodStartedModelAsync(string bookingNumber,
        string carRegistrationNumber,
        string personalIdentityNumber, string carRentalType, int odometerAtStartOfRentalPeriod,
        DateTime? timeStamp = null)
    {
        // This will throw KeyNotFoundException if carRentalType is invalid, no need to validate it separately
        var carRentalPriceEquation = await _carRentalTypesService.GetCarRentalTypePriceEquationAsync(carRentalType);

        var carRentalPeriodStartedModel = new CarRentalPeriodStartedModel
        {
            BookingNumber = _carRentalPeriodValidationService.ValidateBookingNumber(bookingNumber),
            CarRegistrationNumber =
                _carRentalPeriodValidationService.ValidateCarRegistrationNumber(carRegistrationNumber),
            PersonalIdentityNumber =
                _carRentalPeriodValidationService.ValidatePersonalInformationNumber(personalIdentityNumber),
            DateTimeAtStartOfRentalPeriod = _carRentalPeriodValidationService.ValidateTimeStamp(timeStamp),
            OdometerAtStartOfRentalPeriod =
                _carRentalPeriodValidationService.ValidateOdometerAtStartOfRentalPeriod(odometerAtStartOfRentalPeriod),
            CarRentalType = carRentalType,
            CarRentalPriceEquation = carRentalPriceEquation,
            BaseRatePerDayInMinorCurrency = await _carRentalPriceService.GetBaseRatePerDayInMinorCurrencyAsync(),
            BaseRatePerKilometerInMinorCurrency =
                await _carRentalPriceService.GetBaseRatePerKilometerInMinorCurrencyAsync()
        };
        return carRentalPeriodStartedModel;
    }

    public Task<CarRentalPeriodReturnedModel> GetCarRentalPeriodReturnedModelAsync(
        CarRentalPeriodStartedModel carRentalPeriodStartedModel, DateTime dateTimeAtEndOfRentalPeriod,
        int odometerAtReturn)
    {
        var totalRentalPeriodTimeSpan =
            _carRentalPeriodValidationService.ValidateAndCalculateTotalRentalPeriodTimeSpan(
                carRentalPeriodStartedModel.DateTimeAtStartOfRentalPeriod, dateTimeAtEndOfRentalPeriod);

        var totalRentalPeriodDistanceInKilometers =
            _carRentalPeriodValidationService.ValidateAndCalculateTotalRentalPeriodDistanceInKilometers(
                carRentalPeriodStartedModel.OdometerAtStartOfRentalPeriod, odometerAtReturn);

        var totalPriceInMinorCurrency = _carRentalPriceService.CalculateTotalPriceInMinorCurrency(
            carRentalPeriodStartedModel,
            totalRentalPeriodTimeSpan, totalRentalPeriodDistanceInKilometers);

        CarRentalPeriodReturnedModel carRentalPeriodReturnedModel = new()
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
            DateTimeAtStartOfRentalPeriod = carRentalPeriodStartedModel.DateTimeAtStartOfRentalPeriod,
            OdometerAtStartOfRentalPeriod = carRentalPeriodStartedModel.OdometerAtStartOfRentalPeriod,
            BaseRatePerDayInMinorCurrency = carRentalPeriodStartedModel.BaseRatePerDayInMinorCurrency,
            BaseRatePerKilometerInMinorCurrency = carRentalPeriodStartedModel.BaseRatePerKilometerInMinorCurrency
        };

        return Task.FromResult(carRentalPeriodReturnedModel);
    }
}