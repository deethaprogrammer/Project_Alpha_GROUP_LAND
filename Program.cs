using System.Runtime.InteropServices;

public static class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Start.\n\nWhat is your player name?");
        string PlayerName = Console.ReadLine();
        Console.Clear();
        Player player = new(PlayerName, World.Weapons[0], World.Locations[0]);
        player.PrintStats();
        player.CurrentLocation.PrintMap(player);
        Console.Clear();
        player.CurrentLocation.Move(player);
    }
}