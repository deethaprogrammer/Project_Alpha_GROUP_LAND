public class Player
{
    public string Name;
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    public Weapon CurrentWeapon;
    public Location CurrentLocation;
    public Player(string name, int currentHitPoints, int maximumHitPoints, Weapon currentWeapon, Location currentLocation)
    {
        Name = name;
        CurrentHitPoints = currentHitPoints;
        MaximumHitPoints = maximumHitPoints;
        CurrentWeapon = currentWeapon;
        CurrentLocation = currentLocation;
    }
    public void PrintStats() // Use when starting a battle, since it clears console
    {
        List<string> stats = [
            $"Health: {CurrentHitPoints}/{MaximumHitPoints}",
            $"Current Weapon: {CurrentWeapon.Name}",
            $"Current Location: {CurrentLocation.Name}"
            ];
        Console.Clear();
        for (int i = 0; i < stats.Count; i++)
        {
            string stat = stats[i];
            int newSpot = Console.WindowWidth - stat.Length;
            Console.SetCursorPosition(newSpot, i);
            Console.Write(stat);
        }
        Console.SetCursorPosition(0, Console.CursorTop + stats.Count);
    }
}