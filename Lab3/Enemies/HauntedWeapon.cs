using Lab3.Combat;
using Lab3.Core;
using Lab3.Defense;
using Lab3.Items;

namespace Lab3.Enemies;

public class HauntedWeapon : IEnemy {

    // наше одержимое оружие должно само придумать, сколько у него будет здоровья (у Weapon этой логики нет)
    // поэтому считаем, что за каждую единицу урона полагается 5 здоровья
    private const int HEALTH_PER_DAMAGE = 5;

    private readonly Weapon weapon;
    private readonly int maxHealth;
    private int health;

    public HauntedWeapon(Weapon weapon) {
        this.weapon = weapon;
        // проставляем здоровье по формуле
        this.maxHealth = weapon.Damage * HEALTH_PER_DAMAGE;
        this.health = maxHealth;
    }

    public string Code => "haunted-weapon";

    // название врага берется из названия оружия
    public string Name => "Одержимый " + weapon.Name;

    // метод нанесения урона полностью переиспользует логику оружия, тут основная ценность адаптера
    public Hit RollDamage() {
        return weapon.RollDamage();
    }

    // а вот тут ценность адаптера не такая явная, методы ниже мы реализуем сами почти с нуля
    public Damage ReceiveHit(int amount) {
        Damage damage = new Damage(amount);
        health = Math.Max(0, health - damage.Amount);
        return damage;
    }

    public bool IsAlive => health > 0;

    // награду за победу над одержимым оружием привяжем к его стоимости
    public int ExperienceReward => weapon.Price / 2;

    public int RollGoldReward() {
        return Dice.Instance.Roll(weapon.Price / 4, weapon.Price / 2);
    }

    public int Health => health;

    public int MaxHealth => maxHealth;
}