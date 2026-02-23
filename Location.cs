public class Location
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Quest QuestAvailableHere;
    public Monster MonsterLivingHere;
    public Location LocationToNorth, LocationToEast, LocationToSouth, LocationToWest;
    public Location(int id, string name, string description, Quest questAvailableHere, Monster monsterLivingHere, ValueTuple<Location, Location, Location, Location> locations)
    {
        ID = id;
        Name = name;
        Description = description;
        QuestAvailableHere = questAvailableHere;
        MonsterLivingHere = monsterLivingHere;
        (LocationToNorth, LocationToEast, LocationToSouth, LocationToWest) = locations;
    }
}