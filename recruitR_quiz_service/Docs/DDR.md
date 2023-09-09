using base64 encoding for quizaccess tokens instead of cryptographically secure random strings
reason: base64 encoding is computationally cheaper than generating cryptographically secure random strings, and still provides a reasonable level of security

using vertical slices instead of technical layers
reason: vertical slices are easier to understand and maintain than technical layers and new developers can easily understand the codebase and make changes
it also makes it easier to add new features and change existing ones

using dto pattern instead of directly using the database models
reason: dto pattern is used to transfer data between layers and it makes it easier to change the data structure without affecting other layers
such as changing the database structure without affecting the business logic layer

InsertMany for Individual Canddates instead of nesting:
candidates has quizinstanceid as ref isntead of being nested inside the quizinstanceDTO
reason:
Efficiency: InsertMany can be more efficient when you need to insert a large number of individual documents (candidates in your case).
Flexibility: Each candidate document is independent, which means you can update or delete individual candidates without affecting others.
Query Flexibility: You can query and manipulate individual candidates with more granularity.
Storing Candidates as an Array in Parent Document:

Storing candidates within a parent document can be more efficient when you always access candidates together with their parent entity
but this isn't the case (see retreivequiztoattend usecase)

syntactic validation takes place in upstream process so controllers only does business rule validations, using annotations
authN and Z takes place in upstream process via jwt (rethink because we could spare one db call in retrieveQuizToAttend if used nesting)

using asplogger as fallbacklogger if rabbitmq can't be reached

