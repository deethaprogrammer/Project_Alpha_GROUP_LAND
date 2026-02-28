using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

public class Location
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Quest QuestAvailableHere;
    public Monster MonsterLivingHere;
    public Location LocationToNorth, LocationToEast, LocationToSouth, LocationToWest;

    public Player player;
    public bool Locked;


    public char LegendName;
    /* 
    ^^ Must be implemented as constant values ^^
    since it's not given in the constructor seen below
    */
    public Location(int id, string name, string description, Quest? questAvailableHere, Monster? monsterLivingHere, char legendname)
    {
        ID = id;
        Name = name;
        Description = description;
        QuestAvailableHere = questAvailableHere;
        MonsterLivingHere = monsterLivingHere;
        LegendName = legendname;
    }
    public void drawLegend()
    {
        Console.SetCursorPosition(0, 2);
        Console.WriteLine("legend:");
        Console.WriteLine("@: Player (you)");
        Console.WriteLine("H: Your House (start)");
        Console.WriteLine("T: Town Square");
        Console.WriteLine("F: Farm House");
        Console.WriteLine("V: Farmer's Field");
        Console.WriteLine("A: Alchemist's Hut");
        Console.WriteLine("P: Alchemist's Garden");
        Console.WriteLine("G: gaurd Post");
        Console.WriteLine("B: Bridge");
        Console.WriteLine("S: Spider Forest");
    }

    public void drawLocations()
    {
        int x = Console.WindowWidth / 2;
        Console.SetCursorPosition(x + 3, 3);
        Console.WriteLine("@");
        if (LocationToNorth != null)
        {
            Console.SetCursorPosition(x + 3, 1);
            Console.WriteLine(LocationToNorth.LegendName);

            Console.SetCursorPosition(x + 3, 2);
            Console.WriteLine(LocationToNorth.Locked ? "'x'" : "|");

        }

        if (LocationToEast != null)
        {
            Console.SetCursorPosition(x + 7, 3);
            Console.WriteLine((LocationToEast.Locked ? "'x'" : "-") + LocationToEast.LegendName);
        }

        if (LocationToSouth != null)
        {
            Console.SetCursorPosition(x + 3, 5);
            Console.WriteLine(LocationToSouth.LegendName);

            Console.SetCursorPosition(x + 3, 4);
            Console.WriteLine(LocationToSouth.Locked ? "'x'" : "|");
        }

        if (LocationToWest != null)
        {
            Console.SetCursorPosition(x - 2, 3);
            Console.WriteLine(LocationToWest.LegendName + (LocationToWest.Locked ? "'x'" : "-"));
        }

    }
    public void PrintMap(Player player)
    {
        player.PrintStats();
        drawLegend();
        drawLocations();
        Console.SetCursorPosition(0, 14);
    }

    public Quest QuestHere(Player player)
    {
        if (QuestAvailableHere != null && !QuestAvailableHere.IsCompleted && player.GetStartedQuest() == null)
        {
            Console.WriteLine("There is a Quest that you can do. (If you can move than press r to be able to take the quest.)");
            return QuestAvailableHere;
        }
        else if (QuestAvailableHere != null && QuestAvailableHere.IsCompleted && QuestAvailableHere.NextQuest.IsStarted)
        {
            Console.Clear();
            Console.WriteLine($"Thank you for completing my Quest Take your reward:\n'You will get a {QuestAvailableHere.NextQuest.RewardWeapon.Name} Dealing: {QuestAvailableHere.NextQuest.RewardWeapon.MaximumDamage} as max damage'\n'You will get a {World.ProofBravery.Name} (you need 2 bravery badges in order for the guard to let you pass)");
            player.Inventory.AddNormalItem(World.ProofBravery);
            QuestAvailableHere.NextQuest.IsStarted = false;
            QuestAvailableHere.NextQuest.IsCompleted = true;
            if (QuestAvailableHere.NextQuest != null) { player.Inventory.AddItemToInventory(QuestAvailableHere.NextQuest.RewardWeapon); }
            World.ContinueMode();
            Console.Clear();
        }
        else if (QuestAvailableHere != null && (QuestAvailableHere.IsCompleted || player.GetStartedQuest() != null))
        {
            Console.WriteLine(QuestAvailableHere.IsStarted ? "You are currently in a Quest." : "You have already finished this Quest.");
            return null;
        }
        return null;
    }

    public Location LocationLocked(Location movement)
    {
        if (movement.ID == World.LOCATION_ID_FARMHOUSE)
        {
            bool StillLocked = NonPlayableCharacters.UnkownRepairMan.PrintStory();
            World.ContinueMode();
            if (StillLocked)
            {
                return this;
            }
            movement.Locked = false;
            return movement;
        }
        else if (movement.ID == World.LOCATION_ID_BRIDGE)
        {
            bool StillLocked = NonPlayableCharacters.Guard.PrintStory();
            World.ContinueMode();
            if (StillLocked)
            {
                return this;
            }
            movement.Locked = false;
            return movement;
        }
        return movement;
    }
    public Location Move(Player player)
    {
        while (true)
        {
            QuestHere(player);
            PrintMap(player);
            if (player.GetStartedQuest() != null && MonsterLivingHere != null && player.GetStartedQuest()?.monsterType()?.ID == MonsterLivingHere.ID)
            {
                if (player.CurrentWeapon == null)
                {
                    Console.Clear();
                    Console.WriteLine("You can't start a battle without a weapon. Make sure you equip a weapon from your inventory");
                    World.ContinueMode();
                    Console.Clear();
                    return player.FleeTOLocation();
                }
                Console.Clear();
                player.GetStartedQuest().Battle(player);
                return player.CurrentLocation;
            }
            Console.WriteLine("Press:\n[N]: To go Up (North)\n[E]: To go Right (East)\n[S]: To go Down (South)\n[W]: To go Left (West)\n[R]: Return I don't want to move.\nYou can use upper or lower to answer \n(The input will be recognized so you don't need to press enter after)");
            ConsoleKey key = Console.ReadKey(true).Key;

            Location? moving = key switch
            {
                ConsoleKey.N when LocationToNorth != null => LocationToNorth,
                ConsoleKey.E when LocationToEast != null => LocationToEast,
                ConsoleKey.S when LocationToSouth != null => LocationToSouth,
                ConsoleKey.W when LocationToWest != null => LocationToWest,
                ConsoleKey.R => this,
                _ => null
            };
            if (moving != null)
            {
                if (!moving.Locked) { return moving; }
                Console.Clear();
                LocationLocked(moving);
                Console.Clear();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("This is not a valid key, pls chose one of the correct keys");
            }
        }


    }
}