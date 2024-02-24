using ExpensesTracker.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddAppServices(builder.Configuration)
        .AddIdentityServices(builder.Configuration)
        .AddControllers();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

app.Run();
