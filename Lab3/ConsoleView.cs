using Lab3.Combat;
using Lab3.Heroes;
using Lab3.Items;
using Lab3.World;

namespace Lab3;

// весь вывод в консоль собран здесь
public sealed class ConsoleView
{
    public string Describe(Item item)
    {
        string description = item.Name;

        if (item.Enchantments.Count > 0)
        {
            var names = new List<string>();
            foreach (Enchantment enchantment in item.Enchantments)
            {
                names.Add(enchantment.Name);
            }
            description += " [" + string.Join(", ", names) + "]";
        }

        return description + " (" + item.Description + ")";
    }

    public void PrintHero(Hero hero)
    {
        Console.WriteLine(
            $"{hero.Name}, {hero.HeroClass.DisplayName} {hero.Level} ур. | здоровье {hero.MaxHealth} | " +
            $"опыт {hero.Experience}/{hero.ExperienceToNextLevel} | золото {hero.Gold}");
        Console.WriteLine("  Оружие: " + Describe(hero.Weapon));
        Console.WriteLine("  Броня:  " + Describe(hero.Armor));
    }

    public void PrintReport(ExpeditionReport report, string locationName)
    {
        BattleReport battle = report.Battle;
        Console.WriteLine($"=== {locationName} (сид {report.Seed}) ===");
        Console.WriteLine($"{battle.Hero.Name} встречает врага: {battle.Enemy.Name} ({battle.Enemy.MaxHealth} здоровья)");

        foreach (BattleEvent battleEvent in battle.Events)
        {
            PrintEvent(battleEvent, battle);
        }

        PrintRewards(battle, report.Rewards);
    }

    private void PrintEvent(BattleEvent battleEvent, BattleReport battle)
    {
        Participant source = GetParticipant(battleEvent.Source, battle);
        Participant target = GetParticipant(battleEvent.Target, battle);

        switch (battleEvent.Type)
        {
            case EventType.Hit:
                Console.WriteLine(
                    $"[раунд {battleEvent.Round}] {source.Name} -> {target.Name}: {battleEvent.Amount} урона, " +
                    $"осталось {battleEvent.TargetHealthAfter}/{target.MaxHealth}");
                break;
            case EventType.CriticalHit:
                Console.WriteLine(
                    $"[раунд {battleEvent.Round}] {source.Name} -> {target.Name}: КРИТ! {battleEvent.Amount} урона, " +
                    $"осталось {battleEvent.TargetHealthAfter}/{target.MaxHealth}");
                break;
            case EventType.Death:
                Console.WriteLine($"{target.Name} повержен!");
                break;
        }
    }

    private void PrintRewards(BattleReport battle, Rewards rewards)
    {
        if (battle.Outcome == Outcome.Defeat)
        {
            Console.WriteLine($"Поражение... {battle.Hero.Name} едва уносит ноги и теряет {-rewards.Gold} золота");
            return;
        }

        Console.WriteLine($"Победа! +{rewards.Experience} опыта, +{rewards.Gold} золота");
        if (rewards.Loot is not null)
        {
            Console.WriteLine("Добыча: " + Describe(rewards.Loot));
        }
        if (rewards.LevelsGained > 0)
        {
            Console.WriteLine("Новый уровень!");
        }
    }

    private static Participant GetParticipant(Side side, BattleReport battle)
    {
        return side == Side.Hero ? battle.Hero : battle.Enemy;
    }
}