using Lab3.Combat;
using Lab3.Defense;

namespace Lab3.Enemies;

public abstract class EnemyDecorator : IEnemy {

    private readonly IEnemy wrapped;

    protected EnemyDecorator(IEnemy wrapped) {
        this.wrapped = wrapped;
    }

    public virtual string Code => wrapped.Code;

    public virtual string Name => wrapped.Name;

    public virtual int Health => wrapped.Health;

    public virtual int MaxHealth => wrapped.MaxHealth;

    public virtual bool IsAlive => wrapped.IsAlive;

    public virtual Hit RollDamage() {
        return wrapped.RollDamage();
    }

    public virtual Damage ReceiveHit(int damage) {
        return wrapped.ReceiveHit(damage);
    }

    public virtual int ExperienceReward => wrapped.ExperienceReward;

    public virtual int RollGoldReward() {
        return wrapped.RollGoldReward();
    }
}