## Solutions

- Найти название самого продаваемого продукта.

```SQL
SELECT Product.Name
FROM AdventureWorks2017.Production.Product
WHERE Product.ProductID = (
    SELECT TOP 1 SalesOrderDetail.ProductID
    FROM AdventureWorks2017.Sales.SalesOrderDetail
    GROUP BY SalesOrderDetail.ProductID
    ORDER BY COUNT(*) DESC
)
```

- Найти покупателя, совершившего покупку на самую большую сумму, считая сумму покупки исходя из цены товара без скидки (UnitPrice).


```SQL
SELECT CustomerID
FROM AdventureWorks2017.Sales.SalesOrderHeader
         JOIN AdventureWorks2017.Sales.SalesOrderDetail
              ON SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
WHERE UnitPrice * OrderQty = (
    SELECT MAX(UnitPrice * OrderQty)
    FROM AdventureWorks2017.Sales.SalesOrderDetail
)
```

- Найти такие продукты, которые покупал только один покупатель.

> idk how to do it w/o joins 😞
```SQL
SELECT Product.Name
FROM AdventureWorks2017.Production.Product
WHERE ProductID IN (
    SELECT ProductID
    FROM AdventureWorks2017.Sales.SalesOrderDetail
             JOIN AdventureWorks2017.Sales.SalesOrderHeader
                  ON SalesOrderDetail.SalesOrderID = SalesOrderHeader.SalesOrderID
    GROUP BY ProductID
    HAVING COUNT(DISTINCT CustomerID) = 1
)
```

- Вывести список продуктов, цена которых выше средней цены товаров в подкатегории, к которой относится товар.

```SQL
SELECT Product.Name
FROM AdventureWorks2017.Production.Product AS Product
WHERE Product.StandardCost > (
    SELECT AVG(ProductCopy.StandardCost)
    FROM AdventureWorks2017.Production.Product AS ProductCopy
    WHERE Product.ProductSubcategoryID = ProductCopy.ProductSubcategoryID
)
```

- Найти такие товары, которые были куплены более чем одним покупателем, при этом все покупатели этих товаров покупали товары только одного цвета и товары не входят в список покупок покупателей, купивших товары только двух цветов.

```SQL
SELECT ProductID
FROM AdventureWorks2017.Sales.SalesOrderDetail
         JOIN AdventureWorks2017.Sales.SalesOrderHeader
              ON SalesOrderDetail.SalesOrderID = SalesOrderHeader.SalesOrderID
WHERE CustomerID IN
      (
          SELECT CustomerID
          FROM AdventureWorks2017.Sales.SalesOrderDetail
                   JOIN AdventureWorks2017.Sales.SalesOrderHeader
                        ON SalesOrderDetail.SalesOrderID = SalesOrderHeader.SalesOrderID
                   JOIN AdventureWorks2017.Production.Product
                        ON Product.ProductID = SalesOrderDetail.ProductID
          GROUP BY CustomerID
          HAVING COUNT(DISTINCT Color) = 1
      )
  AND ProductID NOT IN
      (
          SELECT ProductID
          FROM AdventureWorks2017.Sales.SalesOrderDetail
                   JOIN AdventureWorks2017.Sales.SalesOrderHeader
                        ON SalesOrderDetail.SalesOrderID = SalesOrderHeader.SalesOrderID
          WHERE CustomerID IN
                (
                    SELECT CustomerID
                    FROM AdventureWorks2017.Sales.SalesOrderDetail
                             JOIN AdventureWorks2017.Sales.SalesOrderHeader
                                  ON SalesOrderDetail.SalesOrderID = SalesOrderHeader.SalesOrderID
                             JOIN AdventureWorks2017.Production.Product
                                  ON Product.ProductID = SalesOrderDetail.ProductID
                    GROUP BY CustomerID
                    HAVING COUNT(DISTINCT Color) = 2
                )
      )
GROUP BY ProductID
HAVING COUNT(DISTINCT CustomerID) > 1
```

- Найти такие товары, которые были куплены такими покупателями, у которых они присутствовали в каждой их покупке.

```SQL

```
- Найти все товары, такие что их покупали всегда с товаром, цена которого максимальна в своей категории.
```SQL

```
- Найти номера тех покупателей, у которых есть как минимум два чека, и каждый из этих чеков содержит как минимум три товара, каждый из которых как минимум был куплен другими покупателями три раза.
```SQL

```
- Найти все чеки, в которых каждый товар был куплен дважды этим же покупателем.
```SQL

```
- Найти товары, которые были куплены минимум три раза различными покупателями.
```SQL

```
- Найти такую подкатегорию или подкатегории товаров, которые содержат более трех товаров, купленных более трех раз.
```SQL

```
- Найти те товары, которые не были куплены более трех раз, и как минимум дважды одним и тем же покупателем.
```SQL

```
