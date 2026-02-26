public class Monster
{
    public readonly int ID;
    public string Name;
    public double MaximumDamage;
    public double CurrentHitPoints;
    public double MaximumHitPoints;
    public bool IsAlive = true;

    private Random random = new Random();

    public Monster(int id, string name, double maximumDamage, double currentHitPoints, double maximumHitPoints)
    {
        ID = id;
        Name = name;
        MaximumDamage = maximumDamage;
        CurrentHitPoints = currentHitPoints;
        MaximumHitPoints = maximumHitPoints;
    }

    public void TakeDamage(double damage, bool isCritical, Player player)
    {
        // 1 = 100% damage taken
        // 5 = 60% damage taken
        int defenseRatio = random.Next(1, 6);
        double mitigation = (defenseRatio - 1) * 0.1;
        double adjustedDamage = damage * (1.0 - mitigation);

        if (isCritical)
        {
            adjustedDamage = adjustedDamage * 1.5;
            this.CurrentHitPoints -= adjustedDamage;

        }
        else
        {
            this.CurrentHitPoints -= adjustedDamage;

        }
        if (CurrentHitPoints < 0) { CurrentHitPoints = 0; }
        Console.WriteLine($"{player.Name} dealt {Math.Round(adjustedDamage, 1)} damage to {Name}!\n{Name} HP: {CurrentHitPoints}/{MaximumHitPoints}");
    }



    // Optional: Weapon weapon for monsters using weapons.
    // weapon.maximumDamage is key
    public bool IsCritical()
    {
        bool isCritical = random.Next(2) == 0;
        return isCritical;
    }
}