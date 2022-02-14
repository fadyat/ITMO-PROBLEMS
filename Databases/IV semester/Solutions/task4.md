## Solutions

- Найти название самого продаваемого продукта.

```SQL
SELECT Product.Name
FROM AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
         INNER JOIN AdventureWorks2017.Production.Product AS Product
                    ON Product.ProductID = SalesOrderDetail.ProductID
GROUP BY Product.Name, Product.ProductID
HAVING SUM(SalesOrderDetail.OrderQty) = (
    SELECT MAX(t.amount)
    FROM (
             SELECT SUM(SalesOrderDetail.OrderQty) AS amount
             FROM AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
             GROUP BY SalesOrderDetail.ProductID
         ) AS t
)
```

- Найти покупателя, совершившего покупку на самую большую сумму, считая сумму покупки исходя из цены товара без скидки (UnitPrice).


```SQL
SELECT CustomerID
FROM AdventureWorks2017.Sales.Customer
WHERE CustomerID IN (
    SELECT CustomerID
    FROM AdventureWorks2017.Sales.SalesOrderHeader AS SalesOrderHeader
             INNER JOIN AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
                        ON SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
    GROUP BY CustomerID
    HAVING SUM(OrderQty * UnitPrice) = (
        SELECT MAX(t.sum)
        FROM (
                 SELECT SUM(OrderQty * UnitPrice) AS sum
                 FROM AdventureWorks2017.Sales.SalesOrderHeader AS SalesOrderHeader
                          INNER JOIN AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
                                     ON SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
                 GROUP BY CustomerID
             ) AS t
    )
)
```
> Extra solution using joins:

```SQL
SELECT TOP 1 Customer.CustomerID,
       SUM(SalesOrderDetail.UnitPrice * SalesOrderDetail.OrderQty) AS Sum
FROM AdventureWorks2017.Sales.Customer AS Customer
         INNER JOIN AdventureWorks2017.Sales.SalesOrderHeader AS SalesOrderHeader
                    ON Customer.CustomerID = SalesOrderHeader.CustomerID
         INNER JOIN AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
                    ON SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
GROUP BY Customer.CustomerID
ORDER BY Sum DESC
```

- Найти такие продукты, которые покупал только один покупатель.

```SQL


```

> Extra solution using joins:

```SQL
SELECT ProductID
FROM AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
         INNER JOIN AdventureWorks2017.Sales.SalesOrderHeader AS SalesOrderHeader
                    ON SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
GROUP BY ProductID
HAVING COUNT(DISTINCT SalesOrderHeader.CustomerID) = 1
```

- Вывести список продуктов, цена которых выше средней цены товаров в подкатегории, к которой относится товар.

```SQL


```