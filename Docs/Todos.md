## TODOS

- Update Controllers to use DTOs (✅)
- Add authentication and authorization features (✅)

***

- Extend or Modify some of the Entities:
    * Add CreatedDateTime, LastModifiedDataTime to both <i>Expenses</i> and <i>User</i> relations (✅)
    * Add more properties to <i>User</i> relation (FirstName, LastName, Country and etc.) (✅)
    * Add Enumeration features to <i>Expenses</i> relation for the properties: _Type_ and _PaymentMethod_ (💭) (❌)
    * ...

- Add the logic to cascade <i>Expenses</i> relation instance on deletion of <i>User</i> instance (✅)
- Update some of the DTOs to support input of Date (✅)
- Add _Identity Service_ for securing database (🕐)
- Add Role-Based Authorization (requires _Identity Service_ functionality) (🕐)
- Add Validation features of framework or change the architecture (consider using MediatR?) (✅)
- Use _Repository_ and _Unit of Work_ patterns for more concise code (🕐)

- Split Users Controller to two individual controllers: Account and Users (✅)
    * Account controller for handling Sign In/Up features
    * Users controller for working with existing users
        - Add Users controller some features to support CRUD operations (✅)
        - I think it is a good idea to transfer user deletion endpoint's feature from users controller to user repository ( )
    * ...

- Add Options Pattern and User Secrets for JWT (✅)
- Get rid of all the nasty yellow warnings (✅)
- Add the __Pagination__ feature (✅)
- Change DTOs to use _Record_ types (💭)
- Should refine DateTime elements to not store milliseconds (even seconds) (💭)
- make the application asynchronous (🕐)
- cookies or token invalidation (cookies - frontend, token inv. - identity service) (💭) (🕐)
- add Error Handling middleware (🕐)
- Seed the database in _Program.cs_ (🕐)
- Add the Sorting and Filtering features (🕐)
- To be continued...
