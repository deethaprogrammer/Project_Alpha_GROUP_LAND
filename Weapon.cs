using System;

public class Weapon
{
    public string Name;
    public int Damage;
    public int Arrows;
    public int MaxArrows;

    public Weapon(string name, int damage, int arrows = 0)
    {
        Name = name;
        Damage = damage;
        Arrows = arrows;
        MaxArrows = arrows;
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

public static class WeaponSystem
{
    public static Weapon CurrentWeapon = new Weapon("None", 0);

    public static void ChangeWeapon()
    {
        Console.WriteLine("CHOOSE YOUR WEAPON");
        Console.WriteLine("A. SWORD (Damage 40)");
        Console.WriteLine("B. BOW (Damage 25, Arrows 5)");
        Console.WriteLine("C. SPEAR (Damage 30)");

        string choice = Console.ReadLine() ?? "";

        switch (choice.ToUpper())
        {
            case "A":
                CurrentWeapon = new Weapon("Sword", 40);
                break;

            case "B":
                CurrentWeapon = new Weapon("Bow", 25, 5);
                break;

            case "C":
                CurrentWeapon = new Weapon("Spear", 30);
                break;

            default:
                Console.WriteLine("INVALID CHOICE");
                return;
        }

        Console.WriteLine(CurrentWeapon.Name + " EQUIPPED");
    }
}
