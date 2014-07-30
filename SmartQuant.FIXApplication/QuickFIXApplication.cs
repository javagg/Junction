using QuickFix;
using SmartQuant;
using System;
using System.Collections.Generic;

namespace SmartQuant.FIXApplication
{
	public class QuickFIXApplication : MessageCracker, IApplication
	{
		private static volatile int nextMDReqID = 1;
		private static volatile int nextSecurityReqID = 1;
		private static volatile int nextClOrdID = 1;
		private QuickFIXProvider provider;
		private List<SessionID> sessionIDs;
		private Dictionary<SessionIDKey, SessionInfo> sessionInfoByID;

		protected QuickFIXApplication ()
		{
			this.sessionIDs = new List<SessionID> ();
			this.sessionInfoByID = new Dictionary<SessionIDKey, SessionInfo> ();
		}

		public virtual void FromAdmin (Message message, SessionID sessionID)
		{
		}

		public virtual void FromApp (Message message, SessionID sessionID)
		{
			this.Crack (message, sessionID);
		}

		public virtual void OnCreate (SessionID sessionID)
		{
		}

		public virtual void OnLogon (SessionID sessionID)
		{
			this.provider.OnSessionLogon ();
		}

		public virtual void OnLogout (SessionID sessionID)
		{
			this.provider.OnSessionLogout ();
		}

		public virtual void ToAdmin (Message message, SessionID sessionID)
		{
		}

		public virtual void ToApp (Message message, SessionID sessionID)
		{
		}

		internal void SetProvider (QuickFIXProvider provider)
		{
			this.provider = provider;
		}

		internal void ResetSessions ()
		{
			this.sessionIDs.Clear ();
			this.sessionInfoByID.Clear ();
		}

		internal void AddSession (SessionID sessionID, SessionInfo session)
		{
			this.sessionIDs.Add (sessionID);
			this.sessionInfoByID.Add ((SessionIDKey)sessionID, session);
		}

		protected SessionInfo GetSessionInfoByID (SessionID sessionID)
		{
			SessionInfo sessionInfo;
			if (this.sessionInfoByID.TryGetValue ((SessionIDKey)sessionID, out sessionInfo))
				return sessionInfo;
			else
				return (SessionInfo)null;
		}

		protected SessionID GetSessionIDByType (SessionType sessionType)
		{
			foreach (KeyValuePair<SessionIDKey, SessionInfo> keyValuePair in this.sessionInfoByID) {
				if (keyValuePair.Value.Type == sessionType)
					return (SessionID)keyValuePair.Key;
			}
			return (SessionID)null;
		}

		protected string GetNextMDReqID ()
		{
			lock (typeof (QuickFIXApplication))
				return string.Format ("{0:yyyyMMddHHmmss} {1}", (object)DateTime.Now, (object)QuickFIXApplication.nextMDReqID++);
		}

		protected string GetNextSecurityReqID ()
		{
			lock (typeof (QuickFIXApplication))
				return string.Format ("{0:yyyyMMddHHmmss} {1}", (object)DateTime.Now, (object)QuickFIXApplication.nextSecurityReqID++);
		}

		protected string GetNextClOrdID ()
		{
			lock (typeof (QuickFIXApplication))
				return string.Format ("{0:yyyyMMddHHmmss} {1}", (object)DateTime.Now, (object)QuickFIXApplication.nextClOrdID++);
		}

		protected void EmitError (string text)
		{
			this.provider.EmitError (text);
		}

		protected void EmitWarning (string text)
		{
			this.provider.EmitWarning (text);
		}

		protected void EmitMessage (string text)
		{
			this.provider.EmitMessage (text);
		}

		protected void EmitInstrumentDefinition (string requestId, Instrument[] instruments, int totalNum)
		{
			this.provider.EmitInstrumentDefinition (requestId, instruments, totalNum);
		}

		protected void EmitInstrumentDefinitionEnd (string requestId, RequestResult result, string text)
		{
			this.provider.EmitInstrumentDefinitionEnd (requestId, result, text);
		}

		protected void EmitTick (Instrument instrument, Tick tick)
		{
			this.provider.EmitTick (instrument, tick);
		}

		public virtual void Send (InstrumentDefinitionRequest request)
		{
			this.EmitInstrumentDefinitionEnd (request.Id, RequestResult.Error, "Cannot send instrument request - method is not implemented.");
		}

		public virtual void Subscribe (Instrument instrument)
		{
			this.EmitError ("Subcribe method is not implemented.");
		}

		public virtual void Unsubscribe (Instrument instrument)
		{
			this.EmitError ("Unsubscribe method is not implemented.");
		}
	}
}
