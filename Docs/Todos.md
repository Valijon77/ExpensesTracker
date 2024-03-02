## TODOS

- Update Controllers to use DTOs (✅)
- Add authentication and authorization features (✅)

***

- Extend or Modify some of the Entities:
    * Add CreatedDateTime, LastModifiedDataTime to both <i>Expenses</i> and <i>User</i> relations (✅)
    * Add more properties to <i>User</i> relation (FirstName, LastName, Country and etc.) (✅)
    * Add Enumeration features to <i>Expenses</i> relation for the properties: Type and PaymentMethod (optional) (❌)
    * ...

- Add the logic to cascade <i>Expenses</i> relation instance on deletion of <i>User</i> instance (✅)
- Update some of the DTOs to support input of Date (✅)
- Add Identity Service for securing database (🕰️)
- Add Validation features of framework or change the architecture (consider using MediatR?) (✅)
- Use Repository and Unit of Work patterns for more concise code (⏰)

- Split Users Controller to two individual controllers: Account and Users (⏱️)
    * Account controller for handling Sign In/Up features
    * Users controller for working with existing users
        - Add Users controller some features to support CRUD operations
    * ...

- Add Options Pattern and User Secrets for JWT (✅)
- Get rid of all the nasty yellow warnings (✅)
- Add the __Pagination__ feature (⌚️)
- Change DTOs to use _Record_ types (optional)
- Should refine DateTime elements to not store milliseconds (even seconds) (optional)
- make the application asynchronous (🕐)
- To be continued...
