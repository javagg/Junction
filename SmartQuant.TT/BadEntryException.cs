using System;

namespace SmartQuant.TT
{
	class BadEntryException : Exception
	{
		public char Action { get; private set; }

		public Entry Entry { get; private set; }

		public BadEntryException (char action, Entry entry)
		{
			this.Action = action;
			this.Entry = entry;
		}
	}
}
