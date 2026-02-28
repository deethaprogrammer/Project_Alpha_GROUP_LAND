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

    public string BattleOption(bool Crit, Monster target, Player player)
    {
        do
        {
            Console.Clear();
            player.PrintStats();
            Console.WriteLine($"Monster: {target.Name} has {Math.Round(target.CurrentHitPoints, 3)}/{target.MaximumHitPoints} left.\n");
            if (Crit) { Console.WriteLine("you can attack with a critical\nPlease enter a new option"); }
            Console.WriteLine($"Only press the corresponding key\nWhat do you want to do?\n[1]Attack.\n[2]Boost Attack(Using MP)\n[3]Defend.\n[4]Heal with a potion you have: {World.HealingElixer.Count} Heal elixers.\n[5]Flee\n(Flee has a 75% chance to happen if player health is higher than the hit points of the enemy.)");

            string option = Console.ReadLine().Trim();
            string? answer = option switch
            {
                "1" => "attack",
                "2" => "boost",
                "3" => "defend",
                "4" => "heal",
                "5" => "flee",
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
            // Console.Clear();
            string answer = BattleOption(playerCritical, target, player);
            Console.Clear();
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
                World.ContinueMode();
            }
            else if (answer == "defend")
            {
                player.Defend();
                return null;
            }
            else if (answer == "heal")
            {
                if (World.HealingElixer.Count == 0)
                {
                    Console.WriteLine("You do not have any potions to use, Kill some mobs to have a chance to get it (30% on monster death)");
                    World.ContinueMode();
                }
                else if (World.HealingElixer.Count > 0)
                {
                    player.Inventory.RemoveNormalItem(World.HealingElixer);
                    double OldHitPoints = player.CurrentHitPoints;
                    player.CurrentHitPoints += 15;
                    if (player.CurrentHitPoints > Player.MaximumHitPoints)
                    {
                        player.CurrentHitPoints = Player.MaximumHitPoints;
                    }
                    Console.WriteLine($"Player used a Healing Elixer and has healed for: {Math.Round(player.CurrentHitPoints - OldHitPoints, 3)}HP");
                    return null;
                }

            }
            else if (answer == "flee")
            {
                if (player.CurrentHitPoints > target.CurrentHitPoints)
                {
                    if (player.FleeFromBattle())
                    {
                        player.CurrentLocation = player.FleeTOLocation();
                        return true;
                    }
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
            World.ContinueMode();
            for (; CurrentKills < KillsNeeded; CurrentKills++)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine($"Monster: {target.Name} has {Math.Round(target.CurrentHitPoints, 1)}/{target.MaximumHitPoints} HP left.\n");
                    player.PrintStats();
                    if (DoActionplayer(player, target) != null)
                    {
                        target.CurrentHitPoints = target.MaximumHitPoints;
                        return;
                    }
                    if (target.CurrentHitPoints != 0) { MonsterAction(target, player); World.ContinueMode(); }
                    if (player.PlayerDied())
                    {
                        target.CurrentHitPoints = target.MaximumHitPoints;
                        CurrentKills = 0;
                        player.GetStartedQuest().IsStarted = false;
                        player.GameOver();
                        return;
                    }

                } while (target.CurrentHitPoints > 0 && player.CurrentHitPoints > 0);
                if (target.CurrentHitPoints == 0)
                {
                    Random rng = new Random();
                    if (rng.Next(1, 11) <= 3)
                    {
                        Console.WriteLine($"{player.Name}, you have gotten an healing elixer (can be used in battle to heal for 15HP)");
                        player.Inventory.AddNormalItem(World.HealingElixer);

                    }
                }
                if (CurrentKills < KillsNeeded - 1 && target.CurrentHitPoints == 0)
                {
                    Console.WriteLine("Do you want to continue fighting or will you be back later? y / n (press y or n after that press enter)");
                    if (Continue() != true)
                    {
                        player.CurrentLocation = player.FleeTOLocation();
                        CurrentKills++;
                        target.CurrentHitPoints = target.MaximumHitPoints;
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
                if (this.NextQuest != null) { this.NextQuest.IsStarted = true; }
            }
        }
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
        }
    }
    public void WonGame()
    {

    }
}
