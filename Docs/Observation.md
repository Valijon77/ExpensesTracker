# Observations while writing ExpensesTrackker App

***

## Making a change to _Entity_ classes.

Making a change to Entity requires changes in the following parts of an app:
* DTOs
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
