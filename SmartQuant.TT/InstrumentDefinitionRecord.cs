using SmartQuant;

namespace SmartQuant.TT
{
	class InstrumentDefinitionRecord
	{
		public InstrumentDefinitionRequest Request { get; private set; }

		public int Total { get; set; }

		public int Leaves { get; set; }

		public InstrumentDefinitionRecord (InstrumentDefinitionRequest request)
		{
			this.Request = request;
			this.Total = 0;
			this.Leaves = 0;
		}
	}
}
