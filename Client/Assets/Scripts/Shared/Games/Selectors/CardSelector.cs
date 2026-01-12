using Assets.Scripts.Shared.Games.SkillCards;
using MessagePack;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shared.Games.Selectors
{
    [MessagePackObject]
    public class CardSelector
    {
        [Key(0)]
        public List<SkillCardRewardTicket> CardRewardTickets { get; set; } = new();
        [Key(1)]
        public List<SkillCardReward> Selections { get; set; } = new();
    }

    [MessagePackObject]
    public class SkillCardRewardTicket
    {
        [Key(0)]
        public int RewardRange { get; set; } 

        [Key(1)]
        public List<int> CardOwnerIndex { get; set; } = new();
    }

    [MessagePackObject]
    public class SkillCardReward
    {
        [Key(0)]
        public string OwnerName { get; set; }
        [Key(1)]
        public string CardName { get; set; }

        [Key(2)]
        public int BeforeLevel { get; set; }
        [Key(3)]
        public int AfterLevel { get; set; }
    }
}
