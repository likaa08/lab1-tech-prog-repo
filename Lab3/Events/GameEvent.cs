using Lab3.Heroes;
using Lab3.World;
namespace Lab3.Events;
public sealed record GameEvent(GameEventType Type, Hero Hero, ExpeditionReport Expedition);