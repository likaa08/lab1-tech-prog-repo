namespace Lab3.Defense;

public class ArmorHandler : DamageHandler {

    private readonly double defense;

    public ArmorHandler(double defense) {
        this.defense = defense;
    }

    public override void Handle(Damage damage) {
        int reduced = (int)Math.Round(damage.Amount * (1 - defense), MidpointRounding.AwayFromZero);
        damage.ReduceTo(Math.Max(1, reduced));
        PassToNext(damage);
    }
}