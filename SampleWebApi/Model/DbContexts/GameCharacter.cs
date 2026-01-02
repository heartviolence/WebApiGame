namespace SampleWebApi.Model.DbContexts
{
    public class GameCharacter
    {
        public int Id { get; set; }
        public string CharacterID { get; set; }

        public int Level { get; set; } = 1;
        public int EXP { get; set; } = 0;

        public int A_Skill_Level { get; set; } = 1;
        public int B_Skill_Level { get; set; } = 1;

        public int StarLevel { get; set; } = 0;
    }   

}
