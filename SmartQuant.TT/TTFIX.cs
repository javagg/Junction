using SmartQuant;
using SmartQuant.FIXApplication;
using System;

namespace SmartQuant.TT
{
	public class TTFIX : QuickFIXProvider, IInstrumentProvider, IDataProvider, IProvider
	{
		protected override Type SessionInfoType {
			get {
				return typeof(TTSessionInfo);
			}
		}

		public TTFIX (Framework framework) : base (framework, (QuickFIXApplication)new TTFIXApplication ())
		{
			this.id = 10;
			this.name = "TT FIX";
			this.description = "Trading Technologies FIX Adapter";
			this.url = "www.tradingtechnologies.com";
		}
	}
}
