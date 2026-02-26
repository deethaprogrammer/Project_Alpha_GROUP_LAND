using System.Data;
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
            Console.WriteLine((LocationToEast.Locked ? " 'x' " : " - ") + LocationToEast.LegendName);
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
            Console.WriteLine(LocationToWest.LegendName + (LocationToWest.Locked ? " 'x' " : " - "));
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
            Console.WriteLine("There is a Quest that you can do");
            return QuestAvailableHere;
        }
        else if (QuestAvailableHere != null && (QuestAvailableHere.IsCompleted || player.GetStartedQuest() != null))
        {
            Console.WriteLine(QuestAvailableHere.IsStarted ? "You are currently in a Quest." : "You have already finished this Quest.");
            return null;
        }
        return null;
    }

    public Location Move(Player player)
    {
        while (true)
        {
            PrintMap(player);
            QuestHere(player);
            Console.WriteLine("Press:\n[N]: To go Up (North)\n[E]: To go Right (East)\n[S]: To go Down (South)\n[W]: To go Left (West)\n[R]: Return I don't want to move.\nYou can use upper or lower to answer \n(The input will be recognized so you don't need to press enter after)");
            ConsoleKey key = Console.ReadKey(true).Key;

            Location? moving = key switch
            {
                ConsoleKey.N when LocationToNorth != null && !LocationToNorth.Locked => LocationToNorth,
                ConsoleKey.E when LocationToEast != null && !LocationToEast.Locked => LocationToEast,
                ConsoleKey.S when LocationToSouth != null && !LocationToSouth.Locked => LocationToSouth,
                ConsoleKey.W when LocationToWest != null && !LocationToWest.Locked => LocationToWest,
                ConsoleKey.R => this,
                _ => null
            };
            if (moving != null)
            {
                return moving;
            }
            if (player.GetStartedQuest() != null && MonsterLivingHere != null && player.GetStartedQuest().monsterType().ID == MonsterLivingHere.ID)
            {
                Console.Clear();
                player.GetStartedQuest().Battle(player);
                return this;
            }
            Console.Clear();
            Console.WriteLine("This is not a valid movement or the Location is Locked (You miss one or more Quest or you miss a key item)");
        }


    }
}