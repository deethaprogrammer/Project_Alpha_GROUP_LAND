using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main()
    {
        Console.Clear();
        Console.WriteLine("Hello Dear player, We would Like to know your name. So please enter a name.");
        string PlayerName = Console.ReadLine()!;
        if (PlayerName == null || PlayerName.Trim() == "") { PlayerName = "John"; }
        Player player = new(PlayerName, World.Locations[0], World.Quests);
        player.Inventory.AddItemToInventory(World.Weapons[0]);
        player.Inventory.EquipItem(World.Weapons[0]);
        Console.Clear();
        while (true)
        {
            player.CurrentLocation.PrintMap(player);
            Options(player);
        }
    }
    public static void Options(Player player)
    {
        Console.WriteLine("What do you want to do?");
        if (player.CurrentLocation.ID == World.LOCATION_ID_HOME)
        {
            Console.WriteLine("[z] Sleep in your bed to restore health");
        }
        Console.WriteLine("[m] Move to a different place.");
        Console.WriteLine("[i] Open your inventory.");
        if (player.CurrentLocation.QuestHere(player) != null)
        {
            Console.WriteLine("[q] Get quest.");
        }
        RunOptions(player);
    }
    public static void RunOptions(Player player)
    {
        do
        {
            string input = Console.ReadLine()!.ToLower();
            switch (input)
            {
                case "z" when player.CurrentLocation.ID == World.LOCATION_ID_HOME:
                    Console.Clear();
                    Console.WriteLine("You have slept and fully restored your HP.");
                    player.CurrentHitPoints = Player.MaximumHitPoints;
                    World.ContinueMode();
                    Console.Clear();
                    return;
                case "m":
                    Mover(player);
                    Console.Clear();
                    return;
                case "i":
                    InventoryMenu(player);
                    Console.Clear();
                    return;
                case "q" when player.CurrentLocation.QuestHere(player) != null:
                    Console.Clear();
                    player.CurrentLocation.QuestAvailableHere.StartQuest(player);
                    Console.Clear();
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Make sure you press the right key");
                    return;
            }
        } while (true);
    }
    public static void Mover(Player player)
    {
        do
        {
            Console.Clear();
            Location NewLocation = player.CurrentLocation.Move(player);
            if (NewLocation.ID == player.CurrentLocation.ID)
            {
                return;
            }
            player.CurrentLocation = NewLocation;

        } while (true);
    }
    public static void InventoryMenu(Player player)
    {
        Console.Clear();
        do
        {
            Console.WriteLine("[1] Open your inventory.\n[2] Open your equipment\n[3] close inventory menu.");
            string input = Console.ReadLine()!;
            switch (input)
            {
                case "1":
                    player.Inventory.equip_deEquip(player.Inventory.Unequipped);
                    return;
                case "2":
                    player.Inventory.equip_deEquip(player.Inventory.Equipped);
                    return;
                case "3":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("not an option");
                    break;
            }
        } while (true);
    }
}