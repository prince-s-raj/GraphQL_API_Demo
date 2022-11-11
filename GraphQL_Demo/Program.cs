using GraphQL_Demo;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Types;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStudentProvider, StudentProvider>();
builder.Services.AddSingleton<ISchema, StudentSchema>();
builder.Services.AddSingleton<StudentDetail>();
builder.Services.AddSingleton<StudentQuery>();
builder.Services.AddGraphQL(opt => opt.EnableMetrics = false).AddSystemTextJson();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapGet("/api/students", ([FromServices] IStudentProvider studentProvider) =>
{
    return studentProvider.GetStudents();
})
.WithName("GetStudents");


app.UseGraphQLAltair();
app.UseGraphQL<ISchema>();
app.Run();