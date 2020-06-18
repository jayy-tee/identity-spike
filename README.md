identity-spike
====
This repo is home to a proof-of-concept .NET Core API and associated projects built around a fictitious and contrived scenario that requires a single authentication provider, backed by two separate user sources.

## Technologies
* .NET Core 3.1 / ASP .NET Core 3.1
* Dapper
* FluentAssertions
* MySQL
* Microsoft SQL Server
* RestSharp
* Serilog


Overview
-------------

The solution is broken down into the following projects:
| Identity|
| ------------- |
| Identity.Api |
| Identity.Api.AcceptanceTests |
| Identity.Application |
| Identity.Cli |
| Identity.Common |
| Identity.Domain |
| Identity.Domain.UnitTests |
| Identity.Infrastrucutre |
| Identity.TestSdk |



 Developer Set-up
 ----------------

Before running, be sure to:
1) Update *appsettings.Local.json* with settings appropriate to your environment.
2) Create and hydrate databases with empty schema


### Database Initialisation ###
Two SQL scripts are provided that can be used to create the schema for the two user sources.


## References ##
| Reference | URL|
| ----------| ---|
| Jason Taylor: Clean Architecture | https://github.com/jasontaylordev/CleanArchitecture |
| greyhamwoohoo: Test-dotnet-core-api-3 | https://github.com/greyhamwoohoo/test-dotnet-core-api-3 |







