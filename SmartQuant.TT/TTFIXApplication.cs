using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX42;
using SmartQuant;
using SmartQuant.FIXApplication;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SmartQuant.TT
{
	class TTFIXApplication : QuickFIXApplication
	{
		private Dictionary<string, InstrumentDefinitionRecord> instrumentRecords;
		private Dictionary<string, MarketDataRecord> mdRecords;
		private Dictionary<Instrument, string> mdRequestIDs;
		private object mdLockObject;

		public TTFIXApplication ()
		{
			this.instrumentRecords = new Dictionary<string, InstrumentDefinitionRecord> ();
			this.mdRecords = new Dictionary<string, MarketDataRecord> ();
			this.mdRequestIDs = new Dictionary<Instrument, string> ();
			this.mdLockObject = new object ();
		}

		public override void FromAdmin (QuickFix.Message message, SessionID sessionID)
		{
			base.FromAdmin (message, sessionID);
			if (!(message is Logout))
				return;
			this.EmitWarning (message.ToString ());
		}

		public override void ToAdmin (QuickFix.Message message, SessionID sessionID)
		{
			base.ToAdmin (message, sessionID);
			if (!(message is Logon))
				return;
			TTSessionInfo ttSessionInfo = (TTSessionInfo)this.GetSessionInfoByID (sessionID);
			message.SetField ((IField)new ResetSeqNumFlag (ttSessionInfo.ResetSequence));
			message.SetField ((IField)new RawData (ttSessionInfo.Password));
		}

		public override void Send (InstrumentDefinitionRequest request)
		{
			SessionID sessionIdByType = this.GetSessionIDByType (SessionType.PRICE);
			if (sessionIdByType == null) {
				this.EmitInstrumentDefinitionEnd (request.Id, RequestResult.Error, "PRICE session is not defined.");
			} else {
				SecurityDefinitionRequest definitionRequest = new SecurityDefinitionRequest ();
				definitionRequest.Set (new SecurityReqID (request.Id));
				definitionRequest.Set (new SecurityRequestType (3));
				if (!string.IsNullOrEmpty (request.FilterSymbol))
					definitionRequest.Set (new Symbol (request.FilterSymbol));
				if (!string.IsNullOrEmpty (request.FilterExchange))
					definitionRequest.Set (new SecurityExchange (request.FilterExchange));
				string val = FIXTypeConverter.SecurityType.Convert (request.FilterType);
				if (val != null)
					definitionRequest.Set (new QuickFix.Fields.SecurityType (val));
				this.instrumentRecords.Add (request.Id, new InstrumentDefinitionRecord (request));
				Session.SendToTarget ((QuickFix.Message)definitionRequest, sessionIdByType);
			}
		}

		public override void Subscribe (Instrument instrument)
		{
			SessionID sessionIdByType = this.GetSessionIDByType (SessionType.PRICE);
			if (sessionIdByType == null) {
				this.EmitError ("Subscribe: PRICE sesion is not defined.");
			} else {
				MarketDataRequest marketDataRequest;
				lock (this.mdLockObject) {
					if (this.mdRequestIDs.ContainsKey (instrument)) {
						this.EmitWarning (string.Format ("Subscribe: Instrument {0} is already subscribed.", (object)instrument));
						return;
					} else {
						string local_2 = this.GetNextMDReqID ();
						marketDataRequest = new MarketDataRequest ();
						marketDataRequest.MDReqID = new MDReqID (local_2);
						marketDataRequest.SubscriptionRequestType = new SubscriptionRequestType ('1');
						marketDataRequest.MarketDepth = new MarketDepth (10);
						marketDataRequest.MDUpdateType = new MDUpdateType (1);
						marketDataRequest.AggregatedBook = new AggregatedBook (true);
						char[] temp_31 = new char[3] {
							'0',
							'1',
							'2'
						};
						foreach (char item_0 in temp_31)
							marketDataRequest.AddGroup ((QuickFix.Group)new MarketDataRequest.NoMDEntryTypesGroup () {
								MDEntryType = new MDEntryType (item_0)
							});
						ContractData local_5 = this.GetContract (instrument);
						marketDataRequest.AddGroup ((QuickFix.Group)new MarketDataRequest.NoRelatedSymGroup () {
							Symbol = new Symbol (local_5.Symbol),
							SecurityID = new SecurityID (local_5.SecurityID),
							SecurityExchange = new SecurityExchange (local_5.SecurityExchange)
						});
						this.mdRecords.Add (local_2, new MarketDataRecord (instrument, local_5));
						this.mdRequestIDs.Add (instrument, local_2);
					}
				}
				Session.SendToTarget ((QuickFix.Message)marketDataRequest, sessionIdByType);
			}
		}

		public override void Unsubscribe (Instrument instrument)
		{
			SessionID sessionIdByType = this.GetSessionIDByType (SessionType.PRICE);
			if (sessionIdByType == null) {
				this.EmitError ("Unsubscribe: PRICE sesion is not defined.");
			} else {
				MarketDataRecord marketDataRecord = (MarketDataRecord)null;
				string index;
				bool flag;
				lock (this.mdLockObject) {
					flag = this.mdRequestIDs.TryGetValue (instrument, out index);
					if (flag) {
						marketDataRecord = this.mdRecords [index];
						this.mdRequestIDs.Remove (instrument);
						this.mdRecords.Remove (index);
					}
				}
				if (flag) {
					MarketDataRequest marketDataRequest = new MarketDataRequest ();
					marketDataRequest.Set (new MDReqID (index));
					marketDataRequest.Set (new SubscriptionRequestType ('2'));
					MarketDataRequest.NoRelatedSymGroup noRelatedSymGroup = new MarketDataRequest.NoRelatedSymGroup ();
					noRelatedSymGroup.Set (new Symbol (marketDataRecord.Contract.Symbol));
					noRelatedSymGroup.Set (new SecurityID (marketDataRecord.Contract.SecurityID));
					noRelatedSymGroup.Set (new SecurityExchange (marketDataRecord.Contract.SecurityExchange));
					marketDataRequest.AddGroup ((QuickFix.Group)noRelatedSymGroup);
					Session.SendToTarget ((QuickFix.Message)marketDataRequest, sessionIdByType);
				} else
					this.EmitWarning (string.Format ("Unsubscribe: Instrument {0} is not subscribed.", (object)instrument));
			}
		}

		public void OnMessage (SecurityDefinition message, SessionID sessionID)
		{
			string str1 = message.SecurityReqID.Obj;
			InstrumentDefinitionRecord definitionRecord;
			if (!this.instrumentRecords.TryGetValue (str1, out definitionRecord))
				return;
			if (definitionRecord.Total == 0) {
				definitionRecord.Total = message.TotalNumSecurities.Obj;
				definitionRecord.Leaves = message.TotalNumSecurities.Obj;
			}
			--definitionRecord.Leaves;
			InstrumentType? nullable1 = FIXTypeConverter.SecurityType.Convert (message.SecurityType.Obj);
			if (nullable1.HasValue) {
				Instrument instrument = new Instrument (nullable1.Value, message.Symbol.Obj, "", (byte)148);
				instrument.Exchange = message.SecurityExchange.Obj;
				if (message.IsSetCurrency ())
					instrument.CurrencyId = CurrencyId.GetId (message.Currency.Obj);
				if (message.IsSetMaturityMonthYear ()) {
					string str2 = message.MaturityMonthYear.Obj;
					string str3 = message.IsSetMaturityDay () ? message.MaturityDay.Obj : "01";
					if (str3.Length == 1)
						str3 = "0" + str3;
					DateTime result;
					if (DateTime.TryParseExact (str2 + str3, "yyyyMMdd", (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
						instrument.Maturity = result;
				}
				if (message.IsSetPutOrCall ()) {
					PutCall? nullable2 = FIXTypeConverter.PutOrCall.Convert (new int? (message.PutOrCall.Obj));
					if (nullable2.HasValue)
						instrument.PutCall = nullable2.Value;
				}
				if (message.IsSetStrikePrice ())
					instrument.Strike = (double)message.StrikePrice.Obj;
				instrument.AltId.Add (new AltId ((byte)10, string.Format ("{0}|{1}|1.0", (object)message.Symbol.Obj, (object)message.SecurityID.Obj), string.Empty));
				this.EmitInstrumentDefinition (str1, new Instrument[1] {
					instrument
				}, definitionRecord.Total);
			} else
				--definitionRecord.Total;
			if (definitionRecord.Leaves != 0)
				return;
			this.instrumentRecords.Remove (str1);
			this.EmitInstrumentDefinitionEnd (str1, RequestResult.Completed, string.Format ("{0} instrument(s) found.", (object)definitionRecord.Total));
		}

		public void OnMessage (MarketDataRequestReject message, SessionID sessionID)
		{
			string key = message.MDReqID.getValue ();
			bool flag;
			lock (this.mdLockObject) {
				MarketDataRecord local_1;
				flag = this.mdRecords.TryGetValue (key, out local_1);
				if (flag) {
					this.mdRecords.Remove (key);
					this.mdRequestIDs.Remove (local_1.Instrument);
				}
			}
			if (flag)
				this.EmitError (message.ToString ());
			else
				this.EmitWarning (string.Format ("Unexpected message: {0}", (object)message));
		}

		public void OnMessage (MarketDataSnapshotFullRefresh message, SessionID sessionID)
		{
			string key = message.MDReqID.getValue ();
			MarketDataRecord marketDataRecord;
			bool flag;
			lock (this.mdLockObject)
				flag = this.mdRecords.TryGetValue (key, out marketDataRecord);
			if (flag) {
				marketDataRecord.Tracker.BeginUpdate ();
				for (int num = 1; num <= message.NoMDEntries.getValue (); ++num) {
					MarketDataSnapshotFullRefresh.NoMDEntriesGroup noMdEntriesGroup = new MarketDataSnapshotFullRefresh.NoMDEntriesGroup ();
					message.GetGroup (num, (QuickFix.Group)noMdEntriesGroup);
					marketDataRecord.Tracker.Add ('0', noMdEntriesGroup.MDEntryType.getValue (), -1, (Price)noMdEntriesGroup.MDEntryPx.getValue (), (int)noMdEntriesGroup.MDEntrySize.getValue ());
				}
				try {
					DataObject[] objects = marketDataRecord.Tracker.EndUpdate ();
					this.EmitDataObjects (marketDataRecord.Instrument, objects);
				} catch (BadEntryException ex) {
					this.EmitError (((object)ex).ToString ());
				}
			} else
				this.EmitWarning (string.Format ("Unexpected message: {0}", (object)message));
		}

		public void OnMessage (MarketDataIncrementalRefresh message, SessionID sessionID)
		{
			string key = message.MDReqID.getValue ();
			MarketDataRecord marketDataRecord;
			bool flag;
			lock (this.mdLockObject)
				flag = this.mdRecords.TryGetValue (key, out marketDataRecord);
			if (flag) {
				marketDataRecord.Tracker.BeginUpdate ();
				for (int num1 = 1; num1 <= message.NoMDEntries.getValue (); ++num1) {
					MarketDataIncrementalRefresh.NoMDEntriesGroup noMdEntriesGroup = new MarketDataIncrementalRefresh.NoMDEntriesGroup ();
					message.GetGroup (num1, (QuickFix.Group)noMdEntriesGroup);
					int position = noMdEntriesGroup.IsSetMDEntryPositionNo () ? noMdEntriesGroup.MDEntryPositionNo.getValue () - 1 : -1;
					Decimal num2 = noMdEntriesGroup.IsSetMDEntryPx () ? noMdEntriesGroup.MDEntryPx.getValue () : new Decimal (-1);
					int size = noMdEntriesGroup.IsSetMDEntrySize () ? (int)noMdEntriesGroup.MDEntrySize.getValue () : -1;
					marketDataRecord.Tracker.Add (noMdEntriesGroup.MDUpdateAction.getValue (), noMdEntriesGroup.MDEntryType.getValue (), position, (Price)num2, size);
				}
				try {
					DataObject[] objects = marketDataRecord.Tracker.EndUpdate ();
					this.EmitDataObjects (marketDataRecord.Instrument, objects);
				} catch (BadEntryException ex) {
					this.EmitError (((object)ex).ToString ());
				}
			} else
				this.EmitWarning (string.Format ("Unexpected message: {0}", (object)message));
		}

		private ContractData GetContract (Instrument instrument)
		{
			ContractData contractData = new ContractData ();
			string[] strArray = instrument.GetSymbol ((byte)10).Split ('|');
			contractData.Symbol = strArray.Length <= 0 || string.IsNullOrWhiteSpace (strArray [0]) ? instrument.Symbol : strArray [0].Trim ();
			contractData.SecurityExchange = instrument.GetExchange ((byte)10);
			contractData.SecurityID = strArray.Length <= 1 || string.IsNullOrWhiteSpace (strArray [1]) ? instrument.Symbol : strArray [1].Trim ();
			Decimal result;
			if (strArray.Length > 2 && Decimal.TryParse (strArray [2].Trim (), NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture, out result) && result != new Decimal (0))
				contractData.PriceMultiplier = result;
			return contractData;
		}

		private void EmitDataObjects (Instrument instrument, DataObject[] objects)
		{
			foreach (DataObject dataObject in objects) {
				if (dataObject is Tick)
					this.EmitTick (instrument, (Tick)dataObject);
			}
		}
	}
}
