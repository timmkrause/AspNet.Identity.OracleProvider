AspNet.Identity.OracleProvider
==============================

ASP.NET Identity provider for Oracle databases.

To Do
=====

- Generic UserStore (?): UserStore&lt;TUser&gt;
    - Extended properties to IdentityUser have to be added manually to the database.

Thanks to
=========

- Raquel Almeida and her ASP.NET Identity provider for MySQL which gave me an excellent jump-start:
    - https://github.com/raquelsa/AspNet.Identity.MySQL
    - http://www.asp.net/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
- Tugberk Ugurlu for completing my test scenarios:
    - https://github.com/tugberkugurlu/AspNet.Identity.RavenDB/tree/master/tests/AspNet.Identity.RavenDB.Tests/Stores
- David Boike for completing my test scenarios:
    - https://github.com/ILMServices/RavenDB.AspNet.Identity/tree/master/RavenDB.AspNet.Identity.Tests