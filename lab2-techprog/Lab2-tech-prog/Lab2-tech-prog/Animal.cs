namespace Lab2_tech_prog;

public abstract class Animal : Pet
{
    private string _name;
    private int _age;
    
    public string Name => _name;
    public int Age => _age;

    public Animal(string name, int age)
    {
        _name = name;
        _age = age;
    }

    public void HappyBirthday()
    {
        _age++;
    }

    public abstract void Express();
    public abstract int GetAgeInHumanYears();

    public string GetName()
    {
        return _name;
    }

    public int GetAge()
    {
        return _age;
    }
}