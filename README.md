AspNet.Identity.OracleProvider
==============================

ASP.NET Identity provider for Oracle databases.

Status
======

At the moment the state of the project is "work in progress" and I really like to get some feedback.

To Do
=====

- Generic UserStore (?): UserStore&lt;TUser&gt;
    - Extended properties to IdentityUser have to be added manually to the database.
- OracleDataContext should be internal, just an OracleConnection or ConnectionString should be provided.