using Lab3.Defense;
namespace Lab3.Combat;

// всё, что может участвовать в бою: и герой, и враги
public interface ICombatant {
    // код вида сущности: класс героя или тип врага
    string Code { get; }

    string Name { get; }

    int Health { get; }

    int MaxHealth { get; }

    bool IsAlive { get; }

    // удар, который будет нанесен данной сущностью
    Hit RollDamage();

    // получение удара, возвращает урон, который прошел на самом деле
    Damage ReceiveHit(int damage);
}