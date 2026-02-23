public class Location
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Quest QuestAvailableHere;
    public Monster MonsterLivingHere;
    public Location LocationToNorth, LocationToEast, LocationToSouth, LocationToWest; 
    /* 
    ^^ Must be implemented as constant values ^^
    since it's not given in the constructor seen below
    */
    public Location(int id, string name, string description, Quest? questAvailableHere, Monster? monsterLivingHere)
    {
        ID = id;
        Name = name;
        Description = description;
        QuestAvailableHere = questAvailableHere;
        MonsterLivingHere = monsterLivingHere;
    }
}