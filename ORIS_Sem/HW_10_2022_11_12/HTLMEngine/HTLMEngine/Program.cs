
using HTLMEngine.models;
using HTMLEngineLibrary;

string template = "html/><head></head><body><h1>{Professor.discipline.name}{Professor.firstname} </h1></body></html>";


var student1 = new Student(1, "1firstname", "1surname", "1patronymic");
var student2 = new Student(2, "2firstname", "2surname", "2patronymic");
var student3 = new Student(3, "3firstname", "3surname", "3patronymic");
var student4 = new Student(4, "4firstname", "4surname", "4patronymic");
var list = new List<Student> { student1, student2, student3, student4 };

var discipline = new Discipline("ORIS", list);

var professor = new Professor("result", "firstname", "surname", "patronymic", discipline, 11106);

var someres = new EngineHTMLService().GetHTML(template, professor);

Console.Read();




//      TODO foreach in html(№ поряд, № студ Ф И О) (таблица)
//      TODO if in html >15 - группа переполнена
//      TODO if 10 - 15 - группа полная
//      TODO if 10< - группа неполнена
//      TODO список дисциплин который ведет преподаватель вообще (лист)
//      TODO 1) не variable, а object.object.Property
//      TODO foreach Array и List
//      TODO if else
//      TODO Readme.md как пользоваться foreach постановкой переменной
//      TODO   теги атрибут @ 
//      TODO проперти модель Model.Result, Model.Name

// {Property}
//  [if condition]: operations ; [else]: operations ;
// <Foreach > </Foreach>