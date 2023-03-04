namespace HTLMEngine.models;

public class Discipline
{
    public string name { get; set; }
    
    public List<Student> list { get; set; }

    public Discipline(string name, List<Student> list)
    {
        this.name = name;
        this.list = list;
    }
}