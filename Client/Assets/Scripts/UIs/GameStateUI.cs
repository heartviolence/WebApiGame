using Assets.Scripts.Shared.Games;
using Assets.Scripts.Shared.Games.Rooms;
using Assets.Scripts.Shared.Games.SkillCards;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UIs
{
    public class GameStateUI : MonoBehaviour
    {
        public TMP_Text Credit;
        public NoteUI Note;
        public SkillCardBookUI SkillCardBook1;
        public SkillCardBookUI SkillCardBook2;
        public SkillCardBookUI SkillCardBook3;
        public TMP_Text CurrentFloor;
        public TMP_Text RoomInfo;
        public TMP_Text cardSelector;
        List<SkillCardBookUI> uis;


        void Start()
        {
            uis = new();
            uis.Add(SkillCardBook1);
            uis.Add(SkillCardBook2);
            uis.Add(SkillCardBook3);
        }

        public void Set(GameState gameState)
        {
            Credit.text = $"Credit :{gameState.Credit}";
            Note.Set(gameState.Notes);

            for (int i = 0; i < gameState.SkillCardBooks.Count; i++)
            {
                uis[i].Set(gameState.SkillCardBooks[i]);
            }

            CurrentFloor.text = $"currentFloor {gameState.Currentfloor}";
            switch (gameState.CurrentRoom)
            {
                case BattleRoom battleRoom:
                    RoomInfo.text = $"BattleRoom,isClear {battleRoom.IsClear},Npc is null?:{battleRoom.Npc is null}";
                    break;
                case NPCRoom npcRoom:
                    RoomInfo.text = $"NPCRoom";
                    break;
                case ShopRoom shopRoom:
                    RoomInfo.text = $"ShopRoom,powerUpCount:{shopRoom.PowerUpCount}";
                    break;
            }

            cardSelector.text = $"ticketCount:{gameState.CardSelector.CardRewardTickets.Count}";

            foreach (var selection in gameState.CardSelector.Selections)
            {
                cardSelector.text += $",{selection.OwnerName}_{selection.CardName}->{selection.AfterLevel}Lv";
            }
        }
    }
}
