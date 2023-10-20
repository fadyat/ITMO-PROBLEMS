## Solutions 


- Найти и вывести на экран количество товаров каждого цвета, исключив из поиска товары, цена которых меньше 30.


```SQL
SELECT Color, COUNT(Color) AS Count
FROM Production.Product
WHERE StandardCost >= 30
GROUP BY Color
```

- Найти и вывести на экран список, состоящий из цветов товаров, таких, что минимальная цена товара данного цвета более 100.

```SQL
SELECT Color
FROM Production.Product
GROUP BY Color
HAVING MIN(StandardCost) > 100
```

- Найти и вывести на экран номера подкатегорий товаров и количество товаров в каждой категории.

```SQL
SELECT ProductSubcategoryID, COUNT(ProductID) AS ProductCount
FROM Production.Product
GROUP BY ProductSubcategoryID
```

- Найти и вывести на экран номера товаров и количество фактов продаж данного товара (используется таблица SalesOrderDetail).

```SQL
SELECT ProductID, COUNT(SalesOrderID) AS NumberOfSales
FROM Sales.SalesOrderDetail
GROUP BY ProductID
```

- Найти и вывести на экран номера товаров, которые были куплены более пяти раз.

```SQL
SELECT ProductID
FROM Sales.SalesOrderDetail
GROUP BY ProductID
HAVING COUNT(ProductID) > 5
```

- Найти и вывести на экран номера покупателей, CustomerID, у которых существует более одного чека, SalesOrderID, с одинаковой датой.

```SQL
SELECT CustomerID
FROM Sales.SalesOrderHeader
GROUP BY CustomerID, OrderDate
HAVING COUNT(SalesOrderID) > 1
```

- Найти и вывести на экран все номера чеков, на которые приходится более трех продуктов.

```SQL
SELECT SalesOrderID
FROM Sales.SalesOrderDetail
GROUP BY SalesOrderID
HAVING COUNT(ProductID) > 3
```

- Найти и вывести на экран все номера продуктов, которые были куплены более трех раз.

```SQL
SELECT ProductID
FROM Sales.SalesOrderDetail
GROUP BY ProductID
HAVING COUNT(SalesOrderID) > 3
```

- Найти и вывести на экран все номера продуктов, которые были куплены или три или пять раз.

```SQL
SELECT ProductID
FROM Sales.SalesOrderDetail
GROUP BY ProductID
HAVING COUNT(SalesOrderID) IN (3, 5)
```

- Найти и вывести на экран все номера подкатегорий, в которым относится более десяти товаров.

```SQL
SELECT ProductSubcategoryID
FROM Production.Product
GROUP BY ProductSubcategoryID
HAVING COUNT(ProductID) > 10
```

- Найти и вывести на экран номера товаров, которые всегда покупались в одном экземпляре за одну покупку.

```SQL
SELECT ProductID
FROM Sales.SalesOrderDetail
WHERE OrderQty = 1
```

- Найти и вывести на экран номер чека, SalesOrderID, на который приходится с наибольшим разнообразием товаров купленных на этот чек.

```SQL
SELECT TOP 1 SalesOrderID
FROM Sales.SalesOrderDetail
GROUP BY SalesOrderID
ORDER BY COUNT(ProductID) DESC
```

- Найти и вывести на экран номер чека, SalesOrderID с наибольшей суммой покупки, исходя из того, что цена товара – это UnitPrice, а количество конкретного товара в чеке – это OrderQty.

```SQL
SELECT TOP 1 SalesOrderID
FROM Sales.SalesOrderDetail
GROUP BY SalesOrderID
ORDER BY SUM(LineTotal) DESC
```

- Определить количество товаров в каждой подкатегории, исключая товары, для которых подкатегория не определена, и товары, у которых не определен цвет.

```SQL
SELECT ProductSubcategoryID, COUNT(ProductID) AS CountOfProductInSubcategory
FROM Production.Product
WHERE Color IS NOT NULL
GROUP BY ProductSubcategoryID
HAVING ProductSubcategoryID IS NOT NULL
```

- Получить список цветов товаров в порядке убывания количества товаров данного цвета.

```SQL
SELECT Color
FROM Production.Product
WHERE Color IS NOT NULL
GROUP BY Color
ORDER BY COUNT(ProductID) DESC
```

- Вывести на экран ProductID тех товаров, что всегда покупались в количестве более 1 единицы на один чек, при этом таких покупок было более двух.

```SQL
SELECT ProductID
FROM Sales.SalesOrderDetail
WHERE OrderQty > 1
GROUP BY ProductID
HAVING COUNT(SalesOrderID) > 2
```






