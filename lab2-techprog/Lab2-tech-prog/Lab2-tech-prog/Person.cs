namespace Lab2_tech_prog;

public class Person
{
    private string _name;
    private Pet _pet;
    
    public string Name => _name;
    public Pet Pet => _pet;

    public Person(string name)
    {
        _name = name;
        _pet = null;
    }

    public Person(string name, Pet pet)
    {
        _name = name;
        _pet = pet;
    }

    public void SetPet(Pet pet)
    {
        _pet = pet;
    }

    public void GetPetInfo()
    {
        Console.WriteLine("----------------------");
        if (_pet == null)
        {
            Console.WriteLine($"{_name} has no pet yet");
            Console.WriteLine("----------------------");
            return;
        }
        Console.WriteLine($"Pet for {_name} is named {_pet.GetName()}");
        Console.WriteLine($"Age: {_pet.GetAge()} year(s), about {_pet.GetAgeInHumanYears()} human years");
        Console.WriteLine("And it expresses itself like this:");
        _pet.Express();
        Console.WriteLine("----------------------");
    }
}