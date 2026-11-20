using Lab3.Defense;

namespace Lab3.Enemies;

public class ArmoredEnemy : EnemyDecorator {

    public ArmoredEnemy(IEnemy wrapped) : base(wrapped) {}

    public override string Name => "Бронированный " + base.Name;

    public override Damage ReceiveHit(int amount) {
        if (amount < 0) {
            throw new ArgumentException("Урон не может быть отрицательным");
        }
        if (amount == 0) {
            return new Damage(0);
        }

        return base.ReceiveHit(amount - amount / 2);
    }
}