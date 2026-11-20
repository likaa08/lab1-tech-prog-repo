using Lab3.Combat;
namespace Lab3.Tactics;
public sealed class BalancedTactic : ITactic {
    public string Code => "balanced";
    public string DisplayName => "Баланс";
    public string Description => "обычный удар каждый раунд";
    public Action ChooseAction(int round, ICombatant hero, ICombatant enemy) => Action.Attack;
}