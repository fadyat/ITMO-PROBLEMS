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

- Вывести на экран следующую информацию: название продукта,
общее количество фактов покупки этого продукта,
общее количество покупателей этого продукта

```SQL
WITH product_data as
         (
             SELECT ProductID,
                    COUNT(DISTINCT Detail.SalesOrderID) as orders_cnt,
                    COUNT(DISTINCT CustomerID)          as customers_cnt
             FROM AdventureWorks2017.Sales.SalesOrderHeader as Header
                      JOIN AdventureWorks2017.Sales.SalesOrderDetail as Detail
                           ON Header.SalesOrderID = Detail.SalesOrderID
             GROUP BY ProductID
         )
SELECT *
FROM product_data
```

- Вывести для каждого покупателя информацию о максимальной
и минимальной стоимости одной покупки в чеке, в виде таблицы: номер покупателя,
максимальная сумма, миниммальная сумма

```SQL
WITH customer_orders as
         (
             SELECT CustomerID,
                    Detail.SalesOrderID,
                    MIN(UnitPrice) as min_price,
                    MAX(UnitPrice) as max_price
             FROM AdventureWorks2017.Sales.SalesOrderDetail as Detail
                      JOIN AdventureWorks2017.Sales.SalesOrderHeader as Header
                           ON Detail.SalesOrderID = Header.SalesOrderID
             GROUP BY CustomerID, Detail.SalesOrderID
         )
SELECT CustomerID,
       MIN(customer_orders.min_price) as min_price,
       MAX(customer_orders.max_price) as max_price
FROM customer_orders
GROUP BY CustomerID
```

- Найти номера покупателей, у которых не было ни одной пары чеков с одинаковым количеством наименований товаров

```SQL
WITH customer_order as
         (
             SELECT CustomerID,
                    Detail.SalesOrderID,
                    COUNT(DISTINCT ProductID) as cnt_products
             FROM AdventureWorks2017.Sales.SalesOrderHeader as Header
                      JOIN AdventureWorks2017.Sales.SalesOrderDetail as Detail
                           ON Header.SalesOrderID = Detail.SalesOrderID
             GROUP BY CustomerID, Detail.SalesOrderID
         ),
     customer_products_orders_cnt as
         (
             SELECT CustomerID,
                    cnt_products,
                    COUNT(*) as cnt_orders
             FROM customer_order
             GROUP BY CustomerID, cnt_products
         )
SELECT DISTINCT CustomerID
FROM customer_products_orders_cnt
WHERE CustomerID NOT IN
      (
          SELECT CustomerID
          FROM customer_products_orders_cnt
          WHERE cnt_orders = 2
      )
```

- Найти номера покупателей, у которых все купленые ими товары были куплены как минимум дважды, т.е. на два разных чека

```SQL
WITH customer_product as
         (
             SELECT CustomerID,
                    ProductID,
                    COUNT(Detail.SalesOrderID) as product_cnt
             FROM AdventureWorks2017.Sales.SalesOrderHeader as Header
                      JOIN AdventureWorks2017.Sales.SalesOrderDetail as Detail
                           ON Header.SalesOrderID = Detail.SalesOrderID
             GROUP BY CustomerID, ProductID
         )
SELECT CustomerID
FROM customer_product
WHERE CustomerID NOT IN
      (
          SELECT CustomerID
          FROM customer_product
          WHERE product_cnt = 1
      )
```
