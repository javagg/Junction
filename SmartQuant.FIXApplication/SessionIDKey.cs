using QuickFix;

namespace SmartQuant.FIXApplication
{
	class SessionIDKey
	{
		private SessionID sessionID;
		private string key;

		public SessionIDKey (SessionID sessionID)
		{
			this.sessionID = sessionID;
			this.key = sessionID.ToString ();
		}

		public static explicit operator SessionIDKey (SessionID sessionID)
		{
			return new SessionIDKey (sessionID);
		}

		public static implicit operator SessionID (SessionIDKey key)
		{
			return key.sessionID;
		}

		public override int GetHashCode ()
		{
			return this.key.GetHashCode ();
		}

		public override bool Equals (object obj)
		{
			if (obj is SessionIDKey)
				return this.key.Equals (((SessionIDKey)obj).key);
			else
				return base.Equals (obj);
		}
	}
}
