using ActiveLogin.Identity.Swedish;
using DomainLibrary.Helpers;

namespace CarRentalTypes;

public sealed class PersonalInformationNumberHelper : IPersonalInformationNumberHelper
{
    public bool ValidatePersonalInformationNumber(string personalInformationNumber)
    {
        return PersonalIdentityNumber.TryParse(personalInformationNumber, out _);
    }
}