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
         INNER JOIN AdventureWorks2017.Sales.SalesOrderDetail
                    ON SalesOrderHeader.SalesOrderID = SalesOrderDetail.SalesOrderID
WHERE UnitPrice * OrderQty = (
    SELECT TOP 1 MAX(UnitPrice * OrderQty) AS MAX_SUM
    FROM AdventureWorks2017.Sales.SalesOrderDetail
    GROUP BY SalesOrderID
    ORDER BY MAX_SUM DESC
);
```

- Найти такие продукты, которые покупал только один покупатель.

```SQL


```

- Вывести список продуктов, цена которых выше средней цены товаров в подкатегории, к которой относится товар.

```SQL


```