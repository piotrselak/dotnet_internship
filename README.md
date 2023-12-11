# Contacts recruitment task

## How to run it?

First run command given below in the root directory.
```
docker-compose up -d
```
If you don't want to use docker, you can install PostgreSQL
locally, but remember to change the connection string in appsettings.json in Server project.

Now go to the ./contacts/Server directory. Type:
```
dotnet ef database update --context ContactsContext
dotnet ef database update --context AuthContext
```
Those comments run EF database migrations, which will
create all needed tables and populate them with some data.

Now, still in the Server directory, run:
```
dotnet run
```

Application should be up and running :)

## Tech stack
Application is written in ASP.NET CORE using Entity Framework Core,
Blazored LocalStorage, Npgsql for PostgreSQL EntityFramework support,
ASP.NET Core Identity, JwtBearer, Blazor WebAssembly.

## Short description
The solution consists of three projects: Server, Shared and Client.

Server is a REST API which handles authentication and database connection.
Client is a blazor single page application
and Shared is a set of useful models or data transfer objects 
used on both backend and frontend.

### Server

#### Auth
The name explains everything. It is a module responsible for
user authentication. Controller exposes login and register endpoints,
service deals with logic. Application uses ASP.NET Core Identity for handling auth.

#### Categories
Classes here deal with all the logic with managing
categories. Controller allows the frontend to fetch
the categories for the sake of creating new contacts in form.
There are additional methods not needed for frontend in service,
those are used by the Contact module on the Server.

#### Contacts
Main module of the application. Here all the operations
on contacts happen.
 
#### Database
Here Entity Framework's database contexts are created.
One is used for contacts, the other one is managed by Identity.

### Shared
Like we said before, Shared is a project for Dtos which are used by both Server and Client
The only exceptional thing here is Result class, which we use for returning values from Services
on both Client and Server.
```
public class Result<TData>
{
    public bool Succeeded { get; init; }
    public TData? Data { get; init; }
    public Error? Error { get; init; }
}
```

```
public record Error(int Code, string Description);

```
By this Error record we can model most of the errors in the application. It's not super generic as the Description 
is always string, but it's enough for this application.

```
public class Empty {}
```
Empty is returned when the operation was successful,
but it did not return any data. I wrote it to model
side effect only operations.

## Client

### Domain
Some of the Dtos needed for frontend are declared here.
Nothing interesting.

### Pages
Directory with all the pages of our application.
Most are pretty self explanatory. 
ContactList presents us with list of all contacts in database.
Edit/Create ContactForm serve the purpose of modifying contacts.

### Provider
This is a place where AuthenticatonStateProvider is implemented.
```
public override async Task<AuthenticationState>GetAuthenticationStateAsync()
```
Is the most important method here. It is used to provide an
instance of AuthenticationState which is needed in every component
where we check if user is authorized to do something.
The rest of the methods are some utility ones, so it will be easier
to work on logging in/ logging out or to parse JWT.

### Services
This is where the logic of application is at. Contact and Category
Services are used to simply call the API and unpack JSON to some DTOs.

Auth Service provides us methods to manage authentication.
It connects API with the AuthenticationStateProvider.
Both Register and Login methods log the user in after operation is successful.

### Shared
There are two components declared here - one is a MainLayout used on
all pages and Nav - a navigation bar used in MainLayout.