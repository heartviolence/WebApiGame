using Assets.Scripts.Shared.GameDatas;
using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Util
{
    public static class UserAccountDetailExtension
    {
        public static GameItem Crystal(this UserAccountDetail user)
        {
            var crystal = user.GameItems.Where(i => i.Name == ItemNames.Crystal).FirstOrDefault();
            if (crystal == null)
            {
                var newItem = new GameItem()
                {
                    Name = ItemNames.Crystal,
                    Count = 0,
                };
                user.GameItems.Add(newItem);
                return newItem;
            }
            return crystal;
        }

        public static GameItem GameItem(this UserAccountDetail user, string name)
        {
            var gameItem = user.GameItems.Where(i => i.Name == name).FirstOrDefault();
            if (gameItem == null)
            {
                var newItem = new GameItem()
                {
                    Name = name,
                    Count = 0,
                };
                user.GameItems.Add(newItem);
                return newItem;
            }
            return gameItem;
        }
    }
}
