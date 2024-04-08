# Observations while writing ExpensesTrackker App

***

## Making a change to _Entity_ classes.

Making a change to Entity requires changes in the following parts of an app:
* DTOs (required more focus and time)
* Controller
* AutoMapperProfile (sometimes)
* Postman tests

## _var_ keyword usage

Writing _var_ everywhere is lowering readability of my code. I think sometimes, for clean code purposes, it is a good practice to explicitly specify the Type of an variable.

## _\[Required\]_ keyword

```csharp
public required string Country { get; set; }
```

When an empty string value for some json property is sent to the server, it may be converted to null for MVC based application but, not getting converted to null for my Web API. When <code>\[Required\]</code> is not applied to above property, it behaves as if <code>\[Required(AllowEmptyStrings=true)\]</code> attribute is applied, thus accepting an empty string. Whereas it is applied to property its __AllowEmptyStrings__ paramater is set to <mark>false</mark> by default.

## Validation in application

My personal opinion is validation should be done outside of the Controllers, which consequently result in more concise controllers.

## OnDelete.Cascade method

Strange functionality I noticed: My relations' design are as following: when entity of principal table is deleted, all its dependents are deleted as well. Simple OnDelete.Cascade policy. But when I delete instances of the principal relation it is not affecting dependent relation instances, leaving instances that should be deleted orphaned. But when the same operation is carried out through EF Core it is working as expected. What can be wrong?

## Why there is a need for checking user fetched from database for null even if using _ClaimsPrincipal_

Gemini answer:
* _ClaimsPrincipal_ represents authentication: It verifies you're dealing with a genuinely authenticated user.
* Database reflects data integrity: The user record might be missing, deleted, or corrupted, even for authenticated users.
* Synchronization issues: Data might not always perfectly align between authentication mechanisms and databases.
* User removal scenarios: A user's authentication details might still exist even after their database record is deleted.
* Authorization: Code might allow or deny actions based on user data not directly stored in ClaimsPrincipal. Attempting to access a missing user's data could lead to errors.
* Feature-specific data: Certain features might rely on user data not captured in authentication claims.
    
## Changes to the database

```csharp
_dataContext.User.Update(user);
```

and

```csharp
_mapper.Map<userUpdateDto, user>();
```

are ensuring query to the database even if the same previous values are provided again. On the other hand, _manual mapping_ is not making queries to the database when the same values are provided again.
