public static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start.\n\nWhat is your player name?");
        string PlayerName = Console.ReadLine();
        Player player = new(PlayerName, 50, 50, World.Weapons[0], World.Locations[0]);
        player.PrintStats();

        // ==============================================================================
        // Monster testing
        // ==============================================================================
        Monster test_subject = new(20, "Test Subject", 10.0, 100.0, 100.0);
        // test 1: taking damage & health stats - SUCCESS
        // test 2: taking damage with random defense between 0 and 40% - SUCCESS
        
        test_subject.TakeDamage(10, true);
        test_subject.TakeDamage(10, true);
        test_subject.TakeDamage(10, false);
        test_subject.TakeDamage(10, true);
        test_subject.TakeDamage(10, true);
        test_subject.TakeDamage(10, false);
        test_subject.TakeDamage(10, false);
        // ==============================================================================

    }
}