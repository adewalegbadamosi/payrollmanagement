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
visual studio


* Launching the service  *
2 options:
First - Run locally without countainer:
- Clone to local system
- Restore dependency (if not automatic)
- docker run -it --rm --name payrol_rabbitmq -p 5673:5672 -p 15673:15672 rabbitmq:3.7-management
- docker run -d --name payrol_mssql -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password10$' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
- Create database named "Finance" (Access mssql in running docker container via shell or access via azure studio)
- Build and run => http://localhost:63487/swagger/index.html

Second - Run locally with countainer:
- Clone to local system
- Restore dependency (if not automatic)
- run docker-compose up  => http://localhost:63487/swagger/index.html
- Create database named "Finance" (Access mssql in running docker container via shell or access via azure studio)

