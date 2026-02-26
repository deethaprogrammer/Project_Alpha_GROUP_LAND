public class Player
{
    public string Name;
    public double CurrentHitPoints;
    public int CurrentMagicPoints;
    public const double MaximumHitPoints = 30.0;
    public const int MaximumMagicPoints = 150;
    public Weapon CurrentWeapon;
    public Location CurrentLocation;
    public Inventory Inventory;
    public readonly Random RNG = new();
    public Player(string name, Weapon currentWeapon, Location currentLocation)
    {
        Name = name;
        CurrentLocation = currentLocation;
        CurrentHitPoints = MaximumHitPoints;
        CurrentMagicPoints = 0;
        CurrentWeapon = currentWeapon;
        Inventory = new(this);
    }
    // Combat
    public void TakeDamage(double damage, Monster monster, bool isCritical = false)
    {
        damage = isCritical ? damage : damage * 1.5;
        int defenseRatio = RNG.Next(1, 6); // 1 = 100% damage, 5 = 60% damage
        double mitigation = (defenseRatio - 1) * 0.1;
        double damageTaken = damage * (1.0 - mitigation);
        double hpLeft = CurrentHitPoints - damageTaken;
        CurrentHitPoints = (hpLeft >= 0) ? hpLeft : 0;
        PrintStats();
        PrintCriticalHit(isCritical);
        Console.WriteLine($"{monster.Name} dealt {Math.Round(damageTaken, 1)} damage to {Name}!");
        if (PlayerDied()) { GameOver(); }
    }
    public void Defend()
    {
        int magicIncrease = RNG.Next(1, 11) * 5;
        CurrentMagicPoints += magicIncrease;
        Console.WriteLine($"{Name} defended themselves and gained {magicIncrease} MP!");
    }
    public bool IsCriticalHit()
    {
        bool isCritical = CurrentMagicPoints > 50;
        if (isCritical) { CurrentMagicPoints -= 50; }
        return isCritical;
    }
    public bool FleeFromBattle()
    {
        if (RNG.NextDouble() < 0.5) { Console.WriteLine($"{Name} failed to flee from the battle!"); return false; }
        else { Console.WriteLine($"{Name} successfully fleed from the battle!"); return true; }
    }
    public bool PlayerDied() => CurrentHitPoints <= 0;
    public void GameOver()
    {
        Console.WriteLine($"{Name} has no more HP to continue on.\nGame Over!\nWould you like to continue?");
        string? prompt = Console.ReadLine();
        if (string.IsNullOrEmpty(prompt)) { prompt = "No"; }
        if (prompt.ToUpper()[0] == 'Y') { ResetStats(); }
        else { Environment.Exit(0); }
    }
    public void Rest() => CurrentHitPoints = MaximumHitPoints;
    public void ResetStats()
    {
        CurrentLocation = World.LocationByID(1);
        CurrentHitPoints = MaximumHitPoints;
        CurrentMagicPoints = 0;
        Inventory.RemoveRandomItem(); // Remove random Weapon in Inventory when losing
    }
    // Print Info of Player Instance
    public void PrintCriticalHit(bool isCritical)
    {
        if (isCritical) { Console.WriteLine("Critical Hit!"); }
    }
    public void PrintStats()
    {
        List<string> stats = [
            $"Health: {CurrentHitPoints}/{MaximumHitPoints}",
            $"Magic Points: {CurrentMagicPoints}/{MaximumMagicPoints}",
            $"Current Weapon: {CurrentWeapon.Name}",
            $"Current Location: {CurrentLocation.Name}",
            ];
        for (int i = 0; i < stats.Count; i++)
        {
            string stat = stats[i];
            int newSpot = Console.WindowWidth - stat.Length;
            Console.SetCursorPosition(newSpot, i);
            Console.Write(stat);
        }
        Console.SetCursorPosition(0, Console.CursorTop + 1);
        Console.WriteLine();
    }
}