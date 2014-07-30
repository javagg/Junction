namespace SmartQuant.FIXApplication
{
	public class PostgreSQLInfoBase : InfoBase
	{
		protected const string DEFAULT_DATABASE = "quickfix";
		protected const string DEFAULT_USER = "postgres";
		protected const string DEFAULT_PASSWORD = "";
		protected const string DEFAULT_HOST = "localhost";
		protected const uint DEFAULT_PORT = 5432U;
		protected const bool DEFAULT_USE_CONNECTION_POOL = false;

		protected PostgreSQLInfoBase ()
		{
		}
	}
}
