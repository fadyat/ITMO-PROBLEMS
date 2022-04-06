## Java server

> Spring Boot + PostgreSQL


### DB creation:
```
https://www.postgresql.org/download/
```

```bash
psql postgres
```


```SQL
CREATE DATABASE student;

GRANT ALL PRIVILEGES ON DATABASE student TO artyomfadeyev;
```


```
\l              : all ur db
\du             : users
\c student      : connect to student db
\d              : check relations
```

### Request examples:
```HTTP
###
POST http://localhost:8080/api/student
Content-Type: application/json

{
  "name": "itsMe",
  "socials": {
    "tg": "@kekw",
    "vk": "@me_vk",
    "ig": "@notme"
  }
}

###
GET http://localhost:8080/api/student

###
GET http://localhost:8080/api/student?id=1

###
GET http://localhost:8080/api/student?id=1,3

###
GET http://localhost:8080/api/student/2

###
GET http://localhost:8080/api/student?tg=@not_fadyat&ig=@itsfadyat&vk=@mrfadeyev

###
GET http://localhost:8080/api/student?tg=@not_fadyat&ig=@itsfadyat&vk=@mrfadeyev&id=1
```