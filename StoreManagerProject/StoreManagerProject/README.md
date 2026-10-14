
# Store Management Project

This is a store management project based on requirements from The C# Academy's Beginner Data Structures course.

The app allows the user to view the store departments; manage products for each department, including viewing, adding, updating, removing, undoing a removal, finding a product by its SKU, and managing its tags; manage orders, including viewing the pending orders, creating, processing, and viewing the order history; and view a summary of the store.

## Dependencies
This app requires the spectre console library. To install it navigate to the project folder in the terminal and enter the following command:

"dotnet add package Spectre.Console". 

## Using The App

Upon opening the app the user is greeted with a menu. The options included are "View Departments", "Manage Products", "Manage Orders", "Store Summary" and "Exit".

### View Departments
This option will present the user with all departments and their number in a table.

### Manage Products
This option will present the user with several options. These include:

#### View Products:
This option will present the user with a table containing all products and their information.

#### Add Product:
This option will present the user with prompts to enter the details of a new product. After all the information is entered it will ask if the user wishes to store this product.

#### Update Product:
This option will present the user with a prompt to select he product they wish to update. Upon the selection they will be asked to enter the new details for the product. At the end it will once again ask for confirmation.

#### Remove Product:
This option will present the user with a prompt to select the product they wish to remove. Once an option is selected the user will be asked to confirm their choice.

#### Undo Product Removal:
This option will present the user with a prompt to restore the most recently removed product as long as there are products that were removed.

#### Find Product:
This option will present the user with a prompt to enter an SKU to search for a product. If one is found its details will be printed.

#### Manage Product Tags:
This option will present the user with a prompt to select a product to manage its tags. Once selected the user may choose to add or remove a tag or return to the product management menu. Adding a tag is handled similarly to entering a products details. To remove a tag the user must select a tag from the menu. The user will then be asked to confirm their decision.

#### Search Product With Tags:
This option will present the user with a prompt to enter tag. If a product contains this tag it will be printed in a table with its details.

#### View Low Stock Products:
If there are any products with less than 10 in stock it will be printed in a table based on its department.

#### Return To Main Menu:
This returns the user to the previous menu.

### Manage Orders
This option will present the user with another menu with several options. These include:

#### View Pending Store Orders:
This option will present the user with a table containing each currently pending order and its details.

#### Create Order:
This option will present the user with prompts to enter details for a new order. After all the details are entered the user will be asked to confirm if they wish to add it to the pending orders.

#### Process Next Order:
This option will present the user with the option to process the earliest pending order. It will continue to prompt the user until all orders are processed or the user cancels processing an order.

#### View Order History:
This option will present the user with a table containing the 5 latest processed orders. 

#### Return To Main Menu:
This option will return the user to the previous menu.

### Store Summary

This option will present the user with a summary of the store, including the amount of departments, products, unique store tags, removed products, currently pending orders in one table and each department and the number of products it contains in another table.

### Exit

This option will close the application.

## Data Structure Decisions

### Menu Options
The menu options for the main menu, managing products, managing orders and managing tags use an array collection. This is because the number of options is fixed.

### Departments
For Departments I also elected to use an array. The number of departments is fixed and later in the code I required their numbered indexes in a loop when reporting low stock products by their departments.

### Products
For Products I decided to use a List. Lists are dynamically sized meaning that their size can increase or decrease. I needed to add or remove products based on user input so a List felt most appropriate.

### Products By SKU
ProductsBySku uses a Dictionary. While storing Products in a List can work well, each Product has an SKU. This SKU can be used to locate a product quickly(fast lookup), therefore a Dictionary was a necessary inclusion.

### removedProducts
When a product is removed from both Products and ProductsBySku, it is completely erased from memory. If we wish to store this data in the event it needs to be restored it is best to use a Stack. A stack operates based on LIFO (Last In, First Out) meaning that the newest item added is the first item to be accessible. With this we can push products to a Stack and pop the most recent to restore it when necessary.

### Product Tags
Each product has a Tags field which contains a set of descriptive words for the product. I decided to use a HashSet to achieve this as the order of the tags is not important. I decided to add a String Comparer to make the HashSet case insensitive. This ensures that duplicate tags cannot be entered and, when searching for a Product with a user entered tag, it is accepted regardless of case.

### pendingOrders
When an order is created it is generally expected that it is added to a queue of orders and that the earliest added order is processed first. To achieve this I decided to implement a Queue to store pending orders. When a user decides to process an order it shows them the earliest order first. This is known as FIFO (First in, First out).

### CompletedOrderHistory
An order history would usually be shown from earliest to newest. It also needs to be resized dynamically. For these reasons I decided to use a List for this feature.

## Creating the App

The first thing I did after creating the project was to create a new file called StoreApp.cs. This file would contain the code for my application. This file is accessed in Program.cs by creating a new instance of this class and calling its Run() method. This method contains an AnsiConsole prompt with options from an array called MenuOptions. The selected option is then read by a switch statement which controls the execution flow of the app.

The first feature I added was the option to view the stores departments. These department were stored in an array as the collections size is known and i would later be using each departments index in a loop. This feature is created inside a method called ViewDepartments(). A new Table is created with a column for the departments name and number. The Departments array is checked to see if it is null and if so prints an error message and returns the user to the main menu. If this check fails then all of the departments are printed in a table for the user.

The next MenuOptions feature I implemented was another menu inside a method called ManageProducts(). This was created much like the previous menu and contains several paths for the user to choose to manage the stores products.

I created a new file that would contain my product management logic called ProductManagement. The first option i implemented was a ViewProducts() method. To do this I would need some products to display so I created a new Product.cs class file inside a Models folder. Each product must have an SKU, a name and a department. They can also have a price, stock quantity, and a set of tags(implemented later). I decided to store these products inside a List<T> called Products as this collection would need to be resized dynamically. Next I created a new file called ProductListSeeder inside a Data folder. This file contains a method which seeds product data into the Products List<T> by calling it inside the StoreApp.cs file on the Products List<T> initialisation. Next, in the ViewProducts() method, I made a check to see if there are any products. If none are found then a message is printed and the user is returned to the previous menu. If there are products, they are printed in a table alongside their details.

Next came the AddProduct() method. The user is prompted for:
- An SKU with a check to ensure that the entered SKU does not already exist and that text has been entered.
- A Name with a check to ensure that text is entered.
- A Department which is aqcuired via a selection prompt to protect from invalid entries.
- A Price with a check to ensure that it is not a negative number.
- A Stock Quantity with a check to ensure that it is not a negative number.
- Confirmation that they wish to add this product to the store records.
If the user enters "y" at the confirmation prompt then a new Product is created and added to the Products List<T>. Else the user is returned to the main menu. 

Next I created an UpdateProduct() method. In order to access a product easily, it was necessary to create a new Dictionary<T> that would contain a Products SKU as its key and the Product itself as a value. I altered AddProduct to add a Product to Products and the new Dictionary<T>, ProductsBySku. Before using this I created a check to ensure that Products was not empty. Then I was able to use ProductsBySku to select a product by matching a key to an SKU user input gathered with a selection prompt. I could then access the relevant Products properties and prompt the user for input on each similarly to how AddProduct() did.

Then I created a RemovedProduct() method. This checks Products to ensure it is not empty and returns the user to the main menu if so. The user is prompted to select a product and then confirm that they wish to remove it. If they enter "y" then the product will be removed. This also required the Product to be removed from both Products and ProductsBySku. However, when it is removed, it is not recoverable. In order to implement the next feature I decided to create a Stack<T> called removedProducts. A Stack<T> is perfect for this feature as the most recently added product should be the first product to recover (LIFO). Now when a product is removed from the previous collections it is added to this Stack<T>.

Next, I created a FindProductBySku() method. The user is prompted to enter an SKU to search for a product. The user input is validated as always, and if a product is found that product is printed to a table with its details.

To implement the restoration of products, I created an UndoRemoval() method. First the removedProducts Stack<T> is checked to ensure that it is not empty.  If it is the user is returned to the previous menu.This check uses the TryPeek() method which requires an out paramater, in this case the product that was last removed. The user is asked to confirm that they wish to restore the product and upon entering "y", the product is restored to both collections and removed from removedProducts.

For the next option I needed to add a collection of tags to each product. To achieve this I decided to use HashSet<T> as each tag must be unique, but their order is not important. I instantiate a new HashSet<T> called Tags on the creation of each Product object and included a String Comparer to make them case-insensitive. I later added some sample data to each Tags HashSet<T> in ProductListSeeder. With this, I was ready to create another method called ManageTags(), which was responsible for prompting the user to select a product via its SKU (If a product does not exist the user is returned to the main menu.), aqcuiring the product via the sku and then prompting the user to select what they would like to do with the product. The options are once again from an array due to the size being known.

The first option in the tag management menu is Add Tag. To implement this feature I created an AddTag(Product product) method which takes a product(the selected product from ManageTags()) as a parameter. The user is prompted to enter a tag and it is checked to ensure that it does not already exist in the products Tags collection. If all the validation checks succeed then the tag is added.

The next option in the menu is Remove Tag. This also takes a product as a paramater. To implement this feature I created a RemoveTag() method. This checks that the products Tags collection is not empty. If it is then the user is returned to the previous menu. The user is prompted to select a tag with a selection prompt, and then asked to confirm that they wish to remove the tag. If the user enters "y" then the tag is removed. Else they are returne to the previous menu.

The final tag management option is "Back to Product Management", which if selected breaks the loop and returns the user to the product management menu.

Next, I decided to implement the order management menu in StoreApp.cs. This is done inside a ManageOrders() method, called in the main menus switch statement. This, like the other menus is created using a while loop. The user us prompted to select from several options. If the user selects "Return to Main Menu", the loop breaks and the user is returned to the previous menu.

Before implementing the menu options I decided to create some seed data for the stores orders. First I created a new class file in the Models folder called Order.cs. Each order must have an Id, a CustomerName, and they can have an ItemCount, a CreatedAt date and a ProcessedAt date. Orders would need to be processed in the order that they were added, so in this case I decided to create a Queue<T> called pendingOrders. In the Models folder I created a new file called OrderQueueSeeder, which contains an array of orders instatiated with data. The method returns a new Queue<T> with the orders array passed as a parameter. The pendingOrders Queue<T> is instantiated by calling this method inside StoreApp.cs. 

Next I created a OrderManagement file which would handle the logic for managing orders. The first method I created was ViewOrders() which checks that pendingOrders has data. If it does not then the user is returned to the previous menu. A table is created with columns for each of an orders properties. Every existing order is printed in this table along with its details.

Then I implemented the CreateOrder() method. This method prompts the user for an orders detals and then creates a new order using the input. The user is asked to confirm if they wish to add the order. If they enter "y", then the order is enqueued. Else they are returned to the main menu. The _nextOrderNumber field is incremented each time an order is enqueued.

Next came processing an order. To do this I created a ProcessNextOrder() method. This first checks that pendingOrders has data, returning the user to the previous menu if so. I decided to use a while loop for this feature as it would enable the user to process orders continually until there were none left. For every order its details are printed and the user is prompted to confirm that they wish to process it. If they enter "y", then the order is dequeued from pendingOrders. It was at this stage that I decided to add a ProcessedAt property to every order. This uses DateTime.Now to add this value to the processed order. When no orders are left to process then a message is printed and the user is returned to the main menu.

I then decided to implement the main menus store summary option. This is done using a ViewStoreSummary() method. First all currently existing tags are aqcuired using a GetUniqueStoreTags() method. This method loops over each products Tags collection and joins them together using the UnionWith() helper method. This is returned when the method is called. After this HashSet<T> is initialised, I created a table which contains seveal information about the store. These include: The number of departments, number of products, the number of unique store tags, number of removed products and the number of pending orders. This table is printed. Then another table lists each department with its product count.

To make the app more readable for the user I decided to add some styling to text, tables and prompts using Spectre Console. This included using Markup.Escape on variables to ensure that if a user entered data with square brackets, the code would not interpret them as styling. I then used the Trim() method on user input to ensure that no leading and trailing whitespace was present. As an extra decoration I added a figlet (A Spectre Console feature) to the main menu screen.

To ensure that I was following the DRY(Don't Repeat Yourself) principle, I created a single method that would handle product collection synchronisation AddProductCollectionsSync(). This is called when adding, undoing a removal of a product. Another method, RemoveProductCollectionsSync(), is used when removing a product from both collections.

Next I decided to implement a ViewOrderHistory() method. This would be called through the Order Management menu. I created a new List<T> called CompletedOrderHistory. I decided to use a List<T> because the size of the collection would need to change dynamically and its order was important. Back in the ProcessNextOrder() method, I now add an order to the list every time one is processed. When ViewOrderHistory() is called a check is run on CompletedOrderHistory collection to see if it is empty. If it is then the user is returned to the previous menu. If the check is passed then a new table is created with columns for each detail in an order. Then each order is printed to that table.

To add another quality of life feature to the app, I decided to create a SearchProductWithTag() method. This prompts the user to enter a tag to search for and every product that contains that tag is added to a List<T>. The List<T> is checked to ensure that products are present and, if not, the user is returned to the previous menu. If the check is passed then a table is created that contains a column for details of a product. Each product from the List<T> is added to the table and printed.

Another feature that was important to add was a ViewLowStockProducts() method. A variable is created that determines the threshold for low stock products. This is then used to create a collection of products ordered by their departments. A check is run on this collection to ensure that it is not empty and returns the user to the preivous menu if so. Inside a loop a table is created for every department in the store that contains a product of low stock, and then another nested loop adds the products to that loop. The table is then printed.

The final addition to the app was a RemoveWithLimitHistory() method. This method is now called when removing a product from the store. It pushes the product to the removedProducts Stack<T>. It checks that removedProducts does not meet the RemovalHistoryLimit(5) and does not continue if so. If removedProducts does meet the limit then removedProducts is stored in an array using the ToArray() method. removedProducts is cleared. A loop pushes the last 5 removed products to the Stack<T>.


