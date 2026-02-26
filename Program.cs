using System.Runtime.InteropServices;

public static class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Start.\n\nWhat is your player name?");
        string PlayerName = Console.ReadLine();
        Console.Clear();
        Player player = new(PlayerName, World.Weapons[0], World.Locations[0], World.Quests);
        player.PrintStats();
        player.CurrentLocation.PrintMap(player);
        do
        {
            Console.Clear();
            int CurLocationId = player.CurrentLocation.ID;
            Location NewLocation = player.CurrentLocation.Move(player);
            if (NewLocation.ID == CurLocationId)
            {
                break;
            }
            player.CurrentLocation = NewLocation;
        } while (true);
        Console.Clear();
        player.CurrentLocation.PrintMap(player);
        if (player.CurrentLocation.QuestHere(player) != null)
        {
            player.CurrentLocation.QuestAvailableHere.StartQuest(player);
        }
        Console.Clear();
        player.CurrentLocation.PrintMap(player);
    }
}