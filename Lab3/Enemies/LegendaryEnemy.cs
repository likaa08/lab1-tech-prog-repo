using Lab3.Combat;

namespace Lab3.Enemies;

public class LegendaryEnemy : EnemyDecorator {

    private const int MULTIPLIER = 2;

    public LegendaryEnemy(IEnemy wrapped) : base(wrapped) {}

    // добавляем в имя оборачиваемого объекта префикс `Легендарный`
    public override string Name => "Легендарный " + base.Name;

    // удваиваем урон оборачиваемого объекта
    public override Hit RollDamage() {
        Hit hit = base.RollDamage();
        return new Hit(hit.Damage * MULTIPLIER, hit.Critical);
    }

    // удваиваем награду за легендарного врага
    public override int ExperienceReward => base.ExperienceReward * MULTIPLIER;

    public override int RollGoldReward() {
        return base.RollGoldReward() * MULTIPLIER;
    }
}