using System;

namespace SmartQuant.TT
{
	class ContractData
	{
		public string Symbol { get; set; }

		public string SecurityExchange { get; set; }

		public string SecurityID { get; set; }

		public Decimal PriceMultiplier { get; set; }

		public ContractData ()
		{
			this.Symbol = string.Empty;
			this.SecurityExchange = string.Empty;
			this.SecurityID = string.Empty;
			this.PriceMultiplier = new Decimal (10, 0, 0, false, 1);
		}
	}
}
