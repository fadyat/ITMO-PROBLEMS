- Найти среднее количество покупок на чек для каждого покупателя

```SQL
WITH customer_orders as
         (
             SELECT CustomerID, Detail.SalesOrderID, COUNT(ProductID) as product_cnt
             FROM AdventureWorks2017.Sales.SalesOrderDetail as Detail
                      JOIN AdventureWorks2017.Sales.SalesOrderHeader as Header
                           ON Detail.SalesOrderID = Header.SalesOrderID
             GROUP BY CustomerID, Detail.SalesOrderID
         )
SELECT CustomerID, AVG(product_cnt) as avg_products_per_order
FROM customer_orders
GROUP BY CustomerID
ORDER BY CustomerID
```

- Найти для каждого продукта и каждого покупателя соотношение
количества фактов покупки данного товара данным покупателем к общему количеству
фактов покупки товаров данным покупателем

```SQL
WITH customer_product as
         (
             SELECT CustomerID, ProductID, COUNT(*) as product_cnt
             FROM AdventureWorks2017.Sales.SalesOrderHeader as Header
                      JOIN AdventureWorks2017.Sales.SalesOrderDetail as Detail
                           ON Header.SalesOrderID = Detail.SalesOrderID
             GROUP BY CustomerID, ProductID
         ),
     customer_products as
         (
             SELECT CustomerID, COUNT(DISTINCT ProductID) as total_products
             FROM AdventureWorks2017.Sales.SalesOrderHeader as Header
                      JOIN AdventureWorks2017.Sales.SalesOrderDetail as Detail
                           ON Header.SalesOrderID = Detail.SalesOrderID
             GROUP BY CustomerID
         )
SELECT customer_product.CustomerID,
       ProductID,
       CAST(customer_product.product_cnt as DECIMAL(7, 2)) /
       CAST(customer_products.total_products as DECIMAL(7, 2)) as ratio
FROM customer_product
         JOIN customer_products
              ON customer_product.CustomerID = customer_products.CustomerID
```
