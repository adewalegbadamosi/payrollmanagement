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
Docker desktop
Dotnet core 6
RabbitMQ
Sql server
Visual studio


* Launching the service  *
2 options:
*First* - Run locally in Visual studio:
- Clone repo to local 
- Restore dependency (if not automatic)
- Start RabbitMQ 
- Start sql server, create database, Finance (update connection string)
- Build and run
- Access swagger at http://localhost:63487/swagger/index.html


*Second* - Run locally in docker:
- Clone repo to local
- run docker-compose up -d
- Access swagger at http://localhost:63487/swagger/index.html


