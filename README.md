# Car rental project

Implementing a small part of the business logic in a fictional car rental company.

DomainLibrary/CarRentalPeriod/CarRentalPeriodFactory.cs is the most top level class, that is probably where you want to
start looking. ICarRentalPeriodFactory is meant to be used in a controller or endpoint.

Tests/IntegrationTests/CarRentalFactoryIntegrationTests.cs does a full integration test of (almost) the entire code
base.

## Design choices

* Testable, SOLID design
* Simple, explicit code, easy digestible for juniors
* Extendable with more car rental types
* Exceptions for error handling, but limit the number of different exceptions that are thrown
* Use domain specific language and code structure
* A simple factory pattern to keep models free from logic
* Command-Query Separation
* Using model classes as a simple Finite State Machine
* Sealed classes for performance
* If the prices are adjusted after the customer has started the rental period, this system still honors the original
  price
* Use minor currency to avoid rounding errors
* Use async methods even when not strictly needed to simulate real world scenario

# Batteries not included

* Data is not cleaned, that is a implementation detail of calling methods.
* Logging is not injected. It could easily be added for instance by using dependency injection or by wrapping the
  ICarRentalPeriodFactory in a logging controller or endpoint.