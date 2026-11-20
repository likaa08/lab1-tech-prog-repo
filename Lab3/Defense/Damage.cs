namespace Lab3.Defense;

public class Damage {

    private int amount;
    private bool evaded;
    private int absorbed;

    public Damage(int amount) {
        this.amount = amount;
    }

    public void ReduceTo(int amount) {
        this.amount = amount;
    }

    public void Evade() {
        amount = 0;
        evaded = true;
    }

    public void Absorb(int value) {
        amount -= value;
        absorbed += value;
    }

    public int Amount => amount;

    public bool IsEvaded => evaded;

    public int Absorbed => absorbed;
}
