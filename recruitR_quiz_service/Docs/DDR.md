using base64 encoding for quizaccess tokens instead of cryptographically secure random strings
reason: base64 encoding is computationally cheaper than generating cryptographically secure random strings, and still provides a reasonable level of security

using vertical slices instead of technical layers
reason: vertical slices are easier to understand and maintain than technical layers and new developers can easily understand the codebase and make changes
it also makes it easier to add new features and change existing ones

using dto pattern instead of directly using the database models
reason: dto pattern is used to transfer data between layers and it makes it easier to change the data structure without affecting other layers
such as changing the database structure without affecting the business logic layer


