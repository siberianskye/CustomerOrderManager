# CustomerOrderManager

Build & Run

Clone repository in VS.
Launch, should launch into browser with Swagger UI.
Migrations should apply database.


TODO//
Publish to executable.



Assumptions

Guids to be used to avoid incrementation in practice.



Design Desicions

Created Repositories to use DbContext, to avoid any DbContext used within an API layer.
EF Core was chosen for data access, obvious reason being it is preferreed but also because I have toyed around with it within my personal project mentioned in the last call and getting more familiar will be good.

Any Validation that can be done up front without needing any other data is done within the API layer. For example checking if CustomerName is missing or the Guid is empty for a Get, these then return bad requests with an appropriate message.
For validation that requires other data to to accessed such as checking for a duplicate order, this is done in the repository save method. I put it within the save and not a separate validator class for example, because if its within the save anywhere that calls it will also go through the validation rather than having to call validation at each place the save is called. This passes a validation result back to the API layer for it to create the response with the appropriate message.

For error handling I did not have time to explore a global error handler to avoid having to add try/catch anywhere suitable, but for now there is a try catch returning 500 on a caught exception within the two post API methods.




Future Improvements

As mentioned, error handling would be a place for improvement, as well as adding the rest of the CRUD operations mainly delete for the parameter to demonstrate the Update/Create it is doing if the type already exists.
There is also potential for unit tests to check the various results and their corrosponding messages.
