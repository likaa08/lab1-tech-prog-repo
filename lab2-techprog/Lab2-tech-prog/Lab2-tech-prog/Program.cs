namespace Lab2_tech_prog;

public class Program
{
    public static void Main(string[] args)
    {
        Parrot kesha = new Parrot("Kesha", 2);
        Turtle zavala = new Turtle("Zavala", 4);
        
        Person jack = new Person("Jack", kesha);
        Person john = new Person("John");
        
        jack.GetPetInfo();
        john.GetPetInfo();
        
        john.SetPet(zavala);
        john.GetPetInfo();
    }
}