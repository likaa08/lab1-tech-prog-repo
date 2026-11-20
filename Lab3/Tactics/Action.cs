namespace Lab3.Tactics;
public sealed record Action(double AttackMultiplier, double IncomingMultiplier) {
    public static readonly Action Attack = new(1.0, 1.0);
    public static readonly Action PowerStrike = new(1.5, 1.5);
    public static readonly Action Defend = new(0.5, 0.3);
}