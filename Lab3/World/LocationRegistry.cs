using Lab3.Core;

namespace Lab3.World;

// все локации игры
public sealed class LocationRegistry
{
    private readonly List<Location> locations;

    public LocationRegistry(IEnumerable<Location> locations)
    {
        this.locations = locations.ToList();
    }

    public IReadOnlyList<Location> All => locations.AsReadOnly();

    public Location Get(string code)
    {
        return locations.FirstOrDefault(location => location.Code == code)
               ?? throw new UnknownCodeException("Локация", code);
    }
}