using QuickFix;
using QuickFix.Transport;
using SmartQuant;
using SmartQuant.FIXApplication.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;

namespace SmartQuant.FIXApplication
{
  public class QuickFIXProvider : Provider
  {
    protected const string CATEGORY_SESSIONS_DEFAULTS = "QuickFIX - Sessions (defaults)";
    protected const string CATEGORY_SESSIONS = "QuickFIX - Sessions";
    protected const string CATEGORY_STORAGE = "QuickFIX - Storage";
    protected const string CATEGORY_LOGGING = "QuickFIX - Logging";
    protected const string CATEGORY_INITIATOR = "QuickFIX - Initiator";
    protected const string DEFAULT_BEGIN_STRING = "FIX.4.2";
    protected const string DEFAULT_START_TIME = "00:00:00";
    protected const string DEFAULT_END_TIME = "00:00:00";
    protected const bool DEFAULT_SEND_REDUNDANT_RESEND_REQUESTS = false;
    protected const bool DEFAULT_RESET_ON_LOGOUT = false;
    protected const bool DEFAULT_RESET_ON_DISCONNECT = false;
    protected const bool DEFAULT_REFRESH_ON_LOGON = false;
    protected const string DEFAULT_DATA_DICTIONARY = "FIX42.xml";
    protected const bool DEFAULT_CHECK_LATENCY = true;
    protected const string DEFAULT_MAX_LATENCY = "00:02:00";
    protected const string DEFAULT_RECONNECT_INTERVAL = "00:00:30";
    protected const string DEFAULT_HEART_BT_INT = "00:00:20";
    protected const InitiatorMode DEFAULT_INITIATOR_MODE = InitiatorMode.Single;
    protected const bool DEFAULT_PERSIST_MESSAGES = false;
    protected const bool DEFAULT_IS_LOGGING_ENABLED = false;
    protected const string KEY_CONNECTION_TYPE = "ConnectionType";
    protected const string KEY_START_TIME = "StartTime";
    protected const string KEY_END_TIME = "EndTime";
    protected const string KEY_SEND_REDUNDANT_RESEND_REQUESTS = "SendRedundantResendRequests";
    protected const string KEY_RESET_ON_LOGOUT = "ResetOnLogout";
    protected const string KEY_RESET_ON_DISCONNECT = "ResetOnDisconnect";
    protected const string KEY_REFRESH_ON_LOGON = "RefreshOnLogon";
    protected const string KEY_DATA_DICTIONARY = "DataDictionary";
    protected const string KEY_CHECK_LATENCY = "CheckLatency";
    protected const string KEY_MAX_LATENCY = "MaxLatency";
    protected const string KEY_RECONNECT_INTERVAL = "ReconnectInterval";
    protected const string KEY_HEART_BT_INT = "HeartBtInt";
    protected const string KEY_SOCKET_CONNECT_PORT = "SocketConnectPort";
    protected const string KEY_SOCKET_CONNECT_HOST = "SocketConnectHost";
    protected const string KEY_PERSIST_MESSAGES = "PersistMessages";
    protected const string KEY_FILE_STORE_PATH = "FileStorePath";
    protected const string KEY_MYSQL_STORE_DATABASE = "MySQLStoreDatabase";
    protected const string KEY_MYSQL_STORE_USER = "MySQLStoreUser";
    protected const string KEY_MYSQL_STORE_PASSWORD = "MySQLStorePassword";
    protected const string KEY_MYSQL_STORE_HOST = "MySQLStoreHost";
    protected const string KEY_MYSQL_STORE_PORT = "MySQLStorePort";
    protected const string KEY_MYSQL_STORE_USE_CONNECTION_POOL = "MySQLStoreUseConnectionPool";
    protected const string KEY_POSTGRESQL_STORE_DATABASE = "PostgreSQLStoreDatabase";
    protected const string KEY_POSTGRESQL_STORE_USER = "PostgreSQLStoreUser";
    protected const string KEY_POSTGRESQL_STORE_PASSWORD = "PostgreSQLStorePassword";
    protected const string KEY_POSTGRESQL_STORE_HOST = "PostgreSQLStoreHost";
    protected const string KEY_POSTGRESQL_STORE_PORT = "PostgreSQLStorePort";
    protected const string KEY_POSTGRESQL_STORE_USE_CONNECTION_POOL = "PostgreSQLStoreUseConnectionPool";
    protected const string KEY_ODBC_STORE_USER = "OdbcStoreUser";
    protected const string KEY_ODBC_STORE_PASSWORD = "OdbcStorePassword";
    protected const string KEY_ODBC_STORE_CONNECTION_STRING = "OdbcStoreConnectionString";
    protected const string KEY_FILE_LOG_PATH = "FileLogPath";
    protected const string KEY_MYSQL_LOG_DATABASE = "MySQLLogDatabase";
    protected const string KEY_MYSQL_LOG_USER = "MySQLLogUser";
    protected const string KEY_MYSQL_LOG_PASSWORD = "MySQLLogPassword";
    protected const string KEY_MYSQL_LOG_HOST = "MySQLLogHost";
    protected const string KEY_MYSQL_LOG_PORT = "MySQLLogPort";
    protected const string KEY_MYSQL_LOG_USE_CONNECTION_POOL = "MySQLLogUseConnectionPool";
    protected const string KEY_MYSQL_LOG_INCOMING_TABLE = "MySQLLogIncomingTable";
    protected const string KEY_MYSQL_LOG_OUTGOING_TABLE = "MySQLLogOutgoingTable";
    protected const string KEY_MYSQL_LOG_EVENT_TABLE = "MySQLLogEventTable";
    protected const string KEY_POSTGRESQL_LOG_DATABASE = "PostgreSQLLogDatabase";
    protected const string KEY_POSTGRESQL_LOG_USER = "PostgreSQLLogUser";
    protected const string KEY_POSTGRESQL_LOG_PASSWORD = "PostgreSQLLogPassword";
    protected const string KEY_POSTGRESQL_LOG_HOST = "PostgreSQLLogHost";
    protected const string KEY_POSTGRESQL_LOG_PORT = "PostgreSQLLogPort";
    protected const string KEY_POSTGRESQL_LOG_USE_CONNECTION_POOL = "PostgreSQLLogUseConnectionPool";
    protected const string KEY_POSTGRESQL_LOG_INCOMING_TABLE = "PostgreSQLLogIncomingTable";
    protected const string KEY_POSTGRESQL_LOG_OUTGOING_TABLE = "PostgreSQLLogOutgoingTable";
    protected const string KEY_POSTGRESQL_LOG_EVENT_TABLE = "PostgreSQLLogEventTable";
    protected const string KEY_ODBC_LOG_USER = "OdbcLogUser";
    protected const string KEY_ODBC_LOG_PASSWORD = "OdbcLogPassword";
    protected const string KEY_ODBC_LOG_CONNECTION_STRING = "OdbcLogConnectionString";
    protected const string KEY_ODBC_LOG_INCOMING_TABLE = "OdbcLogIncomingTable";
    protected const string KEY_ODBC_LOG_OUTGOING_TABLE = "OdbcLogOutgoingTable";
    protected const string KEY_ODBC_LOG_EVENT_TABLE = "OdbcLogEventTable";
    protected const string VALUE_CONNECTION_TYPE = "initiator";
    protected QuickFIXApplication application;
    protected IInitiator initiator;

    protected virtual Type SessionInfoType
    {
      get
      {
        return typeof (SessionInfo);
      }
    }

    [Editor(typeof (BeginStringEditor), typeof (UITypeEditor))]
    [Category("QuickFIX - Sessions (defaults)")]
    [Description("Version of FIX this session should use")]
    [DefaultValue("FIX.4.2")]
    public string BeginString { get; set; }

    [Category("QuickFIX - Sessions (defaults)")]
    [DefaultValue(typeof (TimeSpan), "00:00:00")]
    [Description("Time of day that this FIX session becomes activated")]
    public TimeSpan StartTime { get; set; }

    [Description("Time of day that this FIX session becomes deactivated")]
    [DefaultValue(typeof (TimeSpan), "00:00:00")]
    [Category("QuickFIX - Sessions (defaults)")]
    public TimeSpan EndTime { get; set; }

    [DefaultValue(false)]
    [Category("QuickFIX - Sessions (defaults)")]
    [Description("If set to true, QuickFIX will send all necessary resend requests, even if they appear redundant")]
    public bool SendRedundantResendRequests { get; set; }

    [Description("Determines if sequence numbers should be reset to 1 after a normal logout termination")]
    [Category("QuickFIX - Sessions (defaults)")]
    [DefaultValue(false)]
    public bool ResetOnLogout { get; set; }

    [DefaultValue(false)]
    [Category("QuickFIX - Sessions (defaults)")]
    [Description("Determines if sequence numbers should be reset to 1 after an abnormal termination")]
    public bool ResetOnDisconnect { get; set; }

    [Description("Determines if session state should be restored from persistence layer when logging on")]
    [DefaultValue(false)]
    [Category("QuickFIX - Sessions (defaults)")]
    public bool RefreshOnLogon { get; set; }

    [DefaultValue("FIX42.xml")]
    [Editor(typeof (FileNameEditor), typeof (UITypeEditor))]
    [Category("QuickFIX - Sessions (defaults)")]
    [Description("XML definition file for validating incoming FIX messages")]
    public string DataDictionary { get; set; }

    [Category("QuickFIX - Sessions (defaults)")]
    [DefaultValue(true)]
    [Description("If set to Y, messages must be received from the counterparty within a defined number of seconds (see MaxLatency)")]
    public bool CheckLatency { get; set; }

    [Description("If CheckLatency is set to Y, this defines the number of seconds latency allowed for a message to be processed")]
    [Category("QuickFIX - Sessions (defaults)")]
    [DefaultValue(typeof (TimeSpan), "00:02:00")]
    public TimeSpan MaxLatency { get; set; }

    [Category("QuickFIX - Sessions (defaults)")]
    [Description("Time between reconnection attempts")]
    [DefaultValue(typeof (TimeSpan), "00:00:30")]
    public TimeSpan ReconnectInterval { get; set; }

    [DefaultValue(typeof (TimeSpan), "00:00:20")]
    [Category("QuickFIX - Sessions (defaults)")]
    [Description("Heartbeat interval")]
    public TimeSpan HeartBtInt { get; set; }

    [DefaultValue(false)]
    [Category("QuickFIX - Storage")]
    [Description("If set to N, no messages will be persisted. This will force QuickFIX to always send GapFills instead of resending messages")]
    public bool PersistMessages { get; set; }

    [Category("QuickFIX - Storage")]
    public StorageInfo Storage { get; private set; }

    [Browsable(false)]
    public string StorageXml
    {
      get
      {
        return Helper.InfoToXml((InfoBase) this.Storage);
      }
      set
      {
        Helper.InfoFromXml((InfoBase) this.Storage, value);
      }
    }

    [DefaultValue(false)]
    [Category("QuickFIX - Logging")]
    public bool IsLoggingEnabled { get; set; }

    [Category("QuickFIX - Logging")]
    public LoggingInfo Logging { get; private set; }

    [Browsable(false)]
    public string LoggingXml
    {
      get
      {
        return Helper.InfoToXml((InfoBase) this.Logging);
      }
      set
      {
        Helper.InfoFromXml((InfoBase) this.Logging, value);
      }
    }

    [Editor(typeof (SessionsEditor), typeof (UITypeEditor))]
    [TypeConverter(typeof (SessionsTypeConverter))]
    [Category("QuickFIX - Sessions")]
    public SessionInfo[] Sessions { get; set; }

    [Browsable(false)]
    public string SessionsXml
    {
      get
      {
        return Helper.SessionsToXml(this.Sessions);
      }
      set
      {
        this.Sessions = Helper.SessionsFromXml(value);
      }
    }

    [DefaultValue(InitiatorMode.Single)]
    [Category("QuickFIX - Initiator")]
    public InitiatorMode InitiatorMode { get; set; }

    protected QuickFIXProvider(Framework framework, QuickFIXApplication application)
      : base(framework)
    {
      this.application = application;
      this.application.SetProvider(this);
      this.initiator = (IInitiator) null;
      this.InitSettings();
    }

    internal Type GetSessionInfoType()
    {
      return this.SessionInfoType;
    }

    public override void Connect()
    {
      if (this.initiator != null)
        return;
      this.Status = ProviderStatus.Connecting;
      List<SessionInfo> list = new List<SessionInfo>();
      foreach (SessionInfo sessionInfo in this.Sessions)
      {
        if (sessionInfo.IsEnabled)
          list.Add(sessionInfo);
      }
      SettingErrorList errors = new SettingErrorList();
      if (list.Count == 0)
        this.EmitError("You need to define at least one enabled session");
      foreach (SessionInfo sessionInfo in list)
      {
        if (sessionInfo.Type == SessionType.UNDEFINED)
          this.EmitError(string.Format("You need to set the type for session {0}", (object) sessionInfo.ToString()));
      }
      this.VerifySettings(errors);
      foreach (SettingError settingError in errors.All)
      {
        if (settingError.IsWarning)
          this.EmitWarning(settingError.Text);
        else
          this.EmitError(settingError.Text);
      }
      if (errors.HasErrors)
      {
        this.Status = ProviderStatus.Disconnected;
      }
      else
      {
        SessionSettings settings1 = new SessionSettings();
        Dictionary defaults = new Dictionary();
        defaults.SetString("ConnectionType", "initiator");
        defaults.SetString("StartTime", this.StartTime.ToString());
        defaults.SetString("EndTime", this.EndTime.ToString());
        defaults.SetBool("SendRedundantResendRequests", this.SendRedundantResendRequests);
        defaults.SetBool("ResetOnLogout", this.ResetOnLogout);
        defaults.SetBool("ResetOnDisconnect", this.ResetOnDisconnect);
        defaults.SetBool("RefreshOnLogon", this.RefreshOnLogon);
        defaults.SetString("DataDictionary", this.DataDictionary);
        defaults.SetBool("CheckLatency", this.CheckLatency);
        defaults.SetLong("MaxLatency", (long) (int) this.MaxLatency.TotalSeconds);
        defaults.SetLong("ReconnectInterval", (long) (int) this.ReconnectInterval.TotalSeconds);
        defaults.SetLong("HeartBtInt", (long) (int) this.HeartBtInt.TotalSeconds);
        defaults.SetBool("PersistMessages", this.PersistMessages);
        if (this.PersistMessages)
        {
          switch (this.Storage.Type)
          {
            case InfoType.FILE:
              defaults.SetString("FileStorePath", this.Storage.FILE.FileStorePath);
              break;
            case InfoType.MYSQL:
              defaults.SetString("MySQLStoreDatabase", this.Storage.MYSQL.MySQLStoreDatabase);
              defaults.SetString("MySQLStoreUser", this.Storage.MYSQL.MySQLStoreUser);
              defaults.SetString("MySQLStorePassword", this.Storage.MYSQL.MySQLStorePassword);
              defaults.SetString("MySQLStoreHost", this.Storage.MYSQL.MySQLStoreHost);
              defaults.SetLong("MySQLStorePort", (long) (int) this.Storage.MYSQL.MySQLStorePort);
              defaults.SetBool("MySQLStoreUseConnectionPool", this.Storage.MYSQL.MySQLStoreUseConnectionPool);
              break;
            case InfoType.POSTGRESQL:
              defaults.SetString("PostgreSQLStoreDatabase", this.Storage.POSTGRESQL.PostgreSQLStoreDatabase);
              defaults.SetString("PostgreSQLStoreUser", this.Storage.POSTGRESQL.PostgreSQLStoreUser);
              defaults.SetString("PostgreSQLStorePassword", this.Storage.POSTGRESQL.PostgreSQLStorePassword);
              defaults.SetString("PostgreSQLStoreHost", this.Storage.POSTGRESQL.PostgreSQLStoreHost);
              defaults.SetLong("PostgreSQLStorePort", (long) (int) this.Storage.POSTGRESQL.PostgreSQLStorePort);
              defaults.SetBool("PostgreSQLStoreUseConnectionPool", this.Storage.POSTGRESQL.PostgreSQLStoreUseConnectionPool);
              break;
            case InfoType.ODBC:
              defaults.SetString("OdbcStoreUser", this.Storage.ODBC.OdbcStoreUser);
              defaults.SetString("OdbcStorePassword", this.Storage.ODBC.OdbcStorePassword);
              defaults.SetString("OdbcStoreConnectionString", this.Storage.ODBC.OdbcStoreConnectionString);
              break;
          }
        }
        if (this.IsLoggingEnabled)
        {
          switch (this.Logging.Type)
          {
            case InfoType.FILE:
              defaults.SetString("FileLogPath", this.Logging.FILE.FileLogPath);
              break;
            case InfoType.MYSQL:
              defaults.SetString("MySQLLogDatabase", this.Logging.MYSQL.MySQLLogDatabase);
              defaults.SetString("MySQLLogUser", this.Logging.MYSQL.MySQLLogUser);
              defaults.SetString("MySQLLogPassword", this.Logging.MYSQL.MySQLLogPassword);
              defaults.SetString("MySQLLogHost", this.Logging.MYSQL.MySQLLogHost);
              defaults.SetLong("MySQLLogPort", (long) (int) this.Logging.MYSQL.MySQLLogPort);
              defaults.SetBool("MySQLLogUseConnectionPool", this.Logging.MYSQL.MySQLLogUseConnectionPool);
              defaults.SetString("MySQLLogIncomingTable", this.Logging.MYSQL.MySQLLogIncomingTable);
              defaults.SetString("MySQLLogOutgoingTable", this.Logging.MYSQL.MySQLLogOutgoingTable);
              defaults.SetString("MySQLLogEventTable", this.Logging.MYSQL.MySQLLogEventTable);
              break;
            case InfoType.POSTGRESQL:
              defaults.SetString("PostgreSQLLogDatabase", this.Logging.POSTGRESQL.PostgreSQLLogDatabase);
              defaults.SetString("PostgreSQLLogUser", this.Logging.POSTGRESQL.PostgreSQLLogUser);
              defaults.SetString("PostgreSQLLogPassword", this.Logging.POSTGRESQL.PostgreSQLLogPassword);
              defaults.SetString("PostgreSQLLogHost", this.Logging.POSTGRESQL.PostgreSQLLogHost);
              defaults.SetLong("PostgreSQLLogPort", (long) (int) this.Logging.POSTGRESQL.PostgreSQLLogPort);
              defaults.SetBool("PostgreSQLLogUseConnectionPool", this.Logging.POSTGRESQL.PostgreSQLLogUseConnectionPool);
              defaults.SetString("PostgreSQLLogIncomingTable", this.Logging.POSTGRESQL.PostgreSQLLogIncomingTable);
              defaults.SetString("PostgreSQLLogOutgoingTable", this.Logging.POSTGRESQL.PostgreSQLLogOutgoingTable);
              defaults.SetString("PostgreSQLLogEventTable", this.Logging.POSTGRESQL.PostgreSQLLogEventTable);
              break;
            case InfoType.ODBC:
              defaults.SetString("OdbcLogUser", this.Logging.ODBC.OdbcLogUser);
              defaults.SetString("OdbcLogPassword", this.Logging.ODBC.OdbcLogPassword);
              defaults.SetString("OdbcLogConnectionString", this.Logging.ODBC.OdbcLogConnectionString);
              defaults.SetString("OdbcLogIncomingTable", this.Logging.ODBC.OdbcLogIncomingTable);
              defaults.SetString("OdbcLogOutgoingTable", this.Logging.ODBC.OdbcLogOutgoingTable);
              defaults.SetString("OdbcLogEventTable", this.Logging.ODBC.OdbcLogEventTable);
              break;
          }
        }
        settings1.Set(defaults);
        this.application.ResetSessions();
        foreach (SessionInfo session in list)
        {
          Dictionary settings2 = new Dictionary();
          settings2.SetLong("SocketConnectPort", (long) (int) session.SocketConnectPort);
          settings2.SetString("SocketConnectHost", session.SocketConnectHost);
          SessionID sessionID = new SessionID(this.BeginString, session.SenderCompID, session.TargetCompID);
          settings1.Set(sessionID, settings2);
          this.application.AddSession(sessionID, session);
        }
        IMessageFactory messageFactory = (IMessageFactory) new DefaultMessageFactory();
        IMessageStoreFactory storeFactory = (IMessageStoreFactory) null;
        if (this.PersistMessages)
        {
          switch (this.Storage.Type)
          {
            case InfoType.FILE:
              storeFactory = (IMessageStoreFactory) new FileStoreFactory(settings1);
              break;
            case InfoType.MYSQL:
              storeFactory = (IMessageStoreFactory) null;
              break;
            case InfoType.POSTGRESQL:
              storeFactory = (IMessageStoreFactory) null;
              break;
            case InfoType.ODBC:
              storeFactory = (IMessageStoreFactory) null;
              break;
          }
        }
        else
          storeFactory = (IMessageStoreFactory) new MemoryStoreFactory();
        ILogFactory logFactory = (ILogFactory) null;
        if (this.IsLoggingEnabled)
        {
          switch (this.Logging.Type)
          {
            case InfoType.FILE:
              logFactory = (ILogFactory) new FileLogFactory(settings1);
              break;
            case InfoType.MYSQL:
              logFactory = (ILogFactory) null;
              break;
            case InfoType.POSTGRESQL:
              logFactory = (ILogFactory) null;
              break;
            case InfoType.ODBC:
              logFactory = (ILogFactory) null;
              break;
          }
        }
        else
          logFactory = (ILogFactory) new NullLogFactory();
        try
        {
          switch (this.InitiatorMode)
          {
            case InitiatorMode.Single:
              this.initiator = (IInitiator) new SocketInitiator((IApplication) this.application, storeFactory, settings1, logFactory, messageFactory);
              break;
            case InitiatorMode.Threaded:
              this.initiator = (IInitiator) new SocketInitiator((IApplication) this.application, storeFactory, settings1, logFactory, messageFactory);
              break;
          }
          this.initiator.Start();
        }
        catch (Exception ex)
        {
          this.EmitError(((object) ex).ToString());
        }
      }
    }

    public override void Disconnect()
    {
      if (this.initiator == null)
        return;
      this.Status = ProviderStatus.Disconnecting;
      foreach (SessionID sessionID in this.initiator.GetSessionIDs())
        Session.LookupSession(sessionID).Logout();
    }

    protected virtual void VerifySettings(SettingErrorList errors)
    {
      if (!File.Exists(this.DataDictionary))
        errors.AddError(string.Format("Data dictionary file ({0}) not found", (object) this.DataDictionary));
      if (this.PersistMessages && this.Storage.Type == InfoType.FILE && !Directory.Exists(this.Storage.FILE.FileStorePath))
        errors.AddError(string.Format("FileStorePath ({0}) does not exist", (object) this.Storage.FILE.FileStorePath));
      if (!this.IsLoggingEnabled || this.Logging.Type != InfoType.FILE || Directory.Exists(this.Logging.FILE.FileLogPath))
        return;
      errors.AddError(string.Format("FileLogPath ({0}) does not exist", (object) this.Logging.FILE.FileLogPath));
    }

    internal void OnSessionLogon()
    {
      if (this.initiator == null)
        return;
      bool flag = true;
      foreach (SessionID sessionID in this.initiator.GetSessionIDs())
      {
        if (!Session.LookupSession(sessionID).IsLoggedOn)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        this.Status = ProviderStatus.Connected;
      else
        this.Status = ProviderStatus.Connecting;
    }

    internal void OnSessionLogout()
    {
      if (this.initiator == null)
        return;
      bool flag = true;
      foreach (SessionID sessionID in this.initiator.GetSessionIDs())
      {
        if (Session.LookupSession(sessionID).IsLoggedOn)
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        if (this.Status == ProviderStatus.Disconnecting)
        {
          this.initiator.Stop();
          this.initiator = (IInitiator) null;
        }
        this.Status = ProviderStatus.Disconnected;
      }
      else
      {
        if (this.Status == ProviderStatus.Disconnecting)
          return;
        this.Status = ProviderStatus.Connecting;
      }
    }

    internal new void EmitError(string text)
    {
      base.EmitError(text);
    }

    internal new void EmitWarning(string text)
    {
      base.EmitWarning(text);
    }

    internal new void EmitMessage(string text)
    {
      base.EmitMessage(text);
    }

    internal void EmitInstrumentDefinition(string requestId, Instrument[] instruments, int totalNum)
    {
      base.EmitInstrumentDefinition(new InstrumentDefinition()
      {
        RequestId = requestId,
        ProviderId = this.id,
        Instruments = instruments,
        TotalNum = totalNum
      });
    }

    internal new void EmitInstrumentDefinitionEnd(string requestId, RequestResult result, string text)
    {
      base.EmitInstrumentDefinitionEnd(requestId, result, text);
    }

    internal void EmitTick(Instrument instrument, Tick tick)
    {
      tick.DateTime = this.framework.Clock.DateTime;
      tick.ProviderId = this.id;
      tick.InstrumentId = instrument.Id;
      this.EmitData((DataObject) tick, true);
    }

    public new virtual void Send(InstrumentDefinitionRequest request)
    {
      if (this.Status == ProviderStatus.Connected)
        this.application.Send(request);
      else
        this.EmitInstrumentDefinitionEnd(request.Id, RequestResult.Error, "Provider is not connected.");
    }

    public virtual void Cancel(string requestId)
    {
    }

    public override void Subscribe(Instrument instrument)
    {
      if (this.Status == ProviderStatus.Connected)
        this.application.Subscribe(instrument);
      else
        this.EmitError("Provider is not connected.");
    }

    public override void Unsubscribe(Instrument instrument)
    {
      if (this.Status == ProviderStatus.Connected)
        this.application.Unsubscribe(instrument);
      else
        this.EmitError("Provider is not connected.");
    }

    private void InitSettings()
    {
      this.BeginString = "FIX.4.2";
      this.StartTime = TimeSpan.Parse("00:00:00");
      this.EndTime = TimeSpan.Parse("00:00:00");
      this.SendRedundantResendRequests = false;
      this.ResetOnLogout = false;
      this.ResetOnDisconnect = false;
      this.RefreshOnLogon = false;
      this.DataDictionary = "FIX42.xml";
      this.CheckLatency = true;
      this.MaxLatency = TimeSpan.Parse("00:02:00");
      this.ReconnectInterval = TimeSpan.Parse("00:00:30");
      this.HeartBtInt = TimeSpan.Parse("00:00:20");
      this.PersistMessages = false;
      this.IsLoggingEnabled = false;
      this.Storage = new StorageInfo();
      this.Logging = new LoggingInfo();
      this.Sessions = new SessionInfo[0];
      this.InitiatorMode = InitiatorMode.Single;
    }
  }
}
