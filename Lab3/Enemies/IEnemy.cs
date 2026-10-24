using Lab3.Combat;

namespace Lab3.Enemies;

public interface IEnemy : ICombatant
{
    int ExperienceReward { get; }

    int RollGoldReward();
}