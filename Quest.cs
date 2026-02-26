// Custom weapon


public class Quest
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Weapon RewardWeapon;
    public bool IsCompleted;
    public Location NextQuest;

    public Quest(int id, string name, string description, Weapon rewardWeapon = null, Location nextQuest = null)
    {
        ID = id;
        Name = name;
        Description = description;
        RewardWeapon = rewardWeapon;
        NextQuest = nextQuest;
        IsCompleted = false;
    }


    Monster targetMonster = null;
    int id_1 = World.MONSTER_ID_RAT;
    int id_2 = World.MONSTER_ID_SNAKE;
    int id_3 = World.MONSTER_ID_GIANT_SPIDER;

    public bool CanStartQuest()
    {
        switch (ID)
        {
            case World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN:
                return true;
            case World.QUEST_ID_CLEAR_FARMERS_FIELD:
                Quest alchemistQuest = World.QuestByID(World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN);
                return alchemistQuest != null && alchemistQuest.IsCompleted;
            case World.QUEST_ID_COLLECT_SPIDER_SILK:
                Quest farmerQuest = World.QuestByID(World.QUEST_ID_CLEAR_FARMERS_FIELD);
                return farmerQuest != null && farmerQuest.IsCompleted;
            default:
                return true;
        }
    }

    public bool CompleteQuest(Player player, bool successfullyCompleted)
    {
        if (!successfullyCompleted)
        {
            return false;
        }

        IsCompleted = true;

        if (RewardWeapon != null)
        {
            Console.WriteLine($"You received: {RewardWeapon.Name}!");
            player.CurrentWeapon = RewardWeapon;
        }

        if (NextQuest != null)
        {
            Console.WriteLine("The next location has been revealed");
        }

        return true;
    }

    public void StartQuest(Player player)
    {
        Console.WriteLine($"Quest: {Name}");
        Console.WriteLine(Description);
        Console.WriteLine("Do you want to start this quest? (y/n)");

        string choice = Console.ReadLine().ToLower();

        if (choice == "y" || choice == "yes")
        {
            if (!CanStartQuest())
            {
                Console.WriteLine("You need to complete previous quests before starting this one.");
                return;
            }
            int quest_id = ID;
            Console.WriteLine($"Starting quest: {Name}");
            Console.WriteLine("You must defeat the enemy");

            // Quests
            switch (ID)
            {
                case 1:
                    NextQuest = World.LocationByID(World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN);
                    break;
                case 2:
                    NextQuest = World.LocationByID(World.QUEST_ID_CLEAR_FARMERS_FIELD);
                    break;
                case 3:
                    NextQuest = World.LocationByID(World.QUEST_ID_COLLECT_SPIDER_SILK);
                    break;
            }

            // Monsters by quest ID
            switch (ID)
            {
                case 1:
                    targetMonster = World.MonsterByID(World.MONSTER_ID_RAT);
                    break;
                case 2:
                    targetMonster = World.MonsterByID(World.MONSTER_ID_SNAKE);
                    break;
                case 3:
                    targetMonster = World.MonsterByID(World.MONSTER_ID_GIANT_SPIDER);
                    break;
            }



            if (targetMonster != null)
            {
                Console.WriteLine($"A wild {targetMonster.Name} appears!");

                while (targetMonster.CurrentHitPoints > 0 && player.CurrentHitPoints > 0)
                {
                    Console.WriteLine($"{targetMonster.Name} HP: {targetMonster.ReturnHealth()}");

                    Console.WriteLine("Press enter to attack");
                    Console.ReadLine();

                    // Player attacks monster (will take ferom player.cs)
                    targetMonster.TakeDamage(0, false);
                    Console.WriteLine($"You attacked {targetMonster.Name}");

                    if (targetMonster.CurrentHitPoints <= 0)
                    {
                        Console.WriteLine($"You defeated {targetMonster.Name}!");
                        break;
                    }

                    if (player.CurrentHitPoints > 0)
                    {
                        // Monster attacks player
                        double monsterDamage = targetMonster.MaximumDamage;
                        player.CurrentHitPoints -= (int)monsterDamage;
                        Console.WriteLine($"{targetMonster.Name} attacked you for {monsterDamage} damage");
                    }

                    if (player.CurrentHitPoints <= 0)
                    {
                        Console.WriteLine("You lost");
                        Console.WriteLine("Quest failed. Try again?");
                        string restart = Console.ReadLine().ToLower();
                        if (restart == "y" || restart == "yes")
                        {
                            player.CurrentHitPoints = player.MaximumHitPoints;
                            targetMonster.CurrentHitPoints = targetMonster.MaximumHitPoints;
                            Console.WriteLine("Restarting quest...");
                            return;
                        }
                    }
                }

                // Quest completed successfully
                bool questSuccess = CompleteQuest(player, true);
                if (questSuccess)
                {
                    Console.WriteLine($"Quest completed: {ID}");
                    
                    Weapon questReward = null;
                    switch (ID)
                    {
                        case World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN:
                        // Questreward
                            break;
                        case World.QUEST_ID_CLEAR_FARMERS_FIELD:
                        // Questreward
                            break;
                        case World.QUEST_ID_COLLECT_SPIDER_SILK:
                        //Questreward
                            break;
                    }
                    
                    if (questReward != null)
                    {
                        Console.WriteLine($"You received: {questReward.Name}!");
                        player.CurrentWeapon = questReward;
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Quest declined. You can try again later.");
        }
    }
}