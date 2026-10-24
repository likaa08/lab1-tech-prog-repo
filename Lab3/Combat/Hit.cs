namespace Lab3.Combat;

// удар: сколько урона и был ли он критическим
public readonly record struct Hit(int Damage, bool Critical);