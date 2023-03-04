namespace HTLMEngine.models;

public class Professor
{
    public string result { get; set; }

    public string firstname { get; set; }

    public string surname { get; set; }

    public string patronymic { get; set; }

    public Discipline discipline { get; set; }

    public int numberGroup { get; set; }

    public Professor(string result, string firstname, string surname,
        string patronymic, Discipline discipline, int numberGroup)
    {
        this.result = result;
        this.firstname = firstname;
        this.surname = surname;
        this.patronymic = patronymic;
        this.discipline = discipline;
        this.numberGroup = numberGroup;
    }

}