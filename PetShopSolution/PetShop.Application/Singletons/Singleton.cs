namespace PetShop.Application.Singletons;

public class Singleton
{
    static Singleton? _instance { get; set; }
    public string src = "mongodb://mplopes:3702959@localhost:27017/";
    public static Singleton? Instance()
    {
        if (_instance == null)_instance = new Singleton();
        return _instance;
    }
}