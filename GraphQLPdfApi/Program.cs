using GraphQL.AspNet.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQL();
builder.Services.AddControllers();

builder.Services.AddSingleton<PdfService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGraphQLAltair();
app.UseGraphQL();

app.Run();

