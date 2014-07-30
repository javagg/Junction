namespace SmartQuant.TT
{
	class Entry
	{
		public char Type { get; private set; }

		public int Position { get; private set; }

		public Price Price { get; private set; }

		public int Size { get; private set; }

		public Entry (char type, int position, Price price, int size)
		{
			this.Type = type;
			this.Position = position;
			this.Price = price;
			this.Size = size;
		}

		public override string ToString ()
		{
			return string.Format ("Type={0} Position={1} Price={2} Size={3}", this.Type, this.Position, this.Price, this.Size);
		}
	}
}
