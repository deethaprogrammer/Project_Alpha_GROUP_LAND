public class Monster
{
    public readonly int ID;
    public string Name;
    public double MaximumDamage;
    public double CurrentHitPoints;
    public double MaximumHitPoints;

    private Random random = new Random();

    public Monster(int id, string name, double maximumDamage, double currentHitPoints, double maximumHitPoints)
    {
        ID = id;
        Name = name;
        MaximumDamage = maximumDamage;
        CurrentHitPoints = currentHitPoints;
        MaximumHitPoints = maximumHitPoints;
    }
    
    public void TakeDamage(double damage, bool isCritical)
    {
        // 1 = 100% damage taken
        // 5 = 60% damage taken
        int defenseRatio = random.Next(1, 6);
        double mitigation = (defenseRatio - 1) * 0.1;
        double adjustedDamage = damage * (1.0 - mitigation);

        if (isCritical)
        {
            this.CurrentHitPoints -= adjustedDamage * 1.5;
            Console.WriteLine($"{Name} took {adjustedDamage * 1.5} damage! {ReturnHealth()}");
        } 
        else
        {
            this.CurrentHitPoints -= adjustedDamage;
            Console.WriteLine($"{Name} took {adjustedDamage} damage! {ReturnHealth()}");
        }
    }

    public string ReturnHealth()
    {
        return $"Current health: {CurrentHitPoints}/{MaximumHitPoints}";
    }

    // Optional: Weapon weapon for monsters using weapons.
    // weapon.maximumDamage is key
    public void DealDamageToPlayer(Player target)
    {
        bool isCritical = random.Next(2) == 0;
        target.TakeDamage(MaximumDamage, this, isCritical);
    }
}