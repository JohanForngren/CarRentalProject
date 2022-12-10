namespace DomainLibrary.Helpers;

public interface IPersonalInformationNumberHelper
{
    /// <returns>true if valid, otherwise false</returns>
    public bool ValidatePersonalInformationNumber(string personalInformationNumber);
}