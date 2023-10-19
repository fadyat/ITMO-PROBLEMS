## Solutions
- Найти и вывести на экран названия продуктов, их цвет и размер.

```SQL
SELECT Name, Color, Size
FROM Production.Product
```

- Найти и вывести на экран названия, цвет и размер таких продуктов, у которых цена более 100.

```SQL
SELECT Name, Color, Size
FROM Production.Product
WHERE StandardCost > 100
```

- Найти и вывести на экран название, цвет и размер таких продуктов, у которых цена менее 100 и цвет Black.


```SQL
SELECT Name, Color, Size
From Production.Product
WHERE StandardCost < 100
 AND Color = 'Black'
```

- Найти и вывести на экран название, цвет и размер таких продуктов, у которых цена менее 100 и цвет Black, упорядочив вывод по возрастанию стоимости продуктов.

```SQL
SELECT Name, Color, Size
From Production.Product
WHERE StandardCost < 100
  AND Color = 'Black'
ORDER BY StandardCost
```

- Найти и вывести на экран название и размер первых трех самых дорогих товаров с цветом Black.

```SQL
SELECT TOP 3 WITH TIES Name, Size
From Production.Product
WHERE Color = 'Black'
ORDER BY StandardCost DESC
```

- Найти и вывести на экран название и цвет таких продуктов, для которых определен и цвет, и размер.

```SQL
SELECT Name, Color
FROM Production.Product
WHERE Color IS NOT NULL
  AND Size IS NOT NULL
```

- Найти и вывести на экран неповторяющиеся цвета продуктов, у которых цена находится в диапазоне от 10 до 50 включительно.

```SQL
SELECT DISTINCT Color
FROM Production.Product
WHERE StandardCost BETWEEN 10 AND 50
```

-  Найти и вывести на экран все цвета таких продуктов, у которых в имени первая буква ‘L’ и третья ‘N’.

```SQL
SELECT Color
FROM Production.Product
WHERE Name LIKE 'L_N%'
```

- Найти и вывести на экран названия таких продуктов, которые начинаются либо на букву ‘D’, либо на букву ‘M’, и при этом длина имени – более трех символов.


```SQL
SELECT Name
FROM Production.Product
WHERE Name LIKE '[DM]%'
  AND LEN(Name) > 3
```

- Вывести на экран названия продуктов, у которых дата начала продаж – не позднее 2012 года.

```SQL
SELECT Name
FROM Production.Product
WHERE YEAR(SellStartDate) <= 2012
```

- Найти и вывести на экран названия всех подкатегорий товаров.

```SQL
SELECT DISTINCT Name
FROM Production.ProductSubcategory
```

- Найти и вывести на экран названия всех категорий товаров.

```SQL
SELECT DISTINCT Name
FROM Production.ProductCategory
```

- Найти и вывести на экран имена всех клиентов из таблицы Person, у которых обращение (Title) указано как «Mr.».


```SQL
SELECT FirstName
FROM Person.Person
WHERE Title LIKE 'Mr.'
```

-  Найти и вывести на экран имена всех клиентов из таблицы Person, для которых не определено обращение (Title).


```SQL
SELECT FirstName
FROM Person.Person
WHERE Title IS NULL
```
