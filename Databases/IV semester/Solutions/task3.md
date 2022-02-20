## Solutions


- Найти и вывести на экран название продуктов и название категорий товаров, к которым относится этот продукт, с учетом того, что в выборку попадут только товары с цветом Red и ценой не менее 100.

```SQL
SELECT Product.Name  AS ProductName,
       Category.Name AS CategoryName
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS Subcategory
                    ON Product.ProductSubcategoryID = Subcategory.ProductSubcategoryID
         INNER JOIN AdventureWorks2017.Production.ProductCategory AS Category
                    ON Category.ProductCategoryID = Subcategory.ProductCategoryID
WHERE Product.Color = 'Red' AND Product.StandardCost >= 100;
```

- Вывести на экран названия подкатегорий с совпадающими именами.

```SQL
SELECT ps1.Name
FROM AdventureWorks2017.Production.ProductSubcategory AS ps1
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS ps2
                    ON ps1.ProductSubcategoryID != ps2.ProductSubcategoryID AND ps1.Name = ps2.Name
```

- Вывести на экран название категорий и количество товаров в данной категории.

```SQL
SELECT Category.Name,
       COUNT(Product.ProductID) AS ProductsInCategory
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS Subcategory
                    ON Product.ProductSubcategoryID = Subcategory.ProductSubcategoryID
         INNER JOIN AdventureWorks2017.Production.ProductCategory AS Category
                    ON Subcategory.ProductCategoryID = Category.ProductCategoryID
GROUP BY Category.Name
```

- Вывести на экран название подкатегории, а также количество товаров в данной подкатегории с учетом ситуации, что могут существовать подкатегории с одинаковыми именами.

```SQL
SELECT SubCategory.Name,
       COUNT(Product.ProductSubcategoryID) AS ProductsInSubcategory
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS SubCategory
                    ON Product.ProductSubcategoryID = SubCategory.ProductSubcategoryID
GROUP BY SubCategory.Name, SubCategory.ProductCategoryID
```

- Вывести на экран название первых трех подкатегорий с небольшим количеством товаров.

```SQL
SELECT TOP 3 SubCategory.Name
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS SubCategory
                    ON Product.ProductSubcategoryID = SubCategory.ProductSubcategoryID
GROUP BY SubCategory.Name
ORDER BY COUNT(Product.ProductID)
```

- Вывести на экран название подкатегории и максимальную цену продукта с цветом Red в этой подкатегории.

```SQL
SELECT TOP 1 SubCategory.Name,
             MAX(Product.StandardCost) AS Cost
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS SubCategory
                    ON Product.ProductSubcategoryID = SubCategory.ProductSubcategoryID
WHERE Product.Color = 'Red'
GROUP BY SubCategory.Name
ORDER BY Cost DESC;
```

- Вывести на экран название поставщика и количество товаров, которые он поставляет.

```SQL
SELECT Vendor.Name,
       COUNT(ProductID) AS UniqueProductOrdering
FROM AdventureWorks2017.Purchasing.Vendor AS Vendor
         INNER JOIN AdventureWorks2017.Purchasing.ProductVendor AS ProductVendor
                    ON Vendor.BusinessEntityID = ProductVendor.BusinessEntityID
GROUP BY Vendor.Name
```

- Вывести на экран название товаров, которые поставляются более чем одним поставщиком.


```SQL
SELECT Product.Name
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Purchasing.ProductVendor AS ProductVendor
                    ON Product.ProductID = ProductVendor.ProductID
GROUP BY Product.Name
HAVING COUNT(ProductVendor.BusinessEntityID) > 1
```

- Вывести на экран название самого продаваемого товара.


```SQL
SELECT TOP 1 Product.Name
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
                    ON Product.ProductID = SalesOrderDetail.ProductID
GROUP BY Product.Name
ORDER BY SUM(SalesOrderDetail.OrderQty) DESC;
```

- Вывести на экран название категории, товары из которой продаются наиболее активно.

```SQL
SELECT TOP 1 Category.Name
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Sales.SalesOrderDetail AS SalesOrderDetail
                    ON Product.ProductID = SalesOrderDetail.ProductID
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS Subcategory
                    ON Product.ProductSubcategoryID = Subcategory.ProductSubcategoryID
         INNER JOIN AdventureWorks2017.Production.ProductCategory AS Category
                    ON Subcategory.ProductCategoryID = Category.ProductCategoryID
GROUP BY Category.Name
ORDER BY SUM(SalesOrderDetail.OrderQty) DESC;
```

- Вывести на экран названия категорий, количество подкатегорий и количество товаров в них.

```SQL
SELECT ProductCategory.Name,
       COUNT(DISTINCT ProductSubcategory.ProductSubcategoryID) AS SubcategoriesCount,
       COUNT(Product.ProductID) AS ProductsCount
FROM AdventureWorks2017.Production.Product AS Product
         INNER JOIN AdventureWorks2017.Production.ProductSubcategory AS ProductSubcategory
                    ON Product.ProductSubcategoryID = ProductSubcategory.ProductSubcategoryID
         INNER JOIN AdventureWorks2017.Production.ProductCategory AS ProductCategory
                    ON ProductSubcategory.ProductCategoryID = ProductCategory.ProductCategoryID
GROUP BY ProductCategory.Name
```

- Вывести на экран номер кредитного рейтинга и количество товаров, поставляемых компаниями, имеющими этот кредитный рейтинг.

``` SQL
SELECT Vendor.CreditRating,
       SUM(PurchaseOrderDetail.OrderQty) AS ProductsCount
FROM AdventureWorks2017.Purchasing.Vendor AS Vendor
         INNER JOIN AdventureWorks2017.Purchasing.PurchaseOrderHeader AS PurchaseOrderHeader
                    ON Vendor.BusinessEntityID = PurchaseOrderHeader.VendorID
         INNER JOIN AdventureWorks2017.Purchasing.PurchaseOrderDetail AS PurchaseOrderDetail
                    ON PurchaseOrderHeader.PurchaseOrderID = PurchaseOrderDetail.PurchaseOrderID
GROUP BY Vendor.CreditRating
ORDER BY Vendor.CreditRating
```