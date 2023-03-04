namespace HTLMEngine.models;

public class Student
{
    public static int number = 0;
    
    public int orderNumber { get; set; }

    public int numberStud { get; set; }

    public string firstname { get; set; }
    
    public string surname { get; set; }
    
    public string patronymic { get; set; }

    public Student(int numberStud, string firstname, string surname, string patronymic)
    {
        orderNumber = number;
        number++;
        this.numberStud = numberStud;
        this.firstname = firstname;
        this.surname = surname;
        this.patronymic = patronymic;
    }
}