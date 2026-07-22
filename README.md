# QuickStop Mart: POSSystem (Console)
# Assignment 1 - Lumovy Technology Solutions
Description
-----------------
This is a small console Point-of-Sale (POS) learning project built in C# for .NET 10. It demonstrates core C# concepts from the training: classes and objects, collections (List / Dictionary / Queue / Stack), generics, async/await, using, StringBuilder, helper classes and simple JSON persistence with Newtonsoft.Json.

What the app does (high level)
- Maintains an in-memory product catalog (SKU, name, price, category, stock)
- Start a sale (new order) and add items to the cart by SKU and quantity
- Remove items and "undo" the last add (Stack)
- Compute totals (subtotal + tax) and format money amounts
- Checkout: simulates payment, reduces stock, writes a receipt to receipt.txt and saves the catalog to catalog.json
- Demonstrates a background queue processor that archives processed orders

Requirements
- .NET 10 SDK installed

Quick start (commands)
----------------------
Open a terminal (PowerShell) and run the commands below from the solution folder (C:\Users\ADMIN\source\repos\POSSystem):
- Create the console project 
  dotnet new console -n PosSystem 
  cd PosSystem 
- Compile it 
  dotnet build 
- Run the app
  dotnet run 
- Add the NuGet package 
  dotnet add package Newtonsoft.Json 
- Restore Dependencies
  dotnet restore 
- Run the app (option A: run the project in the folder):
  dotnet run

How to use the app (menu)
-------------------------
When the app runs you'll see a menu. Common actions:
- 1 Show Products: lists catalog items and stock
- 2 Add Product: add a new product to the catalog (SKU must be unique; adding a product with existing SKU updates it)
- 3 Add Item To Cart: add by SKU and quantity (validated against stock)
- 4 Remove Item: remove a full line by SKU
- 5 Undo Last Add: reverts only the quantity added by the last Add (Stack)
- 6 Checkout: processes payment (simulated), reduces stock, prints receipt to console and saves receipt.txt, and saves catalog.json
- 7 Save Catalog: manually persist current catalog to catalog.json
- 8 Exit: quit the app
- 9 Start New Sale: begin a new order and clear undo history

---
