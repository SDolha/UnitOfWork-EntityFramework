# UnitOfWork-EntityFramework
Unit of work and repository pattern interfaces, Entity Framework unit of work and generic repository implementations, with sample application code including comments describing each part of the solution.

The technical approach includes separating data access pattern contracts from implementation, and then the generic implementations of the patterns over Entity Framework for any DbContext from actual sample database model, and is described with more details in this [blog post](https://codesections.wordpress.com/2016/03/15/unit-of-work-over-entity-framework/).

![](https://codesections.files.wordpress.com/2016/03/data-access-diagram.png)

## NuGet packages
* [SDolha.DataAccessPatterns.Contracts](https://www.nuget.org/packages/SDolha.DataAccessPatterns.Contracts)
* [SDolha.DataAccessPatterns.EntityFramework](https://www.nuget.org/packages/SDolha.DataAccessPatterns.EntityFramework)
