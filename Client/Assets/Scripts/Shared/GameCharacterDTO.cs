public class GameCharacterDTO
{
    public string Name { get; set; }

    public int Level { get; set; } = 1;
    public int EXP { get; set; } = 0;

    public int A_Skill_Level { get; set; } = 1;
    public int B_Skill_Level { get; set; } = 1;

    public int StarLevel { get; set; } = 0;
    public int Rank { get; set; } = 0;
}