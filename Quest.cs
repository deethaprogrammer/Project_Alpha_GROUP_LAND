public class Quest
{
    public readonly int ID;
    public string Name;
    public string Description;
    public Weapon RewardWeapon;
    public bool IsCompleted;
    public bool IsStarted;
    public int KillsNeeded = 3;
    public int CurrentKills;
    public bool ClaimedReward;
    public Quest NextQuest;

    public Quest(int id, string name, string description, Weapon rewardWeapon = null, Quest nexquest = null)
    {
        ID = id;
        Name = name;
        Description = description;
        RewardWeapon = rewardWeapon;
        IsCompleted = false;
        NextQuest = nexquest;
    }

    public void CompleteQuest()
    {
        IsCompleted = true;
    }

    public Monster monsterType()
    {
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
        return targetMonster;
    }

    public string BattleOption()
    {
        do
        {
            string option = Console.ReadLine().Trim();
            string? answer = option switch
            {
                "1" => "attack",
                "2" => "boost",
                "3" => "defend",
                "4" => "flee",
                _ => null
            };

            if (answer != null)
            {
                return answer;
            }
            Console.WriteLine("You should really only press the corresponding key");

        } while (true);
    }

    public bool? DoActionplayer(Player player, Monster target)
    {
        bool playerCritical = false;
        do
        {
            string answer = BattleOption();
            if (answer == "attack")
            {
                target.TakeDamage(player.CurrentWeapon.MaximumDamage, playerCritical, player);
                if (target.CurrentHitPoints < 0) { target.CurrentHitPoints = 0; }
                return null;
            }
            else if (answer == "boost")
            {
                playerCritical = player.IsCriticalHit();
                if (!playerCritical) { Console.WriteLine("Not enough mp you need 50 mp"); }
            }
            else if (answer == "defend")
            {
                player.Defend();
                return null;
            }
            else if (answer == "flee")
            {
                if (player.CurrentHitPoints > target.CurrentHitPoints)
                {
                    return player.FleeFromBattle();
                }
                return null;
            }
        } while (true);
    }

    public void MonsterAction(Monster monster, Player target)
    {
        target.TakeDamage(monster.MaximumDamage, monster, monster.IsCritical());

    }

    public bool? Continue()
    {
        do
        {
            string continueOrNot = Console.ReadLine().ToLower();
            bool? Choice = continueOrNot switch
            {
                "y" => true,
                "n" => false,
                _ => null
            };
            if (Choice != null)
            {
                return Choice;
            }
            Console.WriteLine("that is not an option press y + enter or n + enter");
        } while (true);
    }

    public void Battle(Player player)
    {
        Monster target = monsterType();
        if (target != null)
        {
            Console.WriteLine($"A Wild {target.Name} has appeared");
            for (; CurrentKills < KillsNeeded; CurrentKills++)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine($"Monster: {target.Name} has {target.CurrentHitPoints}/{target.MaximumHitPoints} left.\n");
                    player.PrintStats();
                    Console.WriteLine("Only press the corresponding key\nWhat do you want to do?\n[1]Attack.\n[2]Boost Attack(Using MP)\n[3]Defend.\n[4]Flee.\n(Flee has a 50% chance to happen if player health is higher than the hit points of the enemy.)");
                    if (DoActionplayer(player, target) != null)
                    {
                        target.CurrentHitPoints = target.MaximumHitPoints;
                        return;
                    }
                    if (target.CurrentHitPoints != 0) { MonsterAction(target, player); }
                    if (player.CurrentHitPoints <= 0)
                    {
                        target.CurrentHitPoints = target.MaximumHitPoints;
                        return;
                    }

                } while (target.CurrentHitPoints > 0 && player.CurrentHitPoints > 0);
                if (CurrentKills < KillsNeeded - 1 && target.CurrentHitPoints == 0)
                {
                    Console.WriteLine("Do you want to continue fighting or will you be back later? y / n (press y or n after that press enter)");
                    if (Continue() != true)
                    {
                        return;
                    }
                }
                target.CurrentHitPoints = target.MaximumHitPoints;
            }
            if (CurrentKills == KillsNeeded)
            {
                CompleteQuest();
                IsStarted = false;
                CurrentKills = 0;
                Console.WriteLine("Two new messages:\n- A new area opened up\n- Return to the Quest giver in order to claim your reward\nPress enter if you have seen the message");
                Console.ReadLine();
                this.NextQuest.IsStarted = true;
                switch (ID)
                {
                    case 1:
                        World.LocationByID(World.LOCATION_ID_FARMHOUSE).Locked = false;
                        break;
                    case 2:
                        World.LocationByID(World.LOCATION_ID_BRIDGE).Locked = false;
                        break;
                }
            }
        }
    }


    public void receiveReward(Player player)
    {
        
    }
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

            Console.WriteLine($"Quest completed: {Name}");

            if (RewardWeapon != null)
            {
                Console.WriteLine($"You received: {RewardWeapon.Name}!");
                player.CurrentWeapon = RewardWeapon;
            }

            Console.WriteLine("New locations have been unlocked");
        }
    }
    public void WonGame() { }
}
