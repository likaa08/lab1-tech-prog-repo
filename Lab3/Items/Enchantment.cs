namespace Lab3.Items;

// зачарование неизменяемо (record с init-свойствами нельзя поменять после создания),
// поэтому одно и то же зачарование можно безопасно повесить на несколько предметов
public sealed record Enchantment(string Code, string Name, int BonusDamage, double BonusDefense,
    int BonusPrice) {
    public static readonly Enchantment Sharp = new("sharp", "Острое", 3, 0, 20);
    public static readonly Enchantment Fiery = new("fiery", "Огненное", 6, 0, 50);
    public static readonly Enchantment Sturdy = new("sturdy", "Прочное", 0, 0.05, 20);
    public static readonly Enchantment Warding = new("warding", "Охранное", 0, 0.10, 50);

    public static readonly IReadOnlyList<Enchantment> ForWeapons = [Sharp, Fiery];
    public static readonly IReadOnlyList<Enchantment> ForArmor = [Sturdy, Warding];
    public static Enchantment ByCode(string code) =>
        ForWeapons.Concat(ForArmor).FirstOrDefault(e => e.Code == code) ??
        throw new Lab3.Core.UnknownCodeException("Зачарование", code);
}