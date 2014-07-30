using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	public class PostgreSQLStorageInfo : PostgreSQLInfoBase
	{
		[Description ("Name of PostgreSQL database to access for storing messages and session state")]
		[DefaultValue ("quickfix")]
		public string PostgreSQLStoreDatabase { get; set; }

		[Description ("User name logging in to PostgreSQL database")]
		[DefaultValue ("postgres")]
		public string PostgreSQLStoreUser { get; set; }

		[Description ("Users password")]
		[DefaultValue ("")]
		[PasswordPropertyText (true)]
		public string PostgreSQLStorePassword { get; set; }

		[DefaultValue ("localhost")]
		[Description ("Address of PostgreSQL database")]
		public string PostgreSQLStoreHost { get; set; }

		[Description ("Port of PostgreSQL database")]
		[DefaultValue (5432L)]
		public uint PostgreSQLStorePort { get; set; }

		[Description ("Use database connection pools. When possible, sessions will share a single database connection")]
		[DefaultValue (false)]
		public bool PostgreSQLStoreUseConnectionPool { get; set; }

		public PostgreSQLStorageInfo ()
		{
			this.PostgreSQLStoreDatabase = "quickfix";
			this.PostgreSQLStoreUser = "postgres";
			this.PostgreSQLStorePassword = "";
			this.PostgreSQLStoreHost = "localhost";
			this.PostgreSQLStorePort = 5432U;
			this.PostgreSQLStoreUseConnectionPool = false;
		}
	}
}
