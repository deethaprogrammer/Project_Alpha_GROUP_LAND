

public class NonPlayableCharacters
{
    public static NonPlayableCharacters UnkownRepairMan = new(
        "Unkown repairman",
        1,
        "Sorry traveller but I need to work on the road, yesterday there was a big storm that destroyed the road to the farmer\n(You need to have 1 badge of bravery. So come back when you have 1 badge of bravery)",
        "Hello again traveller the road is almost repaired so you can pass give me 5 minutes\n '5 minutes later'\nThe road is finished have a safe journey."
        );
    public static NonPlayableCharacters Guard = new(
    "Guard",
    2,
    $"Sorry traveller but I need to see your badges of bravery.\nyou have currently {World.ProofBravery.Count} badges of bravery.\nI can't let you pass towards the forest the monsters living there can be very dangerous if your paths cross with one of them\nOfcourse there are also one that aren't that dangerous but for those that are dangerous.\nI will let you pass if you have 2 badges of bravery, So that I know that you can defend your self\n(So come back when you have 2 badges of bravery)",
    $"Sorry traveller but I need to see your badges of bravery.\nyou have currently {World.ProofBravery.Count} badges of bravery.\nYou may pass but be carefull the monsters can still hurt you so make sure you have the best equipment that you can get equiped."
    );
    public readonly string Name;
    public int NeededCount;
    public readonly string StoryLocked;
    public readonly string Story;

    public NonPlayableCharacters(string name, int neededcount, string storylocked, string story)
    {
        Name = name;
        NeededCount = neededcount;
        StoryLocked = storylocked;
        Story = story;
    }

    public bool PrintStory()
    {
        if (World.ProofBravery.Count < NeededCount)
        {
            Console.WriteLine(this.Name + ":\n" + StoryLocked);
            return true;
        }
        Console.WriteLine(this.Name + ":\n" + Story);
        return false;
    }
}
