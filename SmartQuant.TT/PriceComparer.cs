using System.Collections.Generic;

namespace SmartQuant.TT
{
	class PriceComparer : IComparer<Price>
	{
		private PriceSortOrder order;

		public PriceComparer (PriceSortOrder order)
		{
			this.order = order;
		}

		public int Compare (Price x, Price y)
		{
			switch (this.order) {
			case PriceSortOrder.Ascending:
				return x.CompareTo (y);
			case PriceSortOrder.Descending:
				return y.CompareTo (x);
			default:
				return 0;
			}
		}
	}
}
