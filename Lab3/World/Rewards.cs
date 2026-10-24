using Lab3.Items;

namespace Lab3.World;

// итоги экспедиции для героя;
// Gold отрицательный, если герой проиграл и потерял золото, Loot равен null, если добычи нет
public sealed record Rewards(int Experience, int Gold, Item? Loot, int LevelsGained);