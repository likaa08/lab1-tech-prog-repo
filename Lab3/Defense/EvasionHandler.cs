using Lab3.Core;

namespace Lab3.Defense;

public class EvasionHandler : DamageHandler {

    private readonly double chance;

    public EvasionHandler(double chance) {
        this.chance = chance;
    }

    public override void Handle(Damage damage) {
        if (Dice.Instance.Chance(chance)) {
            damage.Evade();
            return;
        }

        PassToNext(damage);
    }
}