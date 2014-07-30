using SmartQuant;

namespace SmartQuant.TT
{
	class MarketDataRecord
	{
		public Instrument Instrument { get; private set; }

		public ContractData Contract { get; private set; }

		public MDTracker Tracker { get; private set; }

		public MarketDataRecord (Instrument instrument, ContractData contract)
		{
			this.Instrument = instrument;
			this.Contract = contract;
			this.Tracker = new MDTracker (contract.PriceMultiplier);
		}
	}
}
