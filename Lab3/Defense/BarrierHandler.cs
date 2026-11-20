namespace Lab3.Defense;

public class BarrierHandler : DamageHandler {

    private int strength;

    public BarrierHandler(int strength) {
        this.strength = strength;
    }

    public override void Handle(Damage damage) {
        int absorbed = Math.Min(strength, damage.Amount);
        if (absorbed > 0) {
            strength -= absorbed;
            damage.Absorb(absorbed);
        }

        PassToNext(damage);
    }
}