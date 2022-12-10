// ReSharper disable StringLiteralTypo

namespace Tests.IntegrationTests;

public sealed partial class CarRentalFactoryIntegrationTests
{
    private static object[] _mapsArgumentsAndCalculatesTotalsForReturnedCarReturnCarTestCases =
    {
        new object[]
        {
            "Småbil", (int baseRatePerDayInMinorCurrency, int _) =>
                (int) (baseRatePerDayInMinorCurrency *
                       ValidTestCaseData.ExpectedTotalTimeSpan.TotalDays)
        },
        new object[]
        {
            "Kombi", (int baseRatePerDayInMinorCurrency, int baseRatePerKilometerInMinorCurrency) =>
                (int) (baseRatePerDayInMinorCurrency *
                       ValidTestCaseData.ExpectedTotalTimeSpan.TotalDays * 1.3 +
                       baseRatePerKilometerInMinorCurrency *
                       ValidTestCaseData.ExpectedTotalRentalPeriodDistanceInKilometers)
        },
        new object[]
        {
            "Lastbil", (int baseRatePerDayInMinorCurrency, int baseRatePerKilometerInMinorCurrency) =>
                (int) (baseRatePerDayInMinorCurrency *
                       ValidTestCaseData.ExpectedTotalTimeSpan.TotalDays * 1.5 +
                       baseRatePerKilometerInMinorCurrency *
                       ValidTestCaseData.ExpectedTotalRentalPeriodDistanceInKilometers * 1.5)
        }
    };

    private static object[] _invalidArgumentTestCases =
    {
        new object[]
        {
            null!, ValidTestCaseData.CarRegistrationNumber, ValidTestCaseData.PersonalIdentityNumber,
            ValidTestCaseData.CarRentalType,
            ValidTestCaseData.OdometerAtStart, ValidTestCaseData.Now
        },
        new object[]
        {
            ValidTestCaseData.BookingNumber, null!, ValidTestCaseData.PersonalIdentityNumber,
            ValidTestCaseData.CarRentalType,
            ValidTestCaseData.OdometerAtStart, ValidTestCaseData.Now
        },
        new object[]
        {
            ValidTestCaseData.BookingNumber, ValidTestCaseData.CarRegistrationNumber, null!,
            ValidTestCaseData.CarRentalType,
            ValidTestCaseData.OdometerAtStart, ValidTestCaseData.Now
        },
        new object[]
        {
            ValidTestCaseData.BookingNumber, ValidTestCaseData.CarRegistrationNumber, "123456-7890",
            ValidTestCaseData.CarRentalType,
            ValidTestCaseData.OdometerAtStart, ValidTestCaseData.Now
        },
        new object[]
        {
            ValidTestCaseData.BookingNumber, ValidTestCaseData.CarRegistrationNumber,
            ValidTestCaseData.PersonalIdentityNumber,
            ValidTestCaseData.CarRentalType, -1, ValidTestCaseData.Now
        },
        new object[]
        {
            ValidTestCaseData.BookingNumber, ValidTestCaseData.CarRegistrationNumber,
            ValidTestCaseData.PersonalIdentityNumber,
            ValidTestCaseData.CarRentalType, -1, ValidTestCaseData.Now.AddYears(1000)
        }
    };
}