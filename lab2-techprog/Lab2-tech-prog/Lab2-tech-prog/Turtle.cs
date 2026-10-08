namespace Lab2_tech_prog;

public class Turtle : Animal
{
    public Turtle(string name, int age) : base(name, age)
    {
        
    }

    public override void Express()
    {
        Console.WriteLine("""
                           ----
                          /    \  | o |
                          |     |/ __\
                          \_______/
                          |_| |_| |_|
                          """);
    }

    public override int GetAgeInHumanYears()
    {
        if (GetAge() <= 0)
        {
            return 0;
        }
        return 15 + (GetAge() - 1) * 2;
    }
}