using Assets.Scripts.Shared.Games.Selectors;
using Assets.Scripts.Shared.Games.SkillCards;
using MessagePack;

namespace SampleWebApi.Service.Games.Selectors
{
    [MessagePackObject]
    public class CardSelector
    {
        [Key(0)]
        public List<SkillCardRewardTicket> CardRewardTickets { get; set; } = new();
        [Key(1)]
        public List<SkillCardReward> Selections { get; set; } = new();

        public void AddPowerUpCards(GameState gameState, List<int> ownerIndexes)
        {
            CardRewardTickets.Add(new SkillCardRewardTicket()
            {
                RewardRange = 1,
                CardOwnerIndex = ownerIndexes
            });

            if (Selections.Count == 0)
            {
                TicketToNewCard(gameState);
            }
        }

        public void AddNewCards(GameState gameState, List<int> ownerIndexes)
        {
            CardRewardTickets.Add(new SkillCardRewardTicket()
            {
                RewardRange = 0,
                CardOwnerIndex = ownerIndexes
            });

            if (Selections.Count == 0)
            {
                TicketToNewCard(gameState);
            }
        }

        public void Select(GameState gameState, int index)
        {
            if (Selections.Count - 1 < index)
            {
                return;
            }
            var select = Selections[index];
            Selections.Clear();
            gameState.SkillCardBooks
                .Where(b => b.OwnerName == select.OwnerName)
                .FirstOrDefault()
                .Cards.Where(c => c.CardName == select.CardName)
                .FirstOrDefault().Level = select.AfterLevel;
            if (CardRewardTickets.Count > 0)
            {
                TicketToNewCard(gameState);
            }
        }

        private void TicketToNewCard(GameState gameState)
        {
            var ticket = this.CardRewardTickets[0];
            var candidates = new List<SkillCard>();

            foreach (var ownerIndex in ticket.CardOwnerIndex)
            {
                List<SkillCard> target = gameState.SkillCardBooks[ownerIndex].Cards.Where(c => c.Level < 6).ToList();
                if (ticket.RewardRange == 1)
                {
                    target = target.Where(c => c.Level != 0).ToList();
                }
                candidates.AddRange(target);
            }

            for (int i = 0; i < 3; i++)
            {
                var random = Random.Shared.NextInt64(0, candidates.Count - 1);
                var selection = candidates[(int)random];
                Selections.Add(new SkillCardReward()
                {
                    OwnerName = selection.OwnerName,
                    CardName = selection.CardName,
                    BeforeLevel = selection.Level,
                    AfterLevel = selection.Level + 1
                });
                candidates.RemoveAt((int)random);
            }
            CardRewardTickets.Remove(ticket);
        }

        public bool isActive()
        {
            return Selections.Count != 0;
        }
    }
}
