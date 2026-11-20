using Lab3.Core;
namespace Lab3.Progress;
public sealed record Achievement(string Code, string DisplayName, string Description) {
    public static readonly Achievement FirstBlood =
        new("first-blood", "Первая кровь", "одержать первую победу");
    public static readonly Achievement Lucky = new("lucky", "Счастливчик", "найти зачарованный лут");
    public static readonly Achievement Veteran = new("veteran", "Ветеран", "достичь 5 уровня");
    public static readonly Achievement DragonSlayer =
        new("dragon-slayer", "Драконоборец", "победить дракона");
    public static readonly IReadOnlyList<Achievement> Values = [FirstBlood, Lucky, Veteran, DragonSlayer];
    public static Achievement ByCode(string code) =>
        Values.FirstOrDefault(a => a.Code == code) ?? throw new UnknownCodeException("Достижение", code);
}