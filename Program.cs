public static class Program
{
    static void Main(string[] args)
    {
        Monster monster = new();
        Console.WriteLine(monster);
        Location location = new();
        Console.WriteLine(location);
        Player player = new();
        Console.WriteLine(player);
        Quest quest = new();
        Console.WriteLine(quest);
    }
}