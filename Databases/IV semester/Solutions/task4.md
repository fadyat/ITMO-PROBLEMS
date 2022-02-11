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
             SELECT SUM(SalesOrderDetail.OrderQty) as amount
             FROM AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
             GROUP BY SalesOrderDetail.ProductID
         ) AS t
)
```

- Найти покупателя, совершившего покупку на самую большую сумму, считая сумму покупки исходя из цены товара без скидки (UnitPrice).


```SQL

```