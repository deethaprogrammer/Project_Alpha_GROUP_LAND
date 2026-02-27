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

// public static class WeaponSystem
// {
//     public static Weapon CurrentWeapon = new Weapon("None", 0);

//     public static void ChangeWeapon()
//     {
//         Console.WriteLine("CHOOSE YOUR WEAPON");
//         Console.WriteLine("A. SWORD (Damage 40)");
//         Console.WriteLine("B. BOW (Damage 25, Arrows 5)");
//         Console.WriteLine("C. SPEAR (Damage 30)");

//         string choice = Console.ReadLine() ?? "";

//         switch (choice.ToUpper())
//         {
//             case "A":
//                 CurrentWeapon = new Weapon(4, "Sword", 40);
//                 break;

//             case "B":
//                 CurrentWeapon = new Weapon(5, "Bow", 25);
//                 break;

//             case "C":
//                 CurrentWeapon = new Weapon(6, "Spear", 30);
//                 break;

//             default:
//                 Console.WriteLine("INVALID CHOICE");
//                 return;
//         }

//         Console.WriteLine(CurrentWeapon.Name + " EQUIPPED");
//     }
// }
