** PayrollManagement Service **

* About *
This is a payroll management microservices for managing employee payments in an organization.
It comprises of three independent services with a single entry point, Gateway service.
Gateway service manages authentication and request routing to other services. 
The employee service handles all implementations for creating and managing employee information. 
The Salary service manages records of employee payment with all the input for salary computations.
The microservices communicate asynchronously using RabbitMQ message broker.
The services adopt the use of separate in-memory database for storage and processing of their data. However,
gateway service uses microsoft sql database mssql for authentication data persistence.



* Requirements * 
The services run in containers, so it can only run on operating system with docker desktop installed.
The entire solution use Dotnet core 6.

Launching the services:
After cloning the repo to the local system , run docker-compose up
