public class Player
{
    public string Name;
    public double CurrentHitPoints = 30.0;
    public int CurrentMagicPoints = 150;
    public const double MaximumHitPoints = 30.0;
    public const int MaximumMagicPoints = 150;
    public Weapon CurrentWeapon;
    public Location CurrentLocation;
    public readonly Dictionary<string, List<int>> Inventory = new()
    {
        {"Inventory", []}, 
        {"Equipment", []}
    };
    public readonly Random RNG = new();
    private const string _line = "---------------------------------------------------------------------------------------------------";
    public Player(string name, Weapon currentWeapon)
    {
        Name = name;
        ResetStats();
        CurrentWeapon = currentWeapon;
        CurrentLocation = currentlocation;
    }
    // Location Management
    public void MoveToLocation(Location location) => CurrentLocation = location;
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
    // probably don't need this

    // public void DealDamageToMonster(double damage, Monster monster)
    // {
    //     bool isCritical = RNG.Next(2) == 0;
    //     monster.TakeDamage(damage, isCritical);
    //     PrintStats();
    //     PrintCriticalHit(isCritical);
    //     Console.WriteLine($"{monster.Name} dealt {(int) damage} damage to {this.Name}!");
    // }

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
        RemoveRandomItem(); // Remove random Weapon in Inventory when losing
    }
    // Inventory Management
    public bool ItemInInventory(Weapon findWeapon) => Inventory["Inventory"].Contains(findWeapon.ID);
    public void AddItemToInventory(Weapon newWeapon)
    {
        if (!ItemInInventory(newWeapon))
        { Inventory["Inventory"].Add(newWeapon.ID); }
        else
        { Console.WriteLine("Item is already in inventory."); }
    }
    public void MoveItem(Weapon item, bool equip = false) // Unequips by default
    {
        List<int> fromList = Inventory["Equipment"];
        List<int> toList = Inventory["Inventory"];
        if (equip)
        {
            fromList = Inventory["Inventory"];
            toList = Inventory["Equipment"];
        }
        fromList.Remove(item.ID);
        toList.Add(item.ID);
    }
    public void EquipWeapon(Weapon newWeapon)
    {
        MoveItem(newWeapon, true);
        CurrentWeapon = newWeapon;
    }
    public void UnequipWeapon(Weapon weapon)
    {
        MoveItem(weapon);
        CurrentWeapon = null;
    }
    public void RemoveRandomItem()
    {
        int randomItemID = Inventory["Inventory"][RNG.Next(Inventory["Inventory"].Count) - 1];
        Inventory["Inventory"].Remove(randomItemID);
        Console.WriteLine($"You lost the {World.WeaponByID(randomItemID)} Weapon from your inventory!");
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