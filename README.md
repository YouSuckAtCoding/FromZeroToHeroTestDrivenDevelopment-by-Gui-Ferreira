# FromZeroToHeroTestDrivenDevelopment

Code from the course From Zero to Hero Test Driven Development, made by Gui Ferreira in the plataform Dometrain. The course teaches the basic of testing and TDD and creates a project applying the basics and advanced concepts of TDD, like differents styles (Detroit and London schools), best practices for team development with TDD and using TDD with legacy Code.
The course teaches about the TDD cycle, when to use the test first vs code first approaches, diferent types of Test Doubles (Stubs, Fakes, Mocks, etc...) and testing with unstable dependencies.

The project represents a parking lot charging system, where the app registers its daily price and the prices per hour and calculates what a driver will pay for using the parking lot.

The code is manly C#, and uses XUnit as the test framework. The project also uses packages like Fluent Assertions, NSubstitute and Microsoft.Asp.Net.Mvc.Testing.
The project makes use of a Postgres database while using Docker with the Test Containers package.
