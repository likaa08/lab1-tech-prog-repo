using Lab3.Combat;
namespace Lab3.Tactics;
public interface ITactic {
    string Code { get; }
    string DisplayName { get; }
    string Description { get; }
    Action ChooseAction(int round, ICombatant hero, ICombatant enemy);
}