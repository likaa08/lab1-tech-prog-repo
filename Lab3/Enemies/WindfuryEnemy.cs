using Lab3.Combat;

namespace Lab3.Enemies;

public class WindfuryEnemy : EnemyDecorator {

    private const int REWARD_BONUS_PERCENT = 50;

    public WindfuryEnemy(IEnemy wrapped) : base(wrapped) {}

    public override string Name => "Неистовый " + base.Name;

    public override Hit RollDamage() {
        Hit first = base.RollDamage();
        Hit second = base.RollDamage();
        return new Hit(first.Damage + second.Damage, first.Critical || second.Critical);
    }

    public override int ExperienceReward => WithBonus(base.ExperienceReward);

    public override int RollGoldReward() {
        return WithBonus(base.RollGoldReward());
    }

    private int WithBonus(int reward) {
        return reward + reward * REWARD_BONUS_PERCENT / 100;
    }
}