namespace _2FA.Utilities
{
	public static class AppConfiguration
	{
		private static IConfiguration _configuration;
		public static void Initialize(IConfiguration configuration) => _configuration = configuration;
		public static T GetValue<T>(string key) => _configuration.GetValue<T>(key);
	}
}
