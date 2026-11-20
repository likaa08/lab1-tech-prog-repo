using Lab3.Combat;
namespace Lab3.Tactics;
public sealed class AggressiveTactic : ITactic {
    public string Code => "aggressive";
    public string DisplayName => "Натиск";
    public string Description => "совершать безрассудные удары, пока у врага больше здоровья, чем у героя";
    public Action ChooseAction(int round, ICombatant hero,
        ICombatant enemy) => enemy.Health > hero.Health? Action.PowerStrike
        : Action.Attack;
}