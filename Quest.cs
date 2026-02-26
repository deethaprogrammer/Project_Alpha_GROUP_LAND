// As a player I want to gain rewards when finishing a
// quest so that I can use
// these rewards later in the game
// When a quest is completed then the player
// receives better equipment
// The reward is added to the inventory of the player
// The reward can be equipped manually by the player,
// no automatic equipping.

public class Quest
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Weapon RewardWeapon;
    public bool IsCompleted;
    public bool IsStarted;

    public Quest(int id, string name, string description, Weapon rewardWeapon = null)
    {
        ID = id;
        Name = name;
        Description = description;
        RewardWeapon = rewardWeapon;
        IsCompleted = false;
    }

    public void CompleteQuest()
    {
        IsCompleted = true;
    }

    // When the player enters a location with a quest,
    // ask the user if he wants to start the quest or not
    // If the player does not want to start the quest,
    // then prompt the options once more for that area
    // If the player starts the quest then multiple
    // fight scenes will activate until the player is
    // successful
    // If the player finished the quest then that
    // quest will be marked as completed in their stats
    // When a quest is completed the player will be
    // notified if any new locations have been unlocked.

    public void StartQuest(Player player)
    {
        Console.WriteLine($"Quest: {Name}");
        Console.WriteLine(Description);
        Console.WriteLine("Do you want to start this quest? (y/n)");

        string choice = Console.ReadLine().ToLower();

        if (choice == "y" || choice == "yes")
        {
            Console.WriteLine($"Starting quest: {Name}");
            Console.WriteLine(Description);
            IsStarted = true;

            Monster targetMonster = null;
            int id_1 = World.MONSTER_ID_RAT;
            int id_2 = World.MONSTER_ID_SNAKE;
            int id_3 = World.MONSTER_ID_GIANT_SPIDER;

            switch (ID)
            {
                case 1:
                    targetMonster = World.MonsterByID(id_1);
                    break;
                case 2:
                    targetMonster = World.MonsterByID(id_2);
                    break;
                case 3:
                    targetMonster = World.MonsterByID(id_3);
                    break;
            }

            // if (targetMonster != null)
            // {
            //     Console.WriteLine($"A wild {targetMonster.Name} appears!");

            //     while (targetMonster.CurrentHitPoints > 0 && player.CurrentHitPoints > 0)
            //     {
            //         Console.WriteLine($"{targetMonster.Name} HP: {targetMonster.ReturnHealth()}");

            //         Console.WriteLine("Press enter to attack");
            //         Console.ReadLine();

            //         targetMonster.CurrentHitPoints -= 1;
            //         Console.WriteLine($"You attacked {targetMonster.Name}");

            //         if (targetMonster.CurrentHitPoints <= 0)
            //         {
            //             Console.WriteLine($"You defeated {targetMonster.Name}!");
            //             break;
            //         }

            //         if (player.CurrentHitPoints > 0)
            //         {
            //             player.CurrentHitPoints -= 1;
            //             Console.WriteLine($"{targetMonster.Name} attacked you");
            //         }

            //         if (player.CurrentHitPoints <= 0)
            //         {
            //             Console.WriteLine("You lost");
            //             Console.WriteLine("Quest failed. Try again?");
            //             string restart = Console.ReadLine().ToLower();
            //             if (restart == "y" || restart == "yes")
            //             {
            //                 targetMonster.CurrentHitPoints = targetMonster.MaximumHitPoints;
            //                 Console.WriteLine("Restarting quest...");
            //                 return;
            //             }
            //             else if (restart == "n" || restart == "no")
            //             {
            //                 IsStarted = false;
            //                 return;
            //             }
            //         }
            //     }

                // CompleteQuest();
                // IsStarted = false;
                Console.WriteLine($"Quest completed: {Name}");

                if (RewardWeapon != null)
                {
                    Console.WriteLine($"You received: {RewardWeapon.Name}!");
                    player.CurrentWeapon = RewardWeapon;
                }

                Console.WriteLine("New locations have been unlocked");
            }
        }
    //     else
    //     {
    //         Console.WriteLine("Quest declined. You can try again later.");
    //     }
    // }

    // Won game

    // To win the game the players needs to finish the quests
    public void WonGame() { }
}
