using SampleWebApi.Model.Items;
using ServerShared.DbContexts; 

namespace SampleWebApi.Service.Users.Items
{
    public class GameItemService
    {
        public GameItemService()
        {
        }

        public void AddItem(UserAccountDetail user, string itemName, int count)
        {
            if (ProcessSpecialItem(user, itemName, count))
            {
                return;
            }
            var item = user.GameItems.Find(u => u.Name == itemName);
            if (item == null)
            {
                user.GameItems.Add(new GameItem { Name = itemName, Count = count });
                return;
            }

            item.Count += count;
        }

        bool ProcessSpecialItem(UserAccountDetail user, string itemName, int count)
        {
            switch (itemName)
            {
                case SpeicalItemNames.Crystal:
                    user.Crystal += count;
                    return true;
                default:
                    break;
            }

            return false;
        }
    }
}
