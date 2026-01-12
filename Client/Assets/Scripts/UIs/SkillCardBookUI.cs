using Assets.Scripts.Shared.Games.SkillCards;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UIs
{
    public class SkillCardBookUI : MonoBehaviour
    {
        public GameObject CardPrefab;
        public TMP_Text Ownername;
        public Transform CardList;

        public void Set(SkillCardBook book)
        {
            Ownername.text = book.OwnerName;
            Clear();
            foreach (var card in book.Cards)
            {
                Add(card.CardName, card.Level);
            }
        }

        public void Clear()
        {
            var childCount = CardList.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = CardList.GetChild(i);
                GameObject.Destroy(child.gameObject);
            }
        }

        public void Add(string cardname, int level)
        {
            var prefab = Instantiate(CardPrefab).GetComponent<TMP_Text>();
            prefab.gameObject.transform.SetParent(CardList);
            prefab.text = $"{cardname} : {level}Lv";
        }
    }
}
