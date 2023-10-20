- Найти долю продаж каждого продукта (цена продукта * количество продукта), на каждый чек, в денежном выражении


```SQL
SELECT SalesOrderID,
       Name,
       OrderQty * UnitPrice / SUM(OrderQty * Detail.UnitPrice)
                                  OVER (PARTITION BY SalesOrderID) as share
FROM AdventureWorks2017.Sales.SalesOrderDetail as Detail
         JOIN AdventureWorks2017.Production.Product
              ON Product.ProductID = Detail.ProductID
ORDER BY SalesOrderID
```

- Вывести на экран список продуктов, их стоимость, а также разницу между
стоимостью этого продукта и стоимостью самого дешевого продукта в той же подкатегории,
к которой относится продукт

```SQL
SELECT Name,
       StandardCost,
       StandardCost - MIN(StandardCost)
                          OVER (PARTITION BY ProductSubcategoryID) as difference
FROM AdventureWorks2017.Production.Product
WHERE ProductSubcategoryID IS NOT NULL
```

- Вывести три колонки: номер покупателя, номер чека покупателя
(отсортированный по возрастанию даты чека) и искуственно введенный
порядковый номер текущего чека, начиная с 1, для каждого покупателя

```SQL
SELECT CustomerID,
       SalesOrderID,
       ROW_NUMBER() OVER (
           PARTITION BY CustomerID
           ORDER BY OrderDate
           ) as order_number
FROM AdventureWorks2017.Sales.SalesOrderHeader
```


- Вывести номера продуктов, таких что их цена выше средней цены продукта в подкатегории,
к которой относится продукт. Запрос реализовать двумя способами.
В одном решении допускается использование ОТВ

```SQL
WITH products as
         (
             SELECT ProductID,
                    StandardCost,
                    ProductSubcategoryID,
                    AVG(StandardCost) OVER ( PARTITION BY ProductSubcategoryID) as subcat_avg_price
             FROM AdventureWorks2017.Production.Product
         )
SELECT ProductID
FROM products
WHERE ProductSubcategoryID IS NOT NULL
  AND StandardCost > subcat_avg_price
```
