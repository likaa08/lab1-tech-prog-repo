using Lab3.Combat;
namespace Lab3.Tactics;
public sealed class CautiousTactic : ITactic {
    public string Code => "cautious";
    public string DisplayName => "Осторожность";
    public string Description => "при здоровье ниже 40% через раунд уходить в оборону";
    public Action ChooseAction(int round, ICombatant hero,
        ICombatant enemy) => hero.Health < hero.MaxHealth * 0.4 && round % 2 == 0
        ? Action.Defend
        : Action.Attack;
}