namespace Lab2_tech_prog;

public class Parrot : Animal
{
    public Parrot(string name, int age) : base(name, age)
    {
        
    }

    public override void Express()
    {
        Console.WriteLine(GetName() + ": \"Hello!\"");
        Console.WriteLine("""
                          \
                          (o>
                          \\_//)
                           \_/_)
                            _|_
                          """);
    }

    public override int GetAgeInHumanYears()
    {
        return GetAge() * 5;
    }
}