using DomainLibrary.CarRentalTypes;
using Moq;

namespace Tests.UnitTests;

[TestFixture]
public sealed class CarRentalTypesServiceUnitTests
{
    [SetUp]
    public void SetUp()
    {
        var iCarRentalTypesProviderMock = new Mock<ICarRentalTypesProvider>();
        iCarRentalTypesProviderMock.Setup(mock => mock.GetPriceEquationAsync(ValidArgument))
            .Returns(Task.FromResult(ValidEquation));
        iCarRentalTypesProviderMock.Setup(mock => mock.GetPriceEquationAsync(It.IsNotIn(ValidArgument)))
            .ThrowsAsync(new KeyNotFoundException());
        _sut = new CarRentalTypesService(iCarRentalTypesProviderMock.Object);
    }

    private const string ValidArgument = "valid";
    private const string ValidEquation = "valid equation";
    private ICarRentalTypesService _sut = null!;

    [Test]
    public async Task Returns_equation_on_valid_argument()
    {
        var equation = await _sut.GetCarRentalTypePriceEquationAsync(ValidArgument);
        Assert.That(equation, Is.EqualTo(ValidEquation));
    }

    [Test]
    public void Non_existing_car_rental_type_throws_KeyNotFoundException()
    {
        Assert.That(async () => await _sut.GetCarRentalTypePriceEquationAsync("invalid"),
            Throws.TypeOf<KeyNotFoundException>());
    }
}