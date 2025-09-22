-- SETUP:
    -- Create a database server (docker)
        -- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Pikapika2!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
    -- Connect to the server (Azure Data Studio / Database extension)
    -- Test your connection with a simple query (like a select)
    -- Execute the Chinook database (to create Chinook resources in your db)

    

-- On the Chinook DB, practice writing queries with the following exercises

-- BASIC CHALLENGES
-- List all customers (full name, customer id, and country) who are not in the USA
SELECT * FROM Customer;
-- List all customers from Brazil
SELECT * FROM Customer WHERE Customer.Country = 'Brazil';
-- List all sales agents
SELECT * FROM Employee WHERE Employee.Title LIKE '%Sales% %Agent%';
-- Retrieve a list of all countries in billing addresses on invoices
SELECT BillingAddress FROM "Invoice";
-- Retrieve how many invoices there were in 2009, and what was the sales total for that year?
SELECT * FROM "Invoice" WHERE YEAR("Invoice"."InvoiceDate") = '2009';
    -- (challenge: find the invoice count sales total for every year using one query)
SELECT SUM("Total"),YEAR("Invoice"."InvoiceDate") FROM "Invoice" GROUP BY YEAR("Invoice"."InvoiceDate");
-- how many line items were there for invoice #37
SELECT COUNT(*) FROM "Invoice" WHERE Invoice.InvoiceId = 37;
-- how many invoices per country? BillingCountry  # of invoices -
SELECT "BillingCountry",COUNT(*) FROM "Invoice" Group BY "Invoice"."BillingCountry";
-- Retrieve the total sales per country, ordered by the highest total sales first.
SELECT "BillingCountry",Sum("Total") FROM "Invoice" Group BY "Invoice"."BillingCountry";

-- JOINS CHALLENGES
-- Every Album by Artist
SELECT * FROM "Album" LEFT OUTER JOIN "Artist" ON "Album"."ArtistId" = "Artist"."ArtistId" ORDER BY "Album"."ArtistId";
-- All songs of the rock genre
SELECT Track."Name" FROM "Track" LEFT OUTER JOIN "Genre" ON "Track"."GenreId" = "Genre"."GenreId" WHERE "Genre"."Name" = 'Rock';
-- Show all invoices of customers from brazil (mailing address not billing)
SELECT * FROM "invoice" LEFT OUTER JOIN "Customer" ON "invoice"."CustomerId" = "Customer"."CustomerId" WHERE "Customer"."Country" = 'Brazil' ORDER BY "Customer"."Address";
-- Show all invoices together with the name of the sales agent for each one
SELECT invoice.* FROM "invoice" LEFT OUTER JOIN "Customer"  ON "invoice"."CustomerId" = "Customer"."CustomerId" RIGHT Outer JOIN "Employee" ON "Customer"."SupportRepId" = "Employee"."EmployeeId" WHERE Employee.Title LIKE '%Sales% %Agent%' ORDER BY "Employee"."EmployeeId"; 
-- Which sales agent made the most sales in 2009?
SELECT TOP 1 SUM(invoice."Total") AS totalsum FROM "invoice" LEFT OUTER JOIN "Customer"  ON "invoice"."CustomerId" = "Customer"."CustomerId" RIGHT Outer JOIN "Employee" ON "Customer"."SupportRepId" = "Employee"."EmployeeId" WHERE Employee.Title LIKE '%Sales% %Agent%' GROUP BY "Employee"."EmployeeId" ORDER BY totalsum DESC;
-- How many customers are assigned to each sales agent?
SELECT Employee.EmployeeId,COUNT("Customer".CustomerId) FROM "invoice" LEFT OUTER JOIN "Customer"  ON "invoice"."CustomerId" = "Customer"."CustomerId" RIGHT Outer JOIN "Employee" ON "Customer"."SupportRepId" = "Employee"."EmployeeId" WHERE Employee.Title LIKE '%Sales% %Agent%' GROUP BY "Employee"."EmployeeId";
-- Which track was purchased the most in 2010?
SELECT TOP 1 Track.Name,SUM(Track.UnitPrice) as totalSum FROM Track RIGHT OUTER JOIN "InvoiceLine" ON "Track"."TrackId" = "InvoiceLine"."TrackId" RIGHT OUTER JOIN "Invoice" ON "Invoice"."InvoiceId" = "InvoiceLine"."InvoiceId"  WHERE YEAR("Invoice"."InvoiceDate") = '2010' GROUP BY "Track"."Name" ORDER BY totalSum DESC;
-- Show the top three best selling artists.
SELECT TOP 3 Artist."Name",SUM(Track.UnitPrice) as totalSum FROM "Artist" 
INNER JOIN "Album" ON "Artist"."ArtistId" = "Album"."ArtistId" 
INNER JOIN "Track"  ON "Album"."AlbumId" = "Track"."AlbumId" 
Left OUTER JOIN "InvoiceLine" ON "Track"."TrackId" = "InvoiceLine"."TrackId" GROUP BY "Artist"."Name" ORDER BY totalSum DESC;
-- Which customers have the same initials as at least one other customer?
SELECT "Customer1"."FirstName","Customer1"."LastName" FROM "Customer" as Customer1 
INNER JOIN "Customer" as Customer2 ON "Customer1"."CustomerId" <> "Customer2"."CustomerId" WHERE 
SUBSTRING("Customer1"."FirstName",1,1) = SUBSTRING("Customer2"."FirstName",1,1) AND 
SUBSTRING("Customer1"."LastName",1,1) = SUBSTRING("Customer2"."LastName",1,1);


-- ADVANCED CHALLENGES
-- solve these with a mixture of joins, subqueries, CTE, and set operators.
-- solve at least one of them in two different ways, and see if the execution
-- plan for them is the same, or different.

-- 1. which artists did not make any albums at all?
SELECT Artist."Name" FROM "Artist" 
EXCEPT
SELECT ARTIST."Name" FROM "Artist"
INNER JOIN "Album" ON "Artist"."ArtistId" = "Album"."ArtistId";
-- 2. which artists did not record any tracks of the Latin genre?
SELECT "Artist"."Name" FROM "Artist"
EXCEPT
SELECT "Artist"."Name" FROM "Artist" JOIN 
"Album" ON "Artist"."ArtistId" = "Album"."ArtistId" JOIN
"Track" ON "Track"."AlbumId" = "Album"."AlbumId" JOIN
"Genre" ON "Genre"."GenreId" = "Track"."TrackId" WHERE
"Genre"."Name" = 'Latin';

-- 3. which video track has the longest length? (use media type table)
SELECT TOP 1 "Track".Name FROM "Track" JOIN "MediaType" ON "MediaType"."MediaTypeId" = "Track"."MediaTypeId" WHERE "MediaType"."Name" = 'Protected MPEG-4 video file' ORDER BY "Track"."Milliseconds" DESC;
-- 4. find the names of the customers who live in the same city as the
--    boss employee (the one who reports to nobody)
WITH BossEmployee as (
SELECT E."EmployeeId",E."FirstName",E."LastName",E."City" FROM "Employee" AS E
EXCEPT
SELECT E."EmployeeId",E."FirstName",E."LastName",E."City" FROM "Employee" AS E JOIN
"Customer" ON E."EmployeeId" = "Customer"."SupportRepId"
)
SELECT "Employee"."FirstName","Employee"."LastName" FROM "Employee" JOIN "BossEmployee" ON "Employee"."EmployeeId" <> "BossEmployee"."EmployeeId" AND "Employee"."City" = BossEmployee."City"
-- 5. how many audio tracks were bought by German customers, and what was
--    the total price paid for them?

-- 6. list the names and countries of the customers supported by an employee EmployeeId
--    who was hired younger than 35.


-- DML exercises

-- 1. insert two new records into the employee table.

-- 2. insert two new records into the tracks table.

-- 3. update customer Aaron Mitchell's name to Robert Walter

-- 4. delete one of the employees you inserted.

-- 5. delete customer Robert Walter.