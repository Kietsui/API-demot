using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<QuestDb>(opt => opt.UseInMemoryDatabase("QuestList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/Questitems", async (QuestDb db) =>
    await db.Quests.ToListAsync());

app.MapGet("/Questitems/complete", async (QuestDb db) =>
    await db.Quests.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/Questitems/{id}", async (int id, QuestDb db) =>
    await db.Quests.FindAsync(id)
        is Quest Quest
            ? Results.Ok(Quest)
            : Results.NotFound());

app.MapPost("/Questitems", async (Quest Quest, QuestDb db) =>
{
    db.Quests.Add(Quest);
    await db.SaveChangesAsync();

    return Results.Created($"/Questitems/{Quest.Id}", Quest);
});

app.MapPut("/Questitems/{id}", async (int id, Quest inputQuest, QuestDb db) =>
{
    var Quest = await db.Quests.FindAsync(id);

    if (Quest is null) return Results.NotFound();

    Quest.Name = inputQuest.Name;
    Quest.IsComplete = inputQuest.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/Questitems/{id}", async (int id, QuestDb db) =>
{
    if (await db.Quests.FindAsync(id) is Quest Quest)
    {
        db.Quests.Remove(Quest);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();