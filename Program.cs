using _2FA.Context;
using _2FA.Filters;
using _2FA.Utilities;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
AppConfiguration.Initialize(builder.Configuration);
if (!AppConfiguration.GetValue<bool>("AppSettings:StoreCodeAfterExpiry"))
	builder.Services.AddHostedService<CodeRemoval>();
builder.Services.AddEndpointsApiExplorer()
				.AddDbContext<DatabaseContext>(options => options.UseSqlServer(AppConfiguration.GetValue<string>("ConnectionStrings:DbConnection")))
				.AddControllers(x => x.Filters.Add<UserAuthenticate>());
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	using (var salesContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
	{
		salesContext.Database.EnsureCreated();
	}
}
app.MapControllers();
app.Run();