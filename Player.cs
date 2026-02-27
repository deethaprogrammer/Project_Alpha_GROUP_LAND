using System.Reflection.Metadata.Ecma335;

public class Player
{
    public string Name;
    public double CurrentHitPoints;
    public int CurrentMagicPoints;
    public const double MaximumHitPoints = 30.0;
    public const int MaximumMagicPoints = 150;
    public Weapon? CurrentWeapon;
    public Location CurrentLocation;
    public List<Quest> quests;
    public Inventory Inventory;
    public readonly Random RNG = new();
    public Player(string name, Location currentLocation, List<Quest> quest)
    {
        Name = name;
        CurrentLocation = currentLocation;
        CurrentHitPoints = MaximumHitPoints;
        CurrentMagicPoints = 0;
        quests = quest;
        Inventory = new(this);
    }
    // Combat
    public void TakeDamage(double damage, Monster monster, bool isCritical)
    {
        damage = isCritical ? damage : damage * 1.5;
        int defenseRatio = RNG.Next(1, 6); // 1 = 100% damage, 5 = 60% damage
        double mitigation = (defenseRatio - 1) * 0.1;
        double damageTaken = damage * (1.0 - mitigation);
        double hpLeft = CurrentHitPoints - damageTaken;
        CurrentHitPoints = (hpLeft >= 0) ? hpLeft : 0;
        PrintStats();
        PrintCriticalHit(isCritical);
        if (CurrentHitPoints < 0) { CurrentHitPoints = 0; }
        Console.WriteLine($"{monster.Name} dealt {Math.Round(damage, 1)} damage to {Name}!\n{Name} HP: {CurrentHitPoints}/{MaximumHitPoints}");

    }
    public void Defend()
    {
        int magicIncrease = RNG.Next(1, 11) * 5;
        if (CurrentMagicPoints <= MaximumMagicPoints)
        {
            CurrentMagicPoints += magicIncrease;
            if (CurrentMagicPoints > MaximumMagicPoints) { CurrentMagicPoints = MaximumMagicPoints; }
            Console.WriteLine($"{Name} defended themselves. MP: {CurrentMagicPoints}/{MaximumMagicPoints}");

        }

    }
    public bool IsCriticalHit()
    {
        bool isCritical = CurrentMagicPoints > 50;
        if (isCritical) { CurrentMagicPoints -= 50; }
        return isCritical;
    }
    public bool FleeFromBattle()
    {
        if (RNG.NextDouble() < 0.25) { Console.WriteLine($"{Name} failed to flee from the battle!"); return false; }
        else { Console.WriteLine($"{Name} successfully fleed from the battle!"); return true; }
    }
    public bool PlayerDied() => CurrentHitPoints <= 0;
    public void GameOver()
    {
        Console.Clear();
        Console.WriteLine($"{Name} has no more HP to continue on.\nGame Over!\nWould you like to continue? press (y/n if you do not press one of them it counts as no)");
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
    public Quest GetStartedQuest()
    {
        foreach (Quest quest in quests)
        {
            if (quest.IsStarted)
            {
                return quest;
            }
        }
        return null;
    }

    public Location FleeTOLocation()
    {
        foreach (Location locationQuest in World.Locations)
        {
            if (locationQuest.QuestAvailableHere != null && GetStartedQuest().ID == locationQuest.QuestAvailableHere.ID)
            {
                return locationQuest;
            }
        }
        return null;
    }
    public void PrintStats()
    {
        string currentWeaponName = (CurrentWeapon is null) ? "None" : CurrentWeapon.Name;
        List<string> stats = [
            $"Name: {Name}",
            $"Health: {Math.Round(CurrentHitPoints, 1)}/{MaximumHitPoints}",
            $"Magic Points: {CurrentMagicPoints}/{MaximumMagicPoints}",
            $"Current Weapon: {currentWeaponName}",
            $"Current Location: {CurrentLocation.Name}",
            $"Current Quest: {(GetStartedQuest() != null? GetStartedQuest().Description : "No active Quest")}",
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
    public void WonGame()
    {
        foreach (Quest quest in quests)
        {
            if (!quest.IsCompleted) { return; }
        }
        Console.Clear();
        Console.WriteLine("You have done it.\nYou saved the poor villagers");
        Console.WriteLine("\n\n\nThis game will close itself after you press enter");
        Console.ReadLine();
        Environment.Exit(0);
    }
}