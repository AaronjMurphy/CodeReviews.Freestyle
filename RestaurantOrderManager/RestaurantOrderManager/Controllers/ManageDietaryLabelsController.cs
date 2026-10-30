using RestaurantOrderManager.Models;
using RestaurantOrderManager.UI;

namespace RestaurantOrderManager.Controllers
{
    internal class ManageDietaryLabelsController
    {
        internal static void AddLabel(MenuItem itemToManage)
        {
            string label = UserInput.GetNewLabel(itemToManage);
            if (!UserInput.Confirm("Add this label to the menu item?"))
            {
                UserInput.WriteMessage("The label was not added.");
                UserInput.Continue();
                return;
            }
            itemToManage.DietaryLabels.Add(label);
            UserInput.WriteMessage("The label was added.");
            UserInput.Continue();
        }

        internal static void RemoveLabel(MenuItem itemToManage)
        {
            string labelToRemove = UserInput.GetDietaryLabel(itemToManage);

            if (labelToRemove == string.Empty)
            {
                return;
            }

            if (!UserInput.Confirm("Are you sure you wish to remove this label?"))
            {
                UserInput.WriteMessage("The label was not removed.");
                UserInput.Continue();
                return;
            }

            itemToManage.DietaryLabels.Remove(labelToRemove);
            UserInput.WriteMessage("Label was removed.");
            UserInput.Continue();
        }

    }
}
