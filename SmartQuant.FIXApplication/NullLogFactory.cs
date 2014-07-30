using QuickFix;

namespace SmartQuant.FIXApplication
{
	internal class NullLogFactory : ILogFactory
	{
		public ILog Create (SessionID sessionID)
		{
			return new NullLog ();
		}
	}
}
