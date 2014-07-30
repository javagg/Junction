using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	public class OdbcStorageInfo : OdbcInfoBase
	{
		[DefaultValue ("sa")]
		[Description ("User name logging in to ODBC database")]
		public string OdbcStoreUser { get; set; }

		[PasswordPropertyText (true)]
		[Description ("Users password")]
		[DefaultValue ("")]
		public string OdbcStorePassword { get; set; }

		[DefaultValue ("DATABASE=quickfix;DRIVER={SQL Server};SERVER=(local);")]
		[Description ("ODBC connection string for database")]
		public string OdbcStoreConnectionString { get; set; }

		public OdbcStorageInfo ()
		{
			this.OdbcStoreUser = "sa";
			this.OdbcStorePassword = "";
			this.OdbcStoreConnectionString = "DATABASE=quickfix;DRIVER={SQL Server};SERVER=(local);";
		}
	}
}
