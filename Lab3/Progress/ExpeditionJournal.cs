using Lab3.Events;
using Lab3.World;
namespace Lab3.Progress;
public sealed class ExpeditionJournal : IGameEventListener {
    private readonly Dictionary<string, List<ExpeditionReport>> entries = new();
    public void OnEvent(GameEvent e) {
        if (!entries.TryGetValue(e.Hero.Name, out var list))
            entries[e.Hero.Name] = list = new();
        list.Add(e.Expedition);
    }
    public IReadOnlyList<ExpeditionReport> GetEntries(string name) => entries.TryGetValue(name, out var list)
        ? list.ToArray()
        : [];
}