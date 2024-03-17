using _2FA.Context;
namespace _2FA.Utilities
{
	public class CodeRemoval : BackgroundService
	{
		private static int codeLifetimeInMinutes { get; } = AppConfiguration.GetValue<int>("AppSettings:CodeLifetimeInMinutes");
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				RemoveExpiredCodes();
				await Task.Delay(60000, stoppingToken);
			}
		}
		public override Task StopAsync(CancellationToken cancellationToken)
		{
			RemoveExpiredCodes();
			return Task.CompletedTask;
		}
		private static void RemoveExpiredCodes()
		{
			using (DatabaseContext _dbContext = new())
			{
				_dbContext.Codes.RemoveRange(_dbContext.Codes.Where(c => c.GenerationTime < DateTime.Now.AddMinutes(codeLifetimeInMinutes) && c.UtilizationTime == null));
				_dbContext.SaveChanges();
			}
		}
	}
}
