# Restaurant Manager Project
This is a restaurant manager project based on requirements from The C# Academy's Beginner Data Structure course.

The app allows the user to view the categories of menu items; view all menu items; add, remove and update menu items; undo the removal of menu items, find a menu item by its unique code, manage an items dietary labels (adding and deleting), view all currently outstanding orders, add an order, process an order, view the processed order history and finally view a summary of the restaurant.

## How to Use the Application:
This application uses .NET version 10.0.

This app uses the Spectre Console libary. To install it navigate to the project folder inside the terminal and enter the following command:

'dotnet add package Spectre.Console'.

## Using the App
Upon opening the app the user will be greeted by a menu with a figlet and several options to choose from. These options include: "View Menu Categories", "Manage Menu Items", "Manage Orders", "View Summary" and "Exit".

### View Menu Categories
Upon selecting this option the user will be shown all categories for menu items in a table. They will be prompted to press "Enter" to return to the previous menu.

### Manage Menu Items
Upon selecting this option the user will be greeted with another menu with several options. These include: "View all Menu Items", "Add Menu Item", "Delete Menu Item", "Update Menu Item", "Undo Removal", "Find Menu Item", "Manage Dietary Labels" and "Exit to Previous Menu".

#### View all Menu Items
Upon selecting this option the user will be shown all currently stored menu items along with their details inside a table.

#### Add Menu Item
Upon selecting this option the user will be prompted to enter details for a new menu item. The must choose its unique code, name, category, and price. Once these options are entered they will be prompted to confirm that they wish to add the item. If they enter 'y' then the item is added and a message is displayed. Else a message will display confirming that the item was not added and the user will be returned to the menu.

#### Delete Menu Item
Upon selecting this option the user will be prompted to select one of the existing menu items using its unique code. Once an item is selected they will be prompted to confirm their choice. If they enter 'y' then a message will appear confirming that the item has been removed. If they enter 'n' then a message will appear notifying the user that the item was not removed from the system.

#### Update Menu Item
Upon selecting this option the user will be prompted to select one of the existing menu items using its uniqeu code. Once selected then the user will be prompted to enter the new details for the item (its name, category and price). Once the details are entered the user will be prompted to confirm their choices. If they select 'y' then a message will appear confirming that the item has been updated. If they enter 'n' then a message will appear notifying the user that the item was not updated.

#### Undo Removal
Upon selecting this option, if there are menu items that have been removed, the user will be asked if they would like to undo the removal of the last removed item. If they enter 'y' then a message will display confirming that the item was restored, they will then, as long as there are more removed items, be asked to confirm if they would like to recover the next removed item. This continues until all removed items are recovered or the user enters 'n' when prompted.

#### Find Menu Item
Upon selecting this option the user will be asked to select from a list of currently existing menu item unique codes. Once selected the items details will be printed.

#### Manage Dietary Labels
Upon selecting this option the user will be prompted to select a currently existing menu item. Once selected another menu will appear. This menu has three options: "Add Dietary Label", "Remove Dietary Label" and "Exit to Previous Menu". Upon selecting "Add dietary label" the user will be asked to enter a new label for the item. If the label already exists then a message will appear and the user will be prompted again. Once a valid label is entered, the user is asked to confirm that they wish to add the label. If the user enters 'y' then a message will appear confirming that the lable has been added. If the user enters 'n' then a message will appear notifying the user that the label was not added. Upon selecting "Remove Dietary Label" the user will be asked to select which label they wish to remove and subsequently confirm thier choice. Selecting "Exit to Previous Menu" wil return the user to the previous menu.

#### Exit to Previous Menu
Upon selecting this option the user will be returned to the main menu.

### Manage Orders
Upon selecting this option the user will be presented with another menu with several options. Thes include: "View all Outstanding Orders", "Add Order", "View Order history" and "Exit to Previous Menu".

#### View all Outstanding Orders
Upon selecting this option the user will be presented with a table of all currently unprocessed orders.

#### Add Order
Upon selecting this option the user will be prompted to enter the details of the order. First they are asked to select the table number from a list of options and then enter the item count. Once the data is entered the user is asked to confirm that they wish to add the order.

#### Process Order
Upon selecting this option the user will be asked, for as long as there are unprocessed orders, if they wish to process the earliest created order. If there are no unprocessed orders a message appears and the user is returned to the previous menu. This continues until the user enters 'n', in which case they are returned to the menu.

#### View Order History
Upon selecting this option the user is presented with a table of processed orders, if orders have been processed.

#### Exit to Previous Menu
Upon selecting this option the user is returned to the previous menu.

### View Summary
Upon selecting this option the user is presented with two tables. One contains a column of categories and another column with their item count. The next contains a general summary comprising of a column containing 'Categories', 'Menu Items', 'Recoverable Menu Items', 'Outstanding Orders', 'Processed Orders', and another column containing their respective counts.

### Exit
Upon selecting this option the application will close.

## Collection Decisions

### MenuCategories
I decided to use an array for this collection as the size is fixed and I wished to use its index when creating a table.

### MenuItems
I decided to use a List<T> for this collection as I needed to change its size dynamically and for it to be ordered.

### MenuItemsByUniqueCode
I decided to use a Dictionary<T> for this collection as it would allow me to access menu items via their unique code (fast lookup).

### RemovedMenuItems
I decided to use a Stack<T> for this collection as it would allow me to access and restore the most recently deleted item.

### DietaryLabels
I decided to use a HashSet<T> for this collection as I needed to be able to store a label for each MenuItem object. They cannot have duplicate values and their order is not important.

### OutstandingOrders
I decided to use a Queue<T> for this collection as I needed to access the earliest added order when processing. Each item added to the collection needed to go to the back of the collection or the "queue".

### ProcessedOrders
I decided to use a List<T> for this collection as I needed to change its size dynamically.

## Creating the App
The first thing I decided to implement was the main menu that the user would land on. To do this I created a new class called RestaurantController. This would control the flow of my application. Inside I created a Run() method containing a loop. Inside the loop I created a switch statement with the selectable menu options. The option is gathered using a method inside the UserInput class. The GetMainMenuInput() method is called and returns the menu choice. In the Program class I created a new instance of the RestaurantController class and called the Run() method, displaying the main menu to the user at runtime.

The next thing I decided I needed to do was implement the ability for the user to access all features of the app through the menus. I believed this approach would make it more intuitive to implement the features down the line. First I added four new methods to RestaurantController. These were ViewMenuCategories(), ManageMenuItems(), ManageKitchenOrders() and ViewRestaurantSummary(). To add some placeholder functionality to ViewMenuCategories() I created a new class called TablePrinter. This contains a method called PrintCategoriesTable(). It remains blank for now. ViewMenuCategories() also calls a new method from UserInput; Continue() which prompts the user to press 'Enter' to continue. This would be the only functionality I add to menu options for the time being to ensure that each option is reachable by the user. In the ManageMenuItems() method I created another loop. This loop calls a new method each time to ensure that the menu is the only thing being displayed; the ClearDisplay() method from UserInput. The menu option is gathered using another method from UserInput; GetMainMenuInput() which takes a Menu enum value. I created this enum to ensure that I did not need to create a new method to display each menu. This replaces the GetMainMenuInput() method. Inside UserInput I also created some more methods to help me display information to the user throughout different features in my app. These include: confirm() returning true of false based on user input and Error() printing a message when some issue occurs or when there is no data to display. The switch statement inside ManageMenuItems() calls several method from a new class called ManageMenuItemsController. These include: ViewMenuItemsByCategory(), later changed to ViewAllMenuItems(), AddMenuItem(), DeleteMenuItem(), UpdateMenuItem(), UndoRemoval(), FindMenuItem() and ManageDietaryLabels(). All of these methods call the Continue() method as a placeholder for now besides ManageDietaryLabels() which shows another menu controlled by a switch statement. This switch statement calls two methods from another class called ManageDietaryLabelsController. These methods are AddLabel() and RemoveLabel() both of which call the Continue() method as a placeholder. Another option called from the main menus switch statement is the ManageKitchenOrders() method which displays another menu based on a switch statement. This switch statement calles several methods including: ViewAllOutstandingOrders(), AddOrder(), ProcessOrder() and ViewOrderHistory(). The final method called by the main menus switch statement is the ViewRestaurantSummary() method.

Next I created the functionality to view all categories in the ViewMenuCategories() method. In the TablePrinter class I added a string array parameter to PrintCategoriesTable() which would allow me to pass the categories array to the method. It was at this point I started adding checks to the methods to ensure that should there be no data to display, the method could exit gracefully. If categories are present the method creates a new table with two columns: Number and Category. Then I looped over the categories array and for every category I create a new row with its number and the category name itself. The table is then printed.

I then decided to implement the ability to view all MenuItems and add a MenuItem. To do this I first needed to create a new class called MenuItem which requires an Id, a Code, a Name, a Category, a Price and can have a set of DietaryLabels. In my TablePrinter class I created a new PrintMenuItemsTable() method that takes a List<T> of MenuItems. The list is ordered by category. A check is run before entering this method to ensure that the list is not empty, then if not the method is called. A table is created with a column for each property of MenuItems (excluding DietaryLabels at this point). The list is then looped over and then a row is added for every MenuItem in the list. The table is then printed. At this point I decided to rename ViewMenuItemsByCategory() to ViewAllMenuItems(). Inside the AddMenutItem() method I created a loop that clears the display each time and creates a new MenuItem by calling the GetMenuItemDetails() inside UserInput. This method writes a message to the user describing what they must do. They are prompted for a string value for the unique code, a string for the name, a string for the category, and a decimal for the price. Id is created using the NextMenuItemId field from the ManageMenuItemsController (it starts at 1 and will be incremented each time a new MenuItem is added). The method then returns the new MenuItem. Back in ManageMenuItemsController I decided to create a method to check the validity of the newly created MenuItem with the ValidateMenuItem() method. It takes the item as a parameter and runs two checks to ensure that it does not share the same name as another item (regardless of case) and that the code is unique. If either of these checks succeed then the menuItem is not added. If these checks fail then the SyncMenuItemAddition() method runs which takes the item as a parameter. This method adds the item to the List<T> of MenuItems and the Dictionary<T> of menuItems with their unique code as a key. 

Before implementing the ability to delete a MenuItem I removed the loop in AddMenuItem() as I believed it would make a frustrating experience for the user. Now when an item with a name or code that already exists is attempted to be added a message is printed and the method is exited. The user must select the option again should they wish to add a MenuItem. Then, to implement the ability to delete a MenuItem I created a Stack<T> called RemovedMenuItems, which would store my removed items. In DeleteMenuItem() a check is run to ensure that there are items to delete, else the user is returned to the menu. A menuItem is then created using the GetMenuItemByCode() method in UserInput. This returns a MenuItem based on a string gathered from a SelectionPrompt corresponding to an items unique code using Spectre Console. A confrmation prompt is then shown to the user asking if they wish to remove the item. If they refuse they are returned to the menu, else the SyncMenuItemDeletion() method is run. This method takes the item as a parameter and removes the item from both the MenuItems List<T> and MenuItemsByUniqueCode Dictionary<T>.

To implement the ability to undo the removal of a MenuItem, in the SyncMenuItemDeletion() method I pushed the MenuItem to the RemovedMenuItems Stack<T>. In the UndoRemoval() method I created a check on RemovedMenuItems to ensure that there is data stored before proceeeding. I decided to use a loop here that would allow the user to continuously restore items as long as there are items left to restore. The loop uses the TryPeek() method on the collection and creates an out parameter for the lastRemovedItem. The user is asked if they wish to restore this item. If so the item is popped from the collection and the SyncMenuItemAddition() method is called adding the MenuItem back to both the MenuItems and MenuItemsByUniqueCode collections. If the user cancels any restoration they are returned from the method to the menu. Once there are no more items to restore a message is printed.

Before creating functionality for the UpdateMenuItem() method I altered the GetMenuItemDetails() methods name to GetNewMenuItemDetails() and created a new method in UserInput called GetUpdatedMenuItemDetails(). This method prompts the user for a name string, a category string and a price decimal. It returns all three values. In the UpdateMenuItem() method I created a check to ensure that there are items to update. An item is aqcuired using the GetMenuItemByCode() method. A name string, category string and price decimal are aqcuired using the GetUpdatedMenuItemDetails() method. The user is asked to confirm that they wish to update the selected MenuItem. If so then the existing MenuItems Name, Category and Price properties are updated with the newly gathered details.

Next I decided to create the functionality for the FindMenuItem() method. First I run a check to ensure that items exist to search for. Then I create a MenuItem using the GetMenuItemByCode() method. I then pass that item as a parameter to the FoundMenuItemDetails() method in UserInput. This method prints each detail for the selected MenuItem.

I then needed to implement the functionality for adding a label to MenuItems DietaryLabels. In the ManageDietaryLabels() method in ManageMenuItemsController I created a MenuItem using GetMenuItemByCode() and then passed that item as a parameter to both AddLabel() and RemoveLabel() (for later). In the AddLabel() method inside ManageDietaryLabelsController I create a new string using GetNewLabel(), a method inside UserInput. This method prompts the user for a new string and validates that it does not already exist. It then returns this label. The user is then asked to confirm that they wish to add this label and if so it is added to the MenuItems DietaryLabels HashSet<T>.

Next I needed to implement the functionality for removing a label. In RemoveLabel() I create a string calling the GetDietaryLabel() method in UserInput. This method takes a selected MenuItem as a parameter and runs a check to ensure that the item has labels to remove. The user is then prompted to select the label they wish to remove and it is returned. The user is asked to confirm that they wish to remove the label and if so the label is removed.

To make more of the apps features available to the user on runtime without having to add data I decided to add some seed data for MenuItems. I created a new class called MenuItemSeeder. In the SeedMenuItems() method I created a new List<T> containing two MenuItems of each category all containing data. The method then returns this data. The method is called when MenuItems is initialised. I created another method inside ManageMenuItemsController called GetNextIdNumber() which gets th max Id from the MenuItemsList and retuns it after adding 1 to the number. This method is called on the initialisation of NextMenuItem.

I then needed to add functionality to the ViewAllOutstandingOrders() and AddOrder() methods. First I created a new class called Order. Each Order would have an Id, TableNumber, Items, and an OrderedAt property (it would later have an option ProcessedAt property). In the ManageKitchenOrders class I created a new Queue<T> called PendingOrders. PendingOrders is initialised and calls the SeedOrders() method to get its values. In the OrderSeeder class inside SeedOrders() I created a new List<T> that is instantiated with several Order objects. This List<T> is then returned and subsequently used to seed data to PendingOrders. I created a field member called NextOrderId which calls a method GetMaxOrderId() to get its value. GetMaxOrderId() returns the highest currently existing Id from Order objects in PendingOrders. In the ViewAllOutstandingOrders() method the PrintOutstandingOrdersTable() method is called, using PendingOrders as a parameter. This method creates a table with a column for each detail of an Order and creates a row for each order in PendingOrders. The table is then printed. In the AddOrder() method a new Order object is created  using the GetNewOrderDetails() method from UserInput. This method prompts the user to select a TableNumber, an ItemCount and creates a DateTime value based on the current time. Id is gathered using the NextOrderId field and incrementing it by 1. These values are used to create a new Order which is returned from this method. The user is asked to confirm that they wish to add this Order and if so it is enqueued and nextOrderId is incremented.

Before implementing the functionality of processing an order I decided it would be a good idea to add the optional ProcessedAt property to the Order class. I also changed the name PendingOrders to OutstandingOrders to align it more with the names of the methods. I decided to create a new List<T> called ProcessedOrders which would store the processed orders. In the ProcessOrder() method I create a loop which uses TryPeek() creating an out variable of an Order from OutstandingOrders. The user is then asked, for as long as orders exist to process, if they would like to process the current order. If so the TryDequeue() method is called on OutstandingOrders and the ExecuteOrderProcessing() method is called. Inside this method the Orders ProcessedAt property is given the value of the current time and is added to the ProcessedOrders collection. If the user cancels the processing of an order the order is not processed and the user is returned the the menu.

Next I decided to implement the functionality to view the processed order history. In the ViewOrderHistory() method the PrintProcessedOrderHistory() method is called using the ProcessedOrders as a parameter. In this method inside TablePrinter a new table is created with columns for every detail of an Order (including the ProcessedAt property). The List<T> is then looped over and a row is created for every Order in the list. The table is then printed.

The final feature of the app that needed to be implemented was the ability to view the restaurants summary. In the ViewRestaurantSummary() method I added several prarameters: the categories, menu items, recoverable menu items, outstanding orders, processed orders. I run a check to ensure that each of these collections have values and if they do not an error is displayed and the user is returned to the main menu. The PrintRestaurantSummary() method is then called with these collections also passed as prarameters. In this method two other methods are called: PrintCategoriesTable() which takes categories and menu items as parameters. It creates a new table with a column for categories and another for the count of MenuItems containing that category. A row is added for every category. The table is then printed; and PrintGeneralSummary() which takes categories, menu items, recoverable menu items, outstanding orders, and processed orders as parameters. A table is created with two columns. A row is created for categories and its count, menu items and its count, recoverable menu items and its count, outstanding orders and its count, processed orders and its count. The table is then printed.

To make the app more appealling and easier to use I decided to add styling to prompts and table headings using Spectre Console. This made errors, prompts, messages and confirmation checks more clear and understandable. I also decided to add a figlet to the main menu as a finising touch.

## My Experience and Key Takeaways
This project, while not too dissimilar to the last made me more comfortable working with the different types of collections and making the deicision on which to use. I also found that creating the flow of the app before its features made it easier for me to develop as opposed to doing both simultaneously. I believe I will adopt this approach in future projects.



