using Lab3.Defense;
using Lab3.Tactics;
using Action = Lab3.Tactics.Action;
namespace Lab3.Combat;

// бой идет по раундам: сначала бьет герой, потом враг, пока кто-то не погибнет
public sealed class Battle {
    private readonly ICombatant hero;
    private readonly ICombatant enemy;
    private readonly List<BattleEvent> events = new();
    private int round;
    private readonly ITactic tactic;

    public Battle(ICombatant hero, ICombatant enemy, ITactic? tactic = null) {
        this.hero = hero;
        this.enemy = enemy;
        this.tactic = tactic ?? new BalancedTactic();
    }

    public BattleReport Run() {
        while (hero.IsAlive && enemy.IsAlive) {
            round++;
            Action action = tactic.ChooseAction(round, hero, enemy);
            if (action == Action.PowerStrike)
                Emit(new BattleEvent(round, EventType.PowerStrike, Side.Hero, Side.Enemy, 0, 0));
            else if (action == Action.Defend)
                Emit(new BattleEvent(round, EventType.Defend, Side.Hero, Side.Hero, 0, 0));
            Strike(hero, Side.Hero, enemy, Side.Enemy, action.AttackMultiplier);
            if (enemy.IsAlive) {
                Strike(enemy, Side.Enemy, hero, Side.Hero, action.IncomingMultiplier);
            }
        }

        Outcome outcome = hero.IsAlive ? Outcome.Victory : Outcome.Defeat;
        return new BattleReport(Participant.Of(hero), Participant.Of(enemy), outcome, events.ToList());
    }

    private void Strike(ICombatant attacker, Side source, ICombatant defender, Side target,
                        double multiplier) {
        Hit hit = attacker.RollDamage();
        Damage damage = defender.ReceiveHit(
            Math.Max(1, (int)Math.Round(hit.Damage * multiplier, MidpointRounding.AwayFromZero)));
        if (damage.IsEvaded) {
            Emit(new BattleEvent(round, EventType.Evade, source, target, 0, defender.Health));
            return;
        }
        if (damage.Absorbed > 0)
            Emit(new BattleEvent(round, EventType.Absorb, source, target, damage.Absorbed, defender.Health));
        if (damage.Amount > 0)
            Emit(new BattleEvent(round, hit.Critical ? EventType.CriticalHit : EventType.Hit, source, target,
                                 damage.Amount, defender.Health));

        if (!defender.IsAlive) {
            Emit(new BattleEvent(round, EventType.Death, source, target, 0, 0));
        }
    }

    private void Emit(BattleEvent battleEvent) {
        events.Add(battleEvent);
    }
}