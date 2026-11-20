using Lab3.Combat;

namespace Lab3.World;

// отчет об экспедиции: с каким сидом, куда, как прошел бой и что получил герой
public sealed record ExpeditionReport(int Seed, string LocationCode, string TacticCode, BattleReport Battle,
    Rewards Rewards);