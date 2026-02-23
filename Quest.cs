public class Quest
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Quest(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }
}