Implementing a small part of the business logic in a fictional car rental company.

# Design choices

* SOLID design
* Simple, explicit code, easy digestible for juniors
* Extendable with more car rental types
* Exceptions for error handling, but limit the number of different exceptions that are thrown
* Use domain specific language and code structure
* Factory i service
* Sealed classes for performance
* If the prices are adjusted after the customer has started the rental period, this system still honors the original price
* Use minor currency to avoid rounding errors
* Use async methods even when not strictly needed to simulate real world scenario
