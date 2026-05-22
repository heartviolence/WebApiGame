using ServerShared.DbContexts;

namespace SampleWebApi.Service.Users.Items
{
    public class GameItemService
    {
        public GameItemService()
        {
        }

        public int AddItem(UserAccountDetail user, string itemName, int count)
        {
            var item = user.GameItems.Find(u => u.Name == itemName);
            if (item == null)
            {
                user.GameItems.Add(new GameItem { Name = itemName, Count = count });
                return 0;
            }
            var beforeItemCount = item.Count;
            item.Count += count;
            return beforeItemCount;
        }
    }
}
