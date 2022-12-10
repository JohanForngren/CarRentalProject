using ActiveLogin.Identity.Swedish.TestData;

namespace Tests;

/// <summary>
///     Valid arguments that are reused across tests.
/// </summary>
internal static class ValidTestCaseData
{
    internal const int BaseRatePerDayInMinorCurrency = 19900;
    internal const int BaseRatePerKilometerInMinorCurrency = 200;
    internal const string BookingNumber = "BookingNumber";
    internal const string CarRegistrationNumber = "ABC123";
    internal const string CarRentalType = "Småbil";
    internal const int OdometerAtStart = 100;
    internal const int OdometerAtReturn = 200;
    internal static readonly string PersonalIdentityNumber = PersonalIdentityNumberTestData.GetRandom().ToString();
    internal static readonly DateTime Now = DateTime.Now;
    internal static readonly DateTime TimeStampAtStart = Now.Subtract(TimeSpan.FromHours(37));
    internal static readonly DateTime TimeStampAtReturn = Now;
    internal static readonly TimeSpan ExpectedTotalTimeSpan = TimeStampAtReturn - TimeStampAtStart;
    internal static int ExpectedTotalRentalPeriodDistanceInKilometers => OdometerAtReturn - OdometerAtStart;
}