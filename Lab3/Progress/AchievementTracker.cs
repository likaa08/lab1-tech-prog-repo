using Lab3.Events;
namespace Lab3.Progress;
public sealed class AchievementTracker : IGameEventListener {
    public void OnEvent(GameEvent e) {
        if (e.Type == GameEventType.Victory) {
            e.Hero.Unlock(Achievement.FirstBlood);
            if (e.Expedition.Battle.Enemy.Code == "dragon")
                e.Hero.Unlock(Achievement.DragonSlayer);
            if (e.Expedition.Rewards.Loot?.Enchantments.Count > 0)
                e.Hero.Unlock(Achievement.Lucky);
        } else if (e.Type == GameEventType.LevelUp && e.Hero.Level >= 5)
            e.Hero.Unlock(Achievement.Veteran);
    }
}