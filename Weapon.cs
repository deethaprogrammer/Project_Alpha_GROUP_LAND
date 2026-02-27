using System;

public class Weapon
{
    public int ID;
    public string Name;
    public int MaximumDamage;
    public int Arrows;
    public int MaxArrows = 20;

    public Weapon(int id, string name, int damage)
    {
        ID = id;
        Name = name;
        MaximumDamage = damage;
    }

    public void Attack()
    {
        if (Arrows > 0)
        {
            Arrows--;
        }
    }

    public void Reload()
    {
        if (Name == "Bow")
        {
            Arrows = MaxArrows;
        }
    }
}

