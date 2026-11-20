namespace Lab3.Defense;

public abstract class DamageHandler {

    private DamageHandler? next;

    public DamageHandler SetNext(DamageHandler next) {
        this.next = next;
        return next;
    }

    public abstract void Handle(Damage damage);

    protected void PassToNext(Damage damage) {
        if (next is not null) {
            next.Handle(damage);
        }
    }
}