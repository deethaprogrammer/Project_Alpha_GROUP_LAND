public class Inventory
{
    public readonly List<int> Unequipped = [], Equipped = [];
    public Player Player;
    public Inventory(Player player) => Player = player;
    public bool ItemInInventory(Weapon item) => Unequipped.Contains(item.ID) || Equipped.Contains(item.ID);
    public void AddItemToInventory(Weapon item)
    {
        if (!ItemInInventory(item)) { Unequipped.Add(item.ID); }
    }
    public void RemoveItemFromInventory(Weapon item)
    {
        if (ItemInInventory(item)) { Unequipped.Remove(item.ID); }
    }
    public void EquipItem(Weapon item)
    {
        if (!ItemInInventory(item)) { return; }
        RemoveItemFromInventory(item);
        if (Equipped.Count != 0)
        {
            int itemID = Equipped[0];
            Weapon weapon = World.WeaponByID(itemID);
            UnequipItem(weapon);
        }
        Equipped.Add(item.ID);
        Player.CurrentWeapon = item;
    }
    public void UnequipItem(Weapon item)
    {
        if (!ItemInInventory(item)) { return; }
        Equipped.Remove(item.ID);
        AddItemToInventory(item);
        Player.CurrentWeapon = null;
    }
    public void equip_deEquip(List<int> inventory)
    {
        if (inventory == Unequipped)
        {
            Console.WriteLine("What do you want to equip (If you do Not want to equip anything press r + enter, else press the number of the equipment)");
            Console.WriteLine(ViewItemsInInventory("inventory"));
            string input = Console.ReadLine()!.ToLower();
            if (input == null)
            {
                Console.WriteLine("That is not an option");
            }
            else if (input == "r")
            {
                return;
            }
            else
            {
                Weapon weapon = World.WeaponByID(Convert.ToInt32(input));
                if (weapon == null)
                {
                    Console.WriteLine("Not a valid option!");
                }
                else
                {
                    EquipItem(weapon);
                }
            }
        }
        else if (inventory == Equipped)
        {
            Console.WriteLine("What do you want to unEquip (If you do Not want to unEquip anything press r + enter, else press the number of the equipment)");
            Console.WriteLine(ViewItemsInInventory("equipment"));
            string input = Console.ReadLine()!.ToLower();
            if (input == null)
            {
                Console.WriteLine("That is not an option");
            }
            else if (input == "r")
            {
                return;
            }
            else
            {
                Weapon weapon = World.WeaponByID(Convert.ToInt32(input));
                if (weapon == null)
                {
                    Console.WriteLine("Not a valid option!");
                }
                else
                {
                    UnequipItem(weapon);
                }
            }
        }
    }
    public string ViewItemsInInventory(string inventoryType)
    {
        List<int> inventory = inventoryType.ToLower() switch
        {
            "inventory" => Unequipped,
            "equipment" => Equipped,
            _ => throw new ArgumentException("Error: Invalid Inventory Type")
        };
        string start = $"Items in {inventoryType}:\n";
        foreach (int id in inventory)
        {
            start += $"{id}. {World.WeaponByID(id).Name}, Maximum Damage of the weapon: {World.WeaponByID(id).MaximumDamage} damage\n";
        }
        if (inventory.Count == 0) { start += "None"; }
        return start;
    }
    public void RemoveRandomItem()
    {
        int randomItemID = Unequipped[new Random().Next(Unequipped.Count) - 1];
        Weapon item = World.WeaponByID(randomItemID);
        RemoveItemFromInventory(item);
        Console.WriteLine($"You lost the {item} Weapon from your inventory!");
    }
}