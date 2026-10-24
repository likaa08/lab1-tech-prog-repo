namespace Lab3.Combat;

// участник боя в том виде, в котором он попадает в отчет
public sealed record Participant(string Code, string Name, int MaxHealth)
{
    public static Participant Of(ICombatant combatant)
    {
        return new Participant(combatant.Code, combatant.Name, combatant.MaxHealth);
    }
}