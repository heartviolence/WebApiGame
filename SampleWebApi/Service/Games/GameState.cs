using Assets.Scripts.Shared.Games.SkillCards;
using MessagePack;
using SampleWebApi.Service.Games.NPCs;
using SampleWebApi.Service.Games.Rooms;
using SampleWebApi.Service.Games.Selectors;
using System.Collections.Generic;

namespace SampleWebApi.Service.Games
{
    [MessagePackObject]
    public class GameState
    {
        [Key(0)]
        public int Credit { get; set; }
        [Key(1)]
        public List<int> Notes { get; set; } = new();

        [Key(2)]
        public List<SkillCardBook> SkillCardBooks { get; set; } = new();

        [Key(3)]
        public int Currentfloor { get; set; }
        [Key(4)]
        public IFloorRoom CurrentRoom { get; set; }
        [Key(5)]
        public CardSelector CardSelector { get; set; } = new();

        public void Start(List<string> characters)
        {
            foreach (var character in characters)
            {
                SkillCardBooks.Add(GetSkillCardBook(character));
            }

            Currentfloor = 1;
            Notes.Add(0);
            Notes.Add(0);
            Notes.Add(0);
            CreateRoom();
        }

        private SkillCardBook GetSkillCardBook(string ownerName)
        {
            var book = new SkillCardBook()
            {
                OwnerName = ownerName,
                Cards = new List<SkillCard>()
            };

            book.Cards.Add(new SkillCard()
            {
                OwnerName = ownerName,
                CardName = "0",
                Level = 1,
                Type = 0
            });

            book.Cards.Add(new SkillCard()
            {
                OwnerName = ownerName,
                CardName = "1",
                Level = 1,
                Type = 0
            });

            book.Cards.Add(new SkillCard()
            {
                OwnerName = ownerName,
                CardName = "2",
                Level = 1,
                Type = 0
            });

            return book;
        }
        public void SelectNPC(int index)
        {
            if (CardSelector.isActive())
            {
                return;
            }

            CurrentRoom.SelectNPC(this, index);
        }

        public void BuyItem(int index)
        {
            if (CardSelector.isActive())
            {
                return;
            }

            CurrentRoom.BuyItem(this, index);
        }

        public void BuyPowerUp()
        {
            if (CardSelector.isActive())
            {
                return;
            }

            CurrentRoom.PowerUp(this);
        }

        public void NextFloor()
        {
            if (CardSelector.isActive())
            {
                return;
            }

            if (CurrentRoom.CanGoNextFloor())
            {
                Currentfloor++;
                CreateRoom();
            }
        }
        public void BattleEnd()
        {
            if (CardSelector.isActive())
            {
                return;
            }

            CurrentRoom.BattleEnd(this);
        }

        private void CreateRoom()
        {
            switch (Currentfloor % 3)
            {
                case 0:
                    CurrentRoom = new BattleRoom();
                    break;
                case 1:
                    var npcRoom = new NPCRoom();
                    npcRoom.Npc = new NPC();
                    npcRoom.Npc.Initialize(string.Empty);
                    CurrentRoom = npcRoom;
                    break;
                case 2:
                    var shop = new ShopRoom();
                    shop.Reroll(this);
                    CurrentRoom = shop;
                    break;
            }
        }

        public void RerollShop()
        {
            if (CurrentRoom is ShopRoom shopRoom)
            {
                shopRoom.Reroll(this);
            }
        }

        public void SelectCard(int index)
        {
            CardSelector.Select(this, index);
        }
    }

}
