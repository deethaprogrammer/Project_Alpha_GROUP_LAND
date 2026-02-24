public class Monster
{
    public readonly int ID;
    public string Name;
    public double MaximumDamage;
    public double CurrentHitPoints;
    public double MaximumHitPoints;
    public Monster(int id, string name, double maximumDamage, double currentHitPoints, double maximumHitPoints)
    {
        ID = id;
        Name = name;
        MaximumDamage = maximumDamage;
        CurrentHitPoints = currentHitPoints;
        MaximumHitPoints = maximumHitPoints;
    }
    
    public void TakeDamage(double damage, bool critical)
    {
        if (critical)
        {
            this.CurrentHitPoints -= damage * 1.5;
        } 
        else
        {
            this.CurrentHitPoints -= damage;
        }
    }

    public string ReturnHealth()
    {
        return $"{CurrentHitPoints} / {MaximumHitPoints}";
    }

    
}