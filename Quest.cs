// Custom weapon

public class Quest
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Weapon RewardWeapon;
    public bool IsCompleted;
    public Location NextQuest;

    public Quest(
        int id,
        string name,
        string description,
        Weapon rewardWeapon = null,
        Location nextQuest = null
    )
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
        // Meet the quest person first
        MeetQuestGiver(player);

        Console.WriteLine($"Quest: {Name}");
        Console.WriteLine(Description);
        Console.WriteLine("Do you want to start this quest? (y/n)");

        string choice = Console.ReadLine()?.ToLower() ?? "n";

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

            // Quests - set next quest location
            switch (ID)
            {
                case World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN:
                    NextQuest = World.LocationByID(World.QUEST_ID_CLEAR_FARMERS_FIELD);
                    break;
                case World.QUEST_ID_CLEAR_FARMERS_FIELD:
                    NextQuest = World.LocationByID(World.QUEST_ID_COLLECT_SPIDER_SILK);
                    break;
                case World.QUEST_ID_COLLECT_SPIDER_SILK:
                    // This is the last quest, no next location
                    break;
            }

            // Monsters by quest ID
            switch (ID)
            {
                case World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN:
                    targetMonster = World.MonsterByID(World.MONSTER_ID_RAT);
                    break;
                case World.QUEST_ID_CLEAR_FARMERS_FIELD:
                    targetMonster = World.MonsterByID(World.MONSTER_ID_SNAKE);
                    break;
                case World.QUEST_ID_COLLECT_SPIDER_SILK:
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

                    // Player attacks monster
                    int playerDamage = player.CurrentWeapon.MaximumDamage;
                    targetMonster.TakeDamage(playerDamage, false);
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
                        Console.WriteLine(
                            $"{targetMonster.Name} attacked you for {monsterDamage} damage"
                        );
                    }

                    if (player.CurrentHitPoints <= 0)
                    {
                        Console.WriteLine("You lost");
                        Console.WriteLine("Quest failed. Try again?");
                        string restart = Console.ReadLine()?.ToLower() ?? "n";
                        if (restart == "y" || restart == "yes")
                        {
                            player.CurrentHitPoints = player.MaximumHitPoints;
                            targetMonster.CurrentHitPoints = targetMonster.MaximumHitPoints;
                            Console.WriteLine("Restarting quest...");
                            return;
                        }
                    }
                }

                // Return to quest giver before completing quest
                Console.WriteLine("You must return to the quest giver to report your success...");
                ReturnToQuestGiver(player);

                // Quest completed successfully
                bool questSuccess = CompleteQuest(player, true);
                if (questSuccess)
                {
                    Console.WriteLine($"Quest completed: {ID}");

                    Weapon questReward = null;
                    switch (ID)
                    {
                        case World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN:
                            questReward = new Weapon(World.WEAPON_ID_CLUB, "Club", 10);
                            break;
                        case World.QUEST_ID_CLEAR_FARMERS_FIELD:
                            // Weapon
                            break;
                        case World.QUEST_ID_COLLECT_SPIDER_SILK:
                            // Weapon
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

    private void MeetQuestGiver(Player player)
    {
        Console.WriteLine($"\n{player.Name}, you meet someone who needs your help...");

        switch (ID)
        {
            case World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN:
                Console.WriteLine("An old alchemist approaches you with a worried expression.");
                Console.WriteLine("'Ah, brave adventurer! My garden is overrun with rats!");
                Console.WriteLine(
                    "They're eating my rare herbs and ingredients. Please help me clear them out!'"
                );
                break;
            case World.QUEST_ID_CLEAR_FARMERS_FIELD:
                Console.WriteLine("A farmer runs up to you, looking distressed.");
                Console.WriteLine("'Thank goodness you're here! Snakes have invaded my field!");
                Console.WriteLine(
                    "My crops are being destroyed and I'm too afraid to go out there. Please help!'"
                );
                break;
            case World.QUEST_ID_COLLECT_SPIDER_SILK:
                Console.WriteLine("A mysterious cloaked figure emerges from the shadows.");
                Console.WriteLine(
                    "'I hear you're quite the warrior. I need spider silk from the forest..."
                );
                Console.WriteLine(
                    "The spiders there are dangerous, but their silk is invaluable for my work.'"
                );
                break;
        }
        Console.WriteLine();
    }

    private void ReturnToQuestGiver(Player player)
    {
        Console.WriteLine("\nYou return to report your success...");

        switch (ID)
        {
            case World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN:
                Console.WriteLine("The alchemist's face lights up with joy!");
                Console.WriteLine(
                    "'Wonderful! You've saved my garden! Those herbs are priceless to my research.'"
                );
                break;
            case World.QUEST_ID_CLEAR_FARMERS_FIELD:
                Console.WriteLine("The farmer embraces you gratefully!");
                Console.WriteLine(
                    "'Bless you, hero! My fields are safe again thanks to your bravery!'"
                );
                break;
            case World.QUEST_ID_COLLECT_SPIDER_SILK:
                Console.WriteLine("The cloaked figure nods approvingly.");
                Console.WriteLine(
                    "'Impressive. Few can face those spiders and live. Your skill is undeniable.'"
                );
                break;
        }
        Console.WriteLine();
    }
}
