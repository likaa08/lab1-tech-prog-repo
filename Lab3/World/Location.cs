using Lab3.Tactics;
using Lab3.Combat;
using Lab3.Core;
using Lab3.Enemies;
using Lab3.Heroes;
using Lab3.Items;

namespace Lab3.World;

public abstract class Location {
    private const double LootChance = 0.6;
    private const double EnchantChance = 0.3;

    protected readonly ItemCatalog catalog;

    protected Location(string code, string displayName, int recommendedLevel, ItemCatalog catalog) {
        Code = code;
        DisplayName = displayName;
        RecommendedLevel = recommendedLevel;
        this.catalog = catalog;
    }

    public string Code { get; }

    public string DisplayName { get; }

    public int RecommendedLevel { get; }

    // коды предметов из каталога, которые можно найти в локации
    protected abstract IReadOnlyList<string> LootTable { get; }

    // создание врага делегируется подклассам для повышения гибкости
    protected abstract IEnemy CreateEnemy();

    public ExpeditionReport Explore(Hero hero) {
        return Explore(hero, new BalancedTactic(), Dice.Instance.NextSeed());
    }

    // бизнес-логика, общая для всех локаций:
    // герой встречает врага, сражается с ним и получает награду, если победил
    public ExpeditionReport Explore(Hero hero, int seed) => Explore(hero, new BalancedTactic(), seed);
    public ExpeditionReport Explore(Hero hero, ITactic tactic) => Explore(hero, tactic,
                                                                          Dice.Instance.NextSeed());
    public ExpeditionReport Explore(Hero hero, ITactic tactic, int seed) {
        Dice dice = Dice.Instance;
        dice.Reseed(seed);

        hero.Rest();
        IEnemy enemy = CreateEnemy();
        if (dice.Chance(0.2))
            enemy = MakeElite(enemy, dice);
        BattleReport battle = new Battle(hero, enemy, tactic).Run();

        Rewards rewards = battle.Outcome == Outcome.Victory ? Reward(hero, enemy, dice) : Punish(hero);

        return new ExpeditionReport(seed, Code, tactic.Code, battle, rewards);
    }

    private Rewards Reward(Hero hero, IEnemy enemy, Dice dice) {
        int experience = enemy.ExperienceReward;
        int gold = enemy.RollGoldReward();
        Item? loot = RollLoot(dice);

        int levelsGained = hero.GainExperience(experience);
        hero.AddGold(gold);
        if (loot is not null) {
            hero.AddItem(loot);
        }

        return new Rewards(experience, gold, loot, levelsGained);
    }

    private Item? RollLoot(Dice dice) {
        if (!dice.Chance(LootChance)) {
            return null;
        }

        // добыча - это копия шаблона из каталога, ее можно менять, не боясь испортить шаблон
        Item loot = catalog.Create(dice.Pick(LootTable));
        if (dice.Chance(EnchantChance)) {
            loot.Enchant(dice.Pick(loot.PossibleEnchantments));
        }

        return loot;
    }

    // проигравший герой теряет половину золота
    private static Rewards Punish(Hero hero) {
        int lostGold = hero.Gold / 2;
        hero.SpendGold(lostGold);
        return new Rewards(0, -lostGold, null, 0);
    }
    private static IEnemy MakeElite(IEnemy enemy, Dice dice) {
        bool both = dice.Chance(0.25);
        if (dice.Chance(0.5)) {
            enemy = new LegendaryEnemy(enemy);
            if (both)
                enemy = new WindfuryEnemy(enemy);
        } else {
            enemy = new WindfuryEnemy(enemy);
            if (both)
                enemy = new LegendaryEnemy(enemy);
        }
        return dice.Chance(0.3) ? new ArmoredEnemy(enemy) : enemy;
    }
}