namespace Lab3.Combat;

// бой идет по раундам: сначала бьет герой, потом враг, пока кто-то не погибнет
public sealed class Battle
{
    private readonly ICombatant hero;
    private readonly ICombatant enemy;
    private readonly List<BattleEvent> events = new();
    private int round;

    public Battle(ICombatant hero, ICombatant enemy)
    {
        this.hero = hero;
        this.enemy = enemy;
    }

    public BattleReport Run()
    {
        while (hero.IsAlive && enemy.IsAlive)
        {
            round++;
            Strike(hero, Side.Hero, enemy, Side.Enemy);
            if (enemy.IsAlive)
            {
                Strike(enemy, Side.Enemy, hero, Side.Hero);
            }
        }

        Outcome outcome = hero.IsAlive ? Outcome.Victory : Outcome.Defeat;
        return new BattleReport(Participant.Of(hero), Participant.Of(enemy), outcome, events.ToList());
    }

    private void Strike(ICombatant attacker, Side source, ICombatant defender, Side target)
    {
        Hit hit = attacker.RollDamage();
        int dealt = defender.ReceiveHit(hit.Damage);
        EventType type = hit.Critical ? EventType.CriticalHit : EventType.Hit;
        Emit(new BattleEvent(round, type, source, target, dealt, defender.Health));

        if (!defender.IsAlive)
        {
            Emit(new BattleEvent(round, EventType.Death, source, target, 0, 0));
        }
    }

    private void Emit(BattleEvent battleEvent)
    {
        events.Add(battleEvent);
    }
}