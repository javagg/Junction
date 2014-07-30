using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	public class MySQLStorageInfo : MySQLInfoBase
	{
		[DefaultValue ("quickfix")]
		[Description ("Name of MySQL database to access for storing messages and session state")]
		public string MySQLStoreDatabase { get; set; }

		[DefaultValue ("root")]
		[Description ("User name logging in to MySQL database")]
		public string MySQLStoreUser { get; set; }

		[Description ("Users password")]
		[PasswordPropertyText (true)]
		[DefaultValue ("")]
		public string MySQLStorePassword { get; set; }

		[Description ("Address of MySQL database")]
		[DefaultValue ("localhost")]
		public string MySQLStoreHost { get; set; }

		[Description ("Port of MySQL database")]
		[DefaultValue (3306L)]
		public uint MySQLStorePort { get; set; }

		[DefaultValue (false)]
		[Description ("Use database connection pools. When possible, sessions will share a single database connection")]
		public bool MySQLStoreUseConnectionPool { get; set; }

		public MySQLStorageInfo ()
		{
			this.MySQLStoreDatabase = "quickfix";
			this.MySQLStoreUser = "root";
			this.MySQLStorePassword = "";
			this.MySQLStoreHost = "localhost";
			this.MySQLStorePort = 3306U;
			this.MySQLStoreUseConnectionPool = false;
		}
	}
}
