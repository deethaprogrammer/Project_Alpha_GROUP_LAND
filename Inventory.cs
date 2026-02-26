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
            start += $"{id}. {World.WeaponByID(id).Name}\n";
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