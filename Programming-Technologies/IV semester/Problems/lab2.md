# lab-2 Codegen

## Glossary

- HTTP-сервер — приложение, которое умеет обрабатывать обращения посредством HTTP-запросов.
- HTTP-клиент — набор моделей и методов, которые позволяют взаимодействовать с HTTP-сервером.

## Theory

- [roslyn/source-generators.md at main · dotnet/roslyn · GitHub](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md)
- [roslyn/source-generators.cookbook.md at main · dotnet/roslyn · GitHub](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#use-functionality-from-nuget-packages)
- Генерация HTTP-клиентов - [Swagger Documentation](https://swagger.io/docs/open-source-tools/swagger-codegen/)
- Jason Bock, .NET Development Using the Compiler API
- [C# HttpClient - creating HTTP requests with HttpClient in C# (zetcode.com)](https://zetcode.com/csharp/httpclient/)
- [Что такое REST API? | learnapidoc-ru (starkovden.github.io)](https://starkovden.github.io/what-is-rest-api.html)
- Для джавы, гайд по развёртыванию простого приложения: [Getting Started | Building an Application with Spring Boot](https://spring.io/guides/gs/spring-boot/)

Цель: используя инструмент Roslyn API написать программу, которая генерирует HTTP-клиент для сервера написанного на другом языке.

## Задание

### Написание сервиса с которым будет выполняться работ

Написать HTTP-сервер, которые предоставляет несколько методов (в качестве примера, можно взять 2-3 лабораторные второго подпотока). Рекомендуемый язык - Java ввиду простоты поднятия и прочего. Можно использовать любой другой (лучше заранее согласовать). Примеры необходимого функционала:

1. GET, POST запросы
2. Запросы с аргументами в Query, в Body
3. Сложные модели с Response (не примитивы, хотя бы классы с полями)
4. Аргументы, которые являются коллекциями, респонсы, которые коллекции содержат


<a href="https://ibb.co/Zf5RGb1"><img src="https://i.ibb.co/NF5gtGn/request-response.png" alt="request-response"/></a>
### Написание парсера

Написать упрощённый парсер (на C#) для этого сервера, чтобы можно было получить семантическую модель (можно использовать любые библиотеки для этого), а именно:

1. Описание методов из API - url, список аргументов, возвращаемое значение
2. Модели, которые используются в реквестах и респонсах

<a href="https://ibb.co/FYTd2TJ"><img src="https://i.ibb.co/z2vtpvb/parse-generate.png" alt="parse-generate"/></a>

В качестве примера расмотрим процесс парсинга сервиса на языке, который имеет такой синтаксис:

```csharp
@HttpGet("/pet")
public Pet getPet(int id) { ... }
```

Допускаем, что по такому описанию будет сгенерирован эндпоинт, который умеет принимать запросы `GET /pet?id=...` . Из этого описания мы должны получать необходимую для создания клиента информацию:

- Тип запроса - GET, POST, PUT, etc.
- URL запроса (`/pet` в нешем случае)
- Имя метода
- Список аргументов
- Тип возвращаемого значения

На вход парсинг должен уметь принимать путь до исходного кода сервиса, обрабатывать его и возвращать какое-то представление нашего сервиса. Например, список описаний всех найденных методов, которые можно вызывать через HTTP запросы, может выглядеть так:

```csharp
class MethodDeclaration 
{
    string MethodName
    string ReturnType
    List<ArgDeclaration> ArgList
    string Url
    string HttpMethodName
}
```

### Генерация клиента

Используя Roslyn API реализовать генерацию HTTP-клиента для данного сервера. Для API должны генерироваться все нужные модели, методы. Можно посмотреть на Swager codegen, примеры его работы и результат его генерации.

На защите нужно продемонстрировать написанные сервер и генератор, генерацию методов и моделей.

### Детали реализации

В качестве примера рассмотрим как можно было бы реализовать упрощённый вариант задания, а именно создание метода для вызова запроса с помощью StringBuilder. Для описанного выше метода из сервера клиентский код можно написать так:

 

```csharp
public Pet GetPet(int id)
{
    return new HttpClient().Get("/pet?id=" + id);
}
```

Код генерации подобного метода используя введённую структуру (MethodDescriptor) тогда будет таким:

```csharp
foreach (var md in methods)
{
    var sb = new StringBuilder();
    sb.Append("public ");
    sb.Append(md.ReturnType); // Pet
    sb.Append(md.MethodName) // GetPet
		sb.Append("(");
    foreach (var arg in md.ArgList) //int id
    {
        // ...
    }

    // { return new ...
}
```

<aside>
❗ Обратите внимание, что пример выше реализован **без использования Roslyn API**.

В рамках выполнения лабораторной работы необходимо реализовать решение, базирующееся на использовании инструментов Roslyn API (необходимо будет оперировать такими понятиями, как SyntaxFactory, CompilationUnitSyntax, SyntaxTree, SyntaxNode, SyntaxKind, SyntaxTrivia).

</aside>

## Дополнительные задания (не обязательные)

1. Интегрировать генерацию клиента в процесс компиляции по средствам Source Generator.
2. Должна быть реализована возможность инкрементально сгенерировать обновления - не заменять файл, а добавлять в него недостающие методы (не ломаю при этом те методы, которые были добавлены ранее, если они не относятся к API)
3. Реализовать с помощью Roslyn более реалистичный аналог [HackerType](https://hackertyper.net/). Под более реалистичным подразумевается то, что, например, для if statement’а будет сначала генерироваться `if (...) { }`, а потом код внутри.
4. Сделать генерацию клиента из C# сервера используя Roslyn API. Провести сравнение и анализ разницы между подходами.