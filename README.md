AspNet.Identity.OracleProvider
==============================

ASP.NET Identity provider for Oracle databases.

A pre-release version is available on NuGet: https://www.nuget.org/packages/AspNet.Identity.OracleProvider

To Do
=====

- Generic UserStore (?): UserStore&lt;TUser&gt;
    - Extended properties to IdentityUser have to be added manually to the database.

Thanks to
=========

- [Raquel Almeida](https://github.com/raquelsa) and her ASP.NET Identity provider for MySQL which gave me an excellent jump-start:
    - https://github.com/raquelsa/AspNet.Identity.MySQL
    - http://www.asp.net/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
- [Tugberk Ugurlu](https://github.com/tugberkugurlu) for completing my test scenarios:
    - https://github.com/tugberkugurlu/AspNet.Identity.RavenDB/tree/master/tests/AspNet.Identity.RavenDB.Tests/Stores
- [ILM Services](https://github.com/ILMServices) for completing my test scenarios:
    - https://github.com/ILMServices/RavenDB.AspNet.Identity/tree/master/RavenDB.AspNet.Identity.Tests