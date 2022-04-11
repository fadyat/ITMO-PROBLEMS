## Java Server

Spring Boot

> ### PostgreSQL:
> https://www.postgresql.org/download/
>
>
> ```bash
> psql postgres
> ```
>
>
> ```SQL
> CREATE DATABASE student;
>  
> GRANT ALL PRIVILEGES ON DATABASE student TO artyomfadeyev;
> ```
>
>
> ```
> \l              : all ur db
> \du             : users
> \c student      : connect to student db
> \d              : check relations
> ```

### Request examples:
```http
POST "http://localhost:8080/api/student"
Content-Type: application/json

{
  "name": "itsMe",
  "socials": {
    "tg": "@kekw",
    "vk": "@me_vk",
    "ig": "@notme"
  }
}
```

```http
GET "http://localhost:8080/api/student"
```
```http
GET "http://localhost:8080/api/student?id=1"
```
```http
GET "http://localhost:8080/api/student?id=1,3"
```
```http
GET "http://localhost:8080/api/student/2"
```
```http
GET "http://localhost:8080/api/student?tg=@not_fadyat&ig=@itsfadyat&vk=@mrfadeyev"
```
```http
GET "http://localhost:8080/api/student?tg=@not_fadyat&ig=@itsfadyat&vk=@mrfadeyev&id=1"
```

## Java Server Parser

### ANTLR
https://www.antlr.org/

https://tomassetti.me/getting-started-with-antlr-in-csharp/

### Result example:
```JSON
{
  "MethodName": "getStudent",
  "ReturnType": "List<Student>",
  "Arguments": [
    {
      "ArgumentType": "id",
      "ArgumentName": "Integer"
    }
  ],
  "Url": "/api/student/{id}",
  "HttpMethodName": "GET"
}
```
