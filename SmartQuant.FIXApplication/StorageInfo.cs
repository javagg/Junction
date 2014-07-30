using System.Collections.Generic;

namespace SmartQuant.FIXApplication
{
	public class StorageInfo : TypeInfo
	{
		public FileStorageInfo FILE { get; private set; }

		public MySQLStorageInfo MYSQL { get; private set; }

		public PostgreSQLStorageInfo POSTGRESQL { get; private set; }

		public OdbcStorageInfo ODBC { get; private set; }

		public StorageInfo ()
		{
			this.FILE = new FileStorageInfo ();
			this.MYSQL = new MySQLStorageInfo ();
			this.POSTGRESQL = new PostgreSQLStorageInfo ();
			this.ODBC = new OdbcStorageInfo ();
		}

		public override void GetValues (Dictionary<string, object> values)
		{
			base.GetValues (values);
			this.FILE.GetValues (values);
			this.MYSQL.GetValues (values);
			this.POSTGRESQL.GetValues (values);
			this.ODBC.GetValues (values);
		}

		public override void SetValue (Dictionary<string, object> values)
		{
			base.SetValue (values);
			this.FILE.SetValue (values);
			this.MYSQL.SetValue (values);
			this.POSTGRESQL.SetValue (values);
			this.ODBC.SetValue (values);
		}
	}
}
