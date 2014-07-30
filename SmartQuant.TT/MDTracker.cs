using SmartQuant;
using System;
using System.Collections.Generic;

namespace SmartQuant.TT
{
	class MDTracker
	{
		private SortedList<Price, int> bids;
		private SortedList<Price, int> asks;
		private List<Entry> newEntries;
		private List<Entry> changeEntries;
		private List<Entry> deleteEntries;
		private Decimal priceMultiplier;

		public MDTracker (Decimal priceMultiplier)
		{
			this.bids = new SortedList<Price, int> ((IComparer<Price>)new PriceComparer (PriceSortOrder.Descending));
			this.asks = new SortedList<Price, int> ((IComparer<Price>)new PriceComparer (PriceSortOrder.Ascending));
			this.newEntries = new List<Entry> ();
			this.changeEntries = new List<Entry> ();
			this.deleteEntries = new List<Entry> ();
			this.priceMultiplier = priceMultiplier;
		}

		public void Add (char action, char type, int position, Price price, int size)
		{
			switch (action) {
			case '0':
				this.newEntries.Add (new Entry (type, position, price * this.priceMultiplier, size));
				break;
			case '1':
				this.changeEntries.Add (new Entry (type, position, price * this.priceMultiplier, size));
				break;
			case '2':
				this.deleteEntries.Add (new Entry (type, position, price * this.priceMultiplier, size));
				break;
			}
		}

		public void BeginUpdate ()
		{
			this.newEntries.Clear ();
			this.changeEntries.Clear ();
			this.deleteEntries.Clear ();
		}

		public DataObject[] EndUpdate ()
		{
			List<DataObject> list1 = new List<DataObject> ();
			foreach (Entry entry in this.changeEntries) {
				Level2Side? nullable = new Level2Side? ();
				SortedList<Price, int> list2 = (SortedList<Price, int>)null;
				switch (entry.Type) {
				case '0':
					nullable = new Level2Side? (Level2Side.Bid);
					list2 = this.bids;
					break;
				case '1':
					nullable = new Level2Side? (Level2Side.Ask);
					list2 = this.asks;
					break;
				}
				if (list2 != null) {
					if (entry.Position >= list2.Count)
						throw new BadEntryException ('1', entry);
					Price index = list2.Keys [entry.Position];
					if ((double)entry.Price > 0.0 && (double)entry.Price != (double)index)
						throw new BadEntryException ('1', entry);
					list2 [index] = entry.Size;
					if (entry.Position == 0)
						list1.Add ((DataObject)this.GetBidOrAsk (nullable.Value, list2));
				}
			}
			this.deleteEntries.Sort ((Comparison<Entry>)((x, y) => y.Position.CompareTo (x.Position)));
			foreach (Entry entry in this.deleteEntries) {
				Level2Side? nullable = new Level2Side? ();
				SortedList<Price, int> list2 = (SortedList<Price, int>)null;
				switch (entry.Type) {
				case '0':
					nullable = new Level2Side? (Level2Side.Bid);
					list2 = this.bids;
					break;
				case '1':
					nullable = new Level2Side? (Level2Side.Ask);
					list2 = this.asks;
					break;
				}
				if (list2 != null) {
					if (entry.Position >= list2.Count)
						throw new BadEntryException ('2', entry);
					Price price = list2.Keys [entry.Position];
					if ((double)entry.Price > 0.0 && (double)entry.Price != (double)price)
						throw new BadEntryException ('2', entry);
					list2.RemoveAt (entry.Position);
					if (entry.Position == 0 && list2.Count > 0)
						list1.Add ((DataObject)this.GetBidOrAsk (nullable.Value, list2));
				}
			}
			foreach (Entry entry in this.newEntries) {
				Level2Side? nullable = new Level2Side? ();
				SortedList<Price, int> list2 = (SortedList<Price, int>)null;
				switch (entry.Type) {
				case '0':
					nullable = new Level2Side? (Level2Side.Bid);
					list2 = this.bids;
					break;
				case '1':
					nullable = new Level2Side? (Level2Side.Ask);
					list2 = this.asks;
					break;
				case '2':
					list1.Add ((DataObject)new Trade (DateTime.MinValue, (byte)0, 0, (double)entry.Price, entry.Size));
					break;
				}
				if (list2 != null) {
					if (list2.ContainsKey (entry.Price))
						throw new BadEntryException ('0', entry);
					list2.Add (entry.Price, entry.Size);
					if (list2.IndexOfKey (entry.Price) == 0)
						list1.Add ((DataObject)this.GetBidOrAsk (nullable.Value, list2));
				}
			}
			return list1.ToArray ();
		}

		private Tick GetBidOrAsk (Level2Side side, SortedList<Price, int> list)
		{
			switch (side) {
			case Level2Side.Bid:
				return new Bid (DateTime.MinValue, (byte)0, 0, (double)list.Keys [0], list.Values [0]);
			case Level2Side.Ask:
				return new Ask (DateTime.MinValue, (byte)0, 0, (double)list.Keys [0], list.Values [0]);
			default:
				return null;
			}
		}
	}
}
