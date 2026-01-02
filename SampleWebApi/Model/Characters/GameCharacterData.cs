namespace SampleWebApi.Model.Characters
{
    public class GameCharacterData
    {
        public string characterCode { get; set; }
        public GameCharacterType Type { get; set; }
        public List<GameCharacterAttribute> Attributes { get; set; } = new();
    }
}
