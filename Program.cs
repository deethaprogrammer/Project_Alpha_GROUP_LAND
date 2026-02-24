public static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start.\n\nWhat is your player name?");
        string PlayerName = Console.ReadLine();
        Player player = new(PlayerName, 50, 50, World.Weapons[0], World.Locations[0]);
        player.PrintStats();
    }
}