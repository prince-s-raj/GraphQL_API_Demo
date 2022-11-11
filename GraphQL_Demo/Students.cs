namespace GraphQL_Demo;

using GraphQL;
using GraphQL.Types;
public record Student(int Id, string Name, int Age, string Address);
public class StudentDetail : ObjectGraphType<Student>
{
    public StudentDetail()
    {
        Field(x => x.Id);
        Field(x => x.Name);
        Field(x => x.Age);
        Field(x => x.Address);

    }
}
public class StudentQuery : ObjectGraphType
{
    public StudentQuery(IStudentProvider studentProvider)
    {
        Field<ListGraphType<StudentDetail>>(Name = "students",
            resolve: x =>studentProvider.GetStudents());
        Field<StudentDetail>(Name = "student",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            resolve: x => studentProvider.GetStudents().FirstOrDefault(s => s.Id == x.GetArgument<int>("id")));
    }
}

public class StudentSchema : Schema
{
    public StudentSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<StudentQuery>();
    }
}

public interface IStudentProvider
{
    Student[] GetStudents();
}
public class StudentProvider : IStudentProvider
{
    public Student[] GetStudents() => new[]
    {
        new Student(1, "Raj", 26, "Batticaloa"),
        new Student(1, "Kumara", 34, "Vavuniya"),
        new Student(1, "Anushika", 23, "Jaffna"),
        new Student(1, "Thanush", 41, "Colombo"),
        new Student(1, "Kishan", 33, "Ampara")

    };
}