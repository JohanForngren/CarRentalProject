using ActiveLogin.Identity.Swedish;
using DomainLibrary.Helpers;

namespace ExampleImplementations;

public sealed class PersonalInformationNumberHelper : IPersonalInformationNumberHelper
{
    public bool ValidatePersonalInformationNumber(string personalInformationNumber)
    {
        return PersonalIdentityNumber.TryParse(personalInformationNumber, out _);
    }
}