public static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start.\n\nWhat is your player name?");
        string PlayerName = Console.ReadLine();
        //Player player = new(PlayerName, 50, 50, World.Weapons[0], World.Locations[0]);
        //player.PrintStats();

        Console.WriteLine(World.Locations);
        Monster test = new(20, "Test Subject", 10, 40, 40);
        test.TakeDamage(12.0, true);
        Console.WriteLine(test.ReturnHealth());
    }
}