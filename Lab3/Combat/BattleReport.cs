namespace Lab3.Combat;

// результат боя: кто сражался, чем все закончилось и что происходило по ходу дела
public sealed record BattleReport(Participant Hero, Participant Enemy, Outcome Outcome, IReadOnlyList<BattleEvent> Events);