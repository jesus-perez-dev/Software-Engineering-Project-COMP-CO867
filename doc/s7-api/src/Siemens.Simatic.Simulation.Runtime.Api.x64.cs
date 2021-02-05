/*
 * Siemens.Simatic.Simulation.Runtime.Api.x64
 * Version=1.0.0.0
 * Culture=neutral
 * PublicKeyToken=null
*/

namespace Siemens.Simatic.Simulation.Runtime
{

	public delegate void Delegate_II_EREC_DT (IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime);
	public delegate void Delegate_II_EREC_DT_ELT_ELM (IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime, ELEDType in_LEDType, ELEDMode in_LEDMode);
	public delegate void Delegate_II_EREC_DT_EOS_EOS (IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime, EOperatingState in_PrevState, EOperatingState in_OperatingState);
	public delegate void Delegate_II_EREC_DT_INT64_UINT32 (IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime, long in_CycleTime_ns, uint in_CycleCount);
	public delegate void Delegate_II_EREC_DT_SRICC_UINT32_UINT32_UINT32_UINT32 (IInstance in_Sender, ERuntimeErrorCode in_ErrorCode, DateTime in_DateTime, EInstanceConfigChanged in_InstanceConfigChanged, uint in_Param1, uint in_Param2, uint in_Param3, uint in_Param4);
	public delegate void Delegate_IRRTM (IRemoteRuntimeManager in_Sender);
	public delegate void Delegate_SRCC_UINT32_UINT32_INT32 (ERuntimeConfigChanged in_RuntimeConfigChanged, uint in_Param1, uint in_Param2, int in_Param3);
	public delegate void Delegate_Void ();

	/// This enumeration contains all PLC areas that contain the available PLC tags.
	///
	/// Enum:
	///
	///     InvalidArea - 0
	///     Input - 1
	///     Marker - 2
	///     Output - 3
	///     Counter - 4
	///     Timer - 5
	///     DataBlock - 6
	public enum EArea {
		InvalidArea,
		Input,
		Marker,
		Output,
		Counter,
		Timer,
		DataBlock
	}

	/// This enumeration contains the available communication interfaces of a virtual controller.
	///
	/// Enum:
	///
	///     None - 0
	///     Softbus - 1
	///     TCPIP - 2
	public enum ECommunicationInterface {
		None,
		Softbus,
		TCPIP
	}

	/// This enumeration contains all CPU types that can be loaded in a virtual controller.
	///
	/// Enum:
	///
	///    CPU1500_Unspecified - 0
	///    CPU1511 - 1
	///    CPU1511v2 - 2
	///    CPU1513 - 3
	///    ... - ...
	///    CPU1512SPFv2 - 37
	public enum ECPUType {
		CPU1500_Unspecified,
		CPU1511,
		CPU1511v2,
		CPU1513,
		CPU1513v2,
		CPU1515,
		CPU1515v2,
		CPU1516,
		CPU1516v2,
		CPU1517,
		CPU1518,
		CPU1511C,
		CPU1512C,
		CPU1511F,
		CPU1511Fv2,
		CPU1513F,
		CPU1513Fv2,
		CPU1515F,
		CPU1515Fv2,
		CPU1516F,
		CPU1516Fv2,
		CPU1517F,
		CPU1518F,
		CPU1511T,
		CPU1515T,
		CPU1517T,
		CPU1517TF,
		CPU1518ODK,
		CPU1518FODK,
		ET200SP_Unspecified,
		CPU1510SP,
		CPU1510SPv2,
		CPU1512SP,
		CPU1512SPv2,
		CPU1510SPF,
		CPU1510SPFv2,
		CPU1512SPF,
		CPU1512SPFv2
	}

	/// This list contains all the primitive data types that are used by the I/O access functions.
	///
	/// Enum:
	///
	///    Unknown - 0
	///    Bool - 1
	///    Byte - 2
	///    Char - 3
	///    ... - ...
	///    DB - 82
	public enum EDataType {
		Unknown,
		Bool,
		Byte,
		Char,
		Word,
		Int,
		DWord,
		DInt,
		Real,
		Date,
		TimeOfDay,
		Time,
		S5Time,
		DateAndTime,
		Struct,
		String,
		Counter,
		Timer,
		IEC_Counter,
		IEC_Timer,
		LReal,
		ULInt,
		LInt,
		LWord,
		USInt,
		UInt,
		UDInt,
		SInt,
		WChar,
		WString,
		LTime,
		LTimeOfDay,
		LDT,
		DTL,
		IEC_LTimer,
		IEC_SCounter,
		IEC_DCounter,
		IEC_LCounter,
		IEC_UCounter,
		IEC_UDCounter,
		IEC_USCounter,
		IEC_ULCounter,
		ErrorStruct,
		NREF,
		CREF,
		Aom_Ident,
		Event_Any,
		Event_Att,
		Event_HwInt,
		Hw_Any,
		Hw_IoSystem,
		Hw_DpMaster,
		Hw_Device,
		Hw_DpSlave,
		Hw_Io,
		Hw_Module,
		Hw_SubModule,
		Hw_Hsc,
		Hw_Pwm,
		Hw_Pto,
		Hw_Interface,
		Hw_IEPort,
		OB_Any,
		OB_Delay,
		OB_Tod,
		OB_Cyclic,
		OB_Att,
		Conn_Any,
		Conn_Prg,
		Conn_Ouc,
		Conn_R_ID,
		Port,
		Rtm,
		Pip,
		OB_PCycle,
		OB_HwInt,
		OB_Diag,
		OB_TimeError,
		OB_Startup,
		DB_Any,
		DB_WWW,
		DB_Dyn,
		DB
	}

	/// This list contains all possible causes for a OnConfigurationChanged event that the virtual controller sends.
	///
	/// Enum:
	///
	///   HardwareSoftwareChanged,
	///   IPChanged
	public enum EInstanceConfigChanged {
		HardwareSoftwareChanged,
		IPChanged
	}

	/// This list contains all the LED states of a virtual controller.
	///
	/// Enum:
	///
	///     Off - 0
	///     On - 1
	///     FlashFast - 2
	///     FlashSlow - 3
	///     Invalid - 4
	public enum ELEDMode {
		Off,
		On,
		FlashFast,
		FlashSlow,
		Invalid
	}

	/// This list includes all types of LEDs of a virtual controller.
	///
	/// Enum:
	///
	///     Stop - 0
	///     Run - 1
	///     Error - 2
	///     Maint - 3
	///     Redund - 4
	///     Force - 5
	///     Busf1 - 6
	///     Busf2 - 7
	///     Busf3 - 8
	///     Busf4 - 9
	public enum ELEDType {
		Stop,
		Run,
		Error,
		Maint,
		Redund,
		Force,
		Busf1,
		Busf2,
		Busf3,
		Busf4
	}

	/// This enumeration contains all the operating modes of a virtual controller.
	///
	/// Enum:
	///
	///     Default - 0
	///     SingleStep - 1
	///     ExtendedSingleStep - 2
	///     TimespanSynchronize - 3
	public enum EOperatingMode {
		Default,
		SingleStep,
		ExtendedSingleStep,
		TimespanSynchronized
	}

	/// This enumeration contains all the operating states of a virtual controller.
	///
	/// Enum:
	///
	///     InvalidOperatingState - 0
	///     Off - 1
	///     Booting - 2
	///     Stop - 3
	///     Startup - 4
	///     Run - 5
	///     Freeze - 6
	///     ShuttingDow - 7
	public enum EOperatingState {
		InvalidOperatingState,
		Off,
		Booting,
		Stop,
		Startup,
		Run,
		Freeze,
		ShuttingDown
	}

	/// This list contains all the primitive data types that are used by the I/O access functions.
	///
	/// Enum:
	///
	///     Unspecific - 0
	///     Struct - 1
	///     Bool - 2
	///     Int8 - 3
	///     Int16 - 4
	///     Int32 - 5
	///     Int64 - 6
	///     UInt8 - 7
	///     UInt16 - 8
	///     UInt32 - 9
	///     UInt64 - 10
	///     Float - 11
	///     Double - 12
	///     Char - 13
	///     WChar - 14
	public enum EPrimitiveDataType {
		Unspecific,
		Struct,
		Bool,
		Int8,
		Int16,
		Int32,
		Int64,
		UInt8,
		UInt16,
		UInt32,
		UInt64,
		Float,
		Double,
		Char,
		WChar
	}

	/// This list contains all possible causes of a OnConfigurationChanged event that the Runtime Manager sends.
	///
	/// Enum:
	///
	///     InstanceRegistered - 0
	///     InstanceUnregistered - 1
	///     ConnectionOpened - 2
	///     ConnectionClosed - 3
	///     PortOpened - 4
	///     PortClose - 5
	public enum ERuntimeConfigChanged {
		InstanceRegistered,
		InstanceUnregistered,
		ConnectionOpened,
		ConnectionClosed,
		PortOpened,
		PortClosed
	}

	/// This enumeration contains all error codes that are used by the Simulation Runtime API.
	///
	/// Most API functions return one of these error codes.
	///
	/// If the function is successful, the return value is always SREC_OK.
	///
	/// Errors are returned with negative values, and alarms with positive values.
	///
	/// Enum:
	///
	///     OK - 0
	///     InvalidErrorCode - 1 
	///     NotImplemented - 2 
	///     ... - ...
	///     IsEmpty - 52
	public enum ERuntimeErrorCode {
		OK,
		InvalidErrorCode,
		NotImplemented,
		IndexOutOfRange,
		DoesNotExist,
		AlreadyExists,
		UnknownMessageType,
		InvalidMessageId,
		WrongArgument,
		WrongPipe,
		ConnectionError,
		Timeout,
		MessageCorrupt,
		WrongVersion,
		InstanceNotRunning,
		InterfaceRemoved,
		SharedMemoryNotInitialized,
		ApiNotInitialized,
		WarningAlreadyExists,
		NotSupported,
		WarningInvalidCall,
		ErrorLoadingDll,
		SignalNameDoesNotExist,
		SignalTypeMismatch,
		SignalConfigurationError,
		NoSignalConfigurationLoaded,
		ConfiguredConnectionNotFound,
		ConfiguredDeviceNotFound,
		InvalidConfiguration,
		TypeMismatch,
		LicenseNotFound,
		NoLicenseAvailable,
		WrongCommunicationInterface,
		LimitReached,
		NoStoragePathSet,
		StoragePathAlreadyInUse,
		MesssageIncomplete,
		ArchiveStorageNotCreated,
		RetrieveStorageFailure,
		InvalidOperatingState,
		InvalidArchivePath,
		DeleteExistingStorageFailed,
		CreateDirectoriesFailed,
		NotEnoughMemory,
		WarningTrialModeActive,
		NotRunning,
		NotEmpty,
		NotUpToDate,
		CommunicationInterfaceNotAvailable,
		WarningNotComplete,
		VirtualSwitchMisconfigured,
		RuntimeNotAvailable,
		IsEmpty
	}

	/// This list contains all PLC areas that can be used as a filter to update the tag table.
	///
	/// Enum:
	///
	///     None - 0
	///     IO - 1
	///     M - 2
	///     IOM - 3
	///     CT - 4
	///     IOCT - 5
	///     MCT - 6
	///     IOMCT - 7
	///     DB - 8
	///     IODB - 9
	///     MDB - 10
	///     IOMDB - 11
	///     CTDB - 12
	///     IOCTDB - 13
	///     MCTDB - 14
	///     IOMCTD - 15
	public enum ETagListDetails {
		None,
		IO,
		M,
		IOM,
		CT,
		IOCT,
		MCT,
		IOMCT,
		DB,
		IODB,
		MDB,
		IOMDB,
		CTDB,
		IOCTDB,
		MCTDB,
		IOMCTDB
	}

	/// Interface of a Runtime instance.
	///
	/// It is used to change the operating state of a virtual controller and to exchange I/O data.
	///
	/// Each instance has a unique name and an ID.
	public interface IInstance : IDisposable {

		/// The user program, the hardware configuration and the retentive data are stored in a file, the Virtual SIMATIC Memory Card.
		///
		/// ArchiveStorage() stores this file as a ZIP file.
		///
		/// The instance of the virtual controller must be in OFF operating state for this.
		void ArchiveStorage (string in_FullFileName);

		/// Writes all entries from the tag list to an XML file.
		void CreateConfigurationFile (string in_FullFileName);

		/// Returns the current update status of the tag list storage. "inout_TagListDetails" is NONE, if the list needs to be updated.
		void GetTagListStatus (out ETagListDetails out_TagListDetails, out bool out_IsHMIVisibleOnly);

		/// Shuts down the virtual controller, closes its processes and performs a restart.
		void MemoryReset ();
		/// See: <MemoryReset>
		void MemoryReset (uint in_TimeOut_ms);

		/// Shuts down the Simulation Runtime and closes its process.
		void PowerOff ();
		/// See: <PowerOff>
		void PowerOff (uint in_TimeOut_ms);

		/// The function creates the process for the Simulation Runtime instance and starts the firmware of the virtual controller.
		ERuntimeErrorCode PowerOn ();
		/// See: <PowerOn>
		ERuntimeErrorCode PowerOn (uint in_TimeOut_ms);

		/// Reads the value of a PLC tag.
		///
		/// Parameters:
		///
		///     in_Tag - The name of the PLC tag that is to be read.
		///
		/// Returns:
		///
		///     Contains the value of the PLC tag
		///
		/// Exceptions:
		///
		///     ERuntimeErrorCode.InterfaceRemoved       - The instance is not registered in Runtime Manager.
		///     ERuntimeErrorCode.Timeout                - The function does not return on time.
		///     ERuntimeErrorCode.InstanceNotRunning     - The process of the virtual controller is not running.
		///     ERuntimeErrorCode.IndexOutOfRange        - The offset lies outside the area range. No value could be read.
		///     ERuntimeErrorCode.DoesNotExist           - The entry does not exist in the stored tag
		SDataValue Read (string in_Tag);
		/// See: <Read>
		bool ReadBool (string in_Tag);
		/// See: <Read>
		sbyte ReadChar (string in_Tag);
		/// See: <Read>
		double ReadDouble (string in_Tag);
		/// See: <Read>
		float ReadFloat (string in_Tag);
		/// See: <Read>
		short ReadInt16 (string in_Tag);
		/// See: <Read>
		int ReadInt32 (string in_Tag);
		/// See: <Read>
		long ReadInt64 (string in_Tag);
		/// See: <Read>
		sbyte ReadInt8 (string in_Tag);
		/// See: <Read>
		void ReadSignals (ref SDataValueByName[] inout_Signals);
		/// See: <Read>
		ushort ReadUInt16 (string in_Tag);
		/// See: <Read>
		uint ReadUInt32 (string in_Tag);
		/// See: <Read>
		ulong ReadUInt64 (string in_Tag);
		/// See: <Read>
		byte ReadUInt8 (string in_Tag);
		/// See: <Read>
		char ReadWChar (string in_Tag);

		/// When the event occurs, the registered event object is set to the signaled state.
		///
		/// Only one event object can be registered for the event.
		///
		/// Registration of a new event object causes the previous event object to be deleted.
		void RegisterOnConfigurationChangedEvent ();
		/// See: <RegisterOnConfigurationChangedEvent>
		void RegisterOnConfigurationChangingEvent ();
		/// See: <RegisterOnConfigurationChangedEvent>
		void RegisterOnEndOfCycleEvent ();
		/// See: <RegisterOnConfigurationChangedEvent>
		void RegisterOnLedChangedEvent ();
		/// See: <RegisterOnConfigurationChangedEvent>
		void RegisterOnOperatingStateChangedEvent ();

		/// Creates a Virtual SIMATIC Memory Card from the archived ZIP file. The virtual controller must be in OFF operating state for this.
		void RetrieveStorage (string in_FullFileName);

		/// Calls on the virtual controller to change to RUN operating state.
		///
		/// Exceptions:
		///
		///     ERuntimeErrorCode.InterfaceRemoved   - The instance is not registered in Runtime Manager.
		///     ERuntimeErrorCode.Timeout            - The expected operating state does not occur on time.
		///     ERuntimeErrorCode.InstanceNotRunning - The process of the virtual controller is not running.
		void Run ();

		/// Calls on the virtual controller to change to RUN operating state.
		///
		/// Parameters:
		///
		/// in_TimeOut_ms - A timeout value in milliseconds.
		///
		///    - If no timeout value is set, the function returns immediately. Subscribe to the
		///      OnOperatingStateChanged() event to find out when the operation has
		///      been completed.
		///
		///    - If the value is greater than 0 (a value of 60000 is recommended), the func-
		///      tion returns when the operation has been completed or after a timeout.
		///      Expected operating states when this function is successful: { EOperatingState.Run }
		///
		/// See Also:
		///     <run>
		void Run (uint in_TimeOut_ms);

		/// Undocumented
		void RunNextCycle ();

		/// Sets the IP suite of the network interface of a virtual controller.
		void SetIPSuite (uint in_InterfaceID, SIPSuite4 in_IPSuite, bool in_IsRemanent);

		/// If the virtual controller is running in a TimespanSynchronized operating mode, it is stopped at the synchronization point (Freeze state).
		///
		/// The StartProcessing() function cancels the freeze state.
		///
		/// The virtual controller will now run for at least the requested time before it changes to Freeze state at the next synchronization point.
		void StartProcessing (long in_MinimalTimeToRun_ns);

		/// Calls on the virtual controller to change to STOP operating state.
        ///
        /// Exceptions:
		///
		///     ERuntimeErrorCode.InterfaceRemoved   - The instance is not registered in Runtime Manager.
		///     ERuntimeErrorCode.Timeout            - The expected operating state does not occur on time.
		///     ERuntimeErrorCode.InstanceNotRunning - The process of the virtual controller is not running.
		///
		/// See Also:
		///
		///     <Run>
		void Stop ();
		/// See: <Stop>, <Run>
		void Stop (uint in_TimeOut_ms);

		/// Unregisters this instance from Runtime Manager.
		///
		/// Exceptions:
		///
		///    ERuntimeErrorCode.InterfaceRemoved - The instance is not registered in Runtime Manager.
		///    ERuntimeErrorCode.Timeout          - The function does not return on time.
		void UnregisterInstance ();
		/// See: <UnregisterInstance>
		void UnregisterOnConfigurationChangedEvent ();
		/// See: <UnregisterInstance>
		void UnregisterOnConfigurationChangingEvent ();
		/// See: <UnregisterInstance>
		void UnregisterOnEndOfCycleEvent ();
		/// See: <UnregisterInstance>
		void UnregisterOnLedChangedEvent ();
		/// See: <UnregisterInstance>
		void UnregisterOnOperatingStateChangedEvent ();

		/// The function reads the tags from the virtual controller and writes them to the shared storage arranged by name.
		///
		/// If the tag is an array or a structure, there are multiple entries.
		///
		/// In the case of a structure, there is an entry for the structure itself and an additional entry for each structure element.
		///
		/// >   Entry_1: "StructName"
		/// >   Entry_2: "StructName.ElementName_1"
		/// >   ..
		/// >   Entry_N: "StructName.ElementName_n"
		///
		/// In the case of an array, in this example a two-dimensional array, there is an entry for the array itself and an additional entry for each array element.
		///
		/// >    Entry_1: "ArrayName"
		/// >    Entry_2: "ArrayName[a,b]", where {a} and {b} correspond to the first index of the respective dimension 
		/// >    .. 
		/// >    Entry_N: "ArrayName[x,y]", where {x} and {y} correspond to the last index of the respective dimension 
		///
		/// Memory for up to 500000 entries (not PLC tags) is reserved for the list. If the list becomes too large, the function returns the error/exception "NOT_ENOUGH_MEMORY".
		///
		/// Parameters:
		///
		///     in_TagListDetails - Every combination of the following four areas:
		///
		///         *IO* : Inputs and Outputs
		///
		///         *M*  : Bit memory
		///
		///         *CT* : Counters and Timers
		///
		///         *DB* : Data Blocks
		///
		///         The default setting is IOMCTDB.
		///
		///         Example: IOM reads only the tags from the area Inputs / Outputs and Bit memory.
		///
		///     in_IsHMIVisibleOnly - If true, only tags marked with "HMI Visible" are read. The default setting is true
		///
		///     in_DataBlockFilterList - A string that includes the name of all data blocks that are supposed to be available in the tag table. The string must be in quotation marks.
		///
		///         Example: ""\"DB_1\", \"DB_2\" \"DB_3\"|\"DB_4\"\"DB_5\""
		///
		///         All characters within the quotation marks are interpreted as a DB name. If the
		///         data block does not exist in the PLC program, it is not added to the tag table
		///         memory. No error is triggered in the process.
		///
		///         For this list to be taken into consideration, in_DataBlockFilterList has to be
		///         unequal to NULL and in_TagListDetails has to contain "DB".
		///
		/// Exceptions:
		///
		///  - ERuntimeErrorCode.InterfaceRemoved     The instance is not registered in Runtime Manager.
		///  - ERuntimeErrorCode.Timeout              The function does not return on time.
		void UpdateTagList ();

		/// See: <UpdateTagList>
		void UpdateTagList (ETagListDetails in_TagListDetails);

		/// See: <UpdateTagList>
		void UpdateTagList (ETagListDetails in_TagListDetails, bool in_IsHMIVisibleOnly);

		/// The function blocks the program until the registered event object is in the signaled state or the timeout interval is exceeded.
		bool WaitForOnConfigurationChangedEvent ();
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnConfigurationChangedEvent (uint in_Time_ms);
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnConfigurationChangingEvent ();
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnConfigurationChangingEvent (uint in_Time_ms);
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnEndOfCycleEvent ();
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnEndOfCycleEvent (uint in_Time_ms);
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnLedChangedEvent ();
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnLedChangedEvent (uint in_Time_ms);
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnOperatingStateChangedEvent ();
		/// See: <WaitForOnConfigurationChangedEvent>
		bool WaitForOnOperatingStateChangedEvent (uint in_Time_ms);
		
		/// Writes the value of a PLC tag.
		///
		/// Parameters:
		///
		///     in_Tag   - The name of the PLC tag that is to be written.
		///
		///     in_Value - Contains the value and the expected type of the PLC tag. The UNSPECIFIC and STRUCT types are not supported.
		///
		///         Structures and fields can be emulated through signal lists and then be written
		///         by using the WriteSignals() function.
		///
		/// Exceptions:
		///
		///     ERuntimeErrorCode.InterfaceRemoved   -  The instance is not registered in Runtime Manager.
		///     ERuntimeErrorCode.Timeout            -  The function does not return on time.
		///     ERuntimeErrorCode.InstanceNotRunning -  The process of the virtual controller is not running.
		///     ERuntimeErrorCode.IndexOutOfRange    -  The offset lies outside the area range. No value could be written.
		///     ERuntimeErrorCode.DoesNotExist       -  The entry does not exist in the stored tag list.
		///     ERuntimeErrorCode.NotSupported       -  Access to entire structures or arrays is not supported.
		///     ERuntimeErrorCode.TypeMismatch       -  The expected type does not match the stored type. See Compatible primitive data types (Page 375).
		///     ERuntimeErrorCode.NotUpToData        -  The stored tag list must be updated.
		///     ERuntimeErrorCode.WrongArgument      -  The expected type is UNSPECIFIC.
		void Write (string in_Tag, SDataValue in_Value);
		/// See: <Write>
		void WriteBool (string in_Tag, bool in_Value);
		/// See: <Write>
		void WriteChar (string in_Tag, sbyte in_Value);
		/// See: <Write>
		void WriteDouble (string in_Tag, double in_Value);
		/// See: <Write>
		void WriteFloat (string in_Tag, float in_Value);
		/// See: <Write>
		void WriteInt16 (string in_Tag, short in_Value);
		/// See: <Write>
		void WriteInt32 (string in_Tag, int in_Value);
		/// See: <Write>
		void WriteInt64 (string in_Tag, long in_Value);
		/// See: <Write>
		void WriteInt8 (string in_Tag, sbyte in_Value);
		/// See: <Write>
		void WriteSignals (SDataValueByName[] in_Signals);
		/// See: <Write>
		void WriteUInt16 (string in_Tag, ushort in_Value);
		/// See: <Write>
		void WriteUInt32 (string in_Tag, uint in_Value);
		/// See: <Write>
		void WriteUInt64 (string in_Tag, ulong in_Value);
		/// See: <Write>
		void WriteUInt8 (string in_Tag, byte in_Value);
		/// See: <Write>
		void WriteWChar (string in_Tag, char in_Value);
		
		/// Sets or returns the communication interface of the virtual controller: Local communication (Softbus) or TCPIP.
		///
		/// A change of communication interface occurs only when the controller is restarted.
		///
		/// All instances that are started must use the same communication interface.
		///
		/// PowerOn is prevented if a communication interface that is not used by the started instances is selected.
		ECommunicationInterface CommunicationInterface { get; set; }

		/// Returns a configured IP address of the instance.
		string [] ControllerIP { get; }

		/// Returns the IP suite instance. If the "Softbus" communication interface is used, the subnet mask and default gateway are 0.
		SIPSuite4[] ControllerIPSuite4 { get; }

		/// Returns the downloaded name of the virtual controller.
		string ControllerName { get; }

		/// Returns the downloaded short designation of the virtual controller.
		string ControllerShortDesignation { get; }

		/// Returns or sets the CPU type of the virtual controller. 
		///
		/// A change of CPU type occurs only when the controller is restarted. 
		///
		/// When a different CPU type is loaded via STEP 7 or from the Virtual Memory Card, this CPU type applies.
		ECPUType CPUType { get; set; }

		/// Returns the instance ID. The ID is assigned by Runtime Manager when the instance is registered.
		int ID { get; }

		/// Returns the instance ID. The ID is assigned by Runtime Manager when the instance is registered.
		SInstanceInfo Info { get; }

		/// Returns an interface that you use to call the .NET functions in this section.
		IIOArea InputArea { get; }
		bool IsAlwaysSendOnEndOfCycleEnabled { get; set; }

		/// Returns an interface that you use to call the .NET functions in this section.
		IIOArea MarkerArea { get; }

		/// Returns the name of the instance.
		string Name { get; }

		/// Returns or sets the operating mode of the virtual controller. 
		/// A change in the value during runtime only takes effect at the synchronization point.
		EOperatingMode OperatingMode { get; set; }

		/// Returns the operating state of the virtual controller.
		///
		/// When the operating state changes, the OnOperatingStateChanged() (Page 243) event is triggered.
		///
		/// For details about the operating state, see Data types (Page 330).
		EOperatingState OperatingState { get; }

		/// Returns an interface that you use to call the .NET functions in this section.
		IIOArea OutputArea { get; }

		/// Returns or sets the overwritten minimum cycle time in nanoseconds that is used in the SingleStep_CT and SingleStep_CPT operating modes.
		///
		/// A value between 0 and 6000000000 is valid.  The default setting is 100 ms.
		///
		/// A change in the value during runtime only takes effect at the cycle control point.
		long OverwrittenMinimalCycleTime_ns { get; set; }

		/// Sets or returns the scaling factor with which the virtual time advances.
		///
		/// Start with a small scaling factor and incrementally approach a scaling factor at which the virtual controller remains in RUN. 
		///
		/// A value between 0.01 and 100 is valid. The default setting is 1.
		///
		/// If the value is less than 1, the virtual time of the virtual controller runs X-times slower than the real time.
		///
		/// If the value is greater than 1, the virtual time of the virtual controller runs X-times faster than the real time.
		double ScaleFactor { get; set; }

		/// Returns or sets the full path of the directory in which the instance stores its retentive data. 
		///
		/// This can also be a network share. Set the path before you start the instance. 
		///
		/// A change to the path takes effect only when the controller is restarted. If no path is set, the default setting is: 
		///
		/// > (My Documents)\Siemens\Simatic\Simulation\Runtime\Persistence\(Instance Name)
		string StoragePath { get; set; }

		/// Sets or returns the virtual system time of the virtual controller. 
		///
		/// A system time between "Jan 1 1970 00:00:00:000" and "Dec 31 2200 23:59:59:999" is valid.
		DateTime SystemTime { get; set; }

		/// Returns a list of all tags.
		STagInfo[] TagInfos { get; }
		
		event Delegate_II_EREC_DT_SRICC_UINT32_UINT32_UINT32_UINT32 OnConfigurationChanged;
		event Delegate_II_EREC_DT OnConfigurationChanging;
		event Delegate_II_EREC_DT_INT64_UINT32 OnEndOfCycle;
		event Delegate_II_EREC_DT_ELT_ELM OnLedChanged;
		event Delegate_II_EREC_DT_EOS_EOS OnOperatingStateChanged;
	}

	/// The interface is used to call the "I/O access via address" functions.
	/// 
	/// In PLCSIM Advanced the complete scope of the input and output area is used (see <AreaSize>). This is also possible when no IO module is configured.
	///
	/// Inputs and outputs that are defined via configured IO modules are synchronized to the defined update of the process image partition (PIP).
	///
	/// Inputs and outputs that are not assigned to an IO module are synchronized in the cycle control point.
	///
	/// Note the following when synchronizing these inputs and outputs:
	///
	/// - Inputs can only be used as inputs. 
	///
	///     - You can write the values via the API, but values which are written via the user program (TIA Portal) are not visible in the API.
	///
	/// - Outputs can be used as output and as input.
	///
	/// You can write the values via the API and via the CPU / the user program (TIA Portal).
	///
	/// If API and user program write to the same area, the values from the API will overwrite the vales from the user program.
	public interface IIOArea {

		/// Reads an individual bit from the area.
		bool ReadBit (uint in_Offset, byte in_Bit);

		/// Reads an individual byte from the area.
		byte ReadByte (uint in_Offset);

		/// Reads a byte array from the area.
		byte [] ReadBytes (uint in_Offset, uint in_BytesToRead);

		/// Structures and fields can be emulated through signal lists and be read by using the ReadSignals() function. 
		///
		/// The function also takes into consideration the byte order (Endianness).
		///
		/// Only primitive data type signals are supported, but the function is not type-safe.
		void ReadSignals (ref SDataValueByAddress[] inout_Signals);

		/// Writes an individual bit to the area.
		void WriteBit (uint in_Offset, byte in_Bit, bool in_Value);

		/// Writes an individual byte to the area.
		void WriteByte (uint in_Offset, byte in_Value);
		/// Writes a byte array to the area.
		uint WriteBytes (uint in_Offset, byte [] in_Values);
		/// Writes a byte array to the area.
		uint WriteBytes (uint in_Offset, uint in_BytesToWrite, byte [] in_Values);

		/// Writes multiple signals within an API call.
		///
		/// The function also takes into consideration the byte order (Endianness).
		///
		/// The function supports only primitive data type signals, but it is not typical.
		void WriteSignals (SDataValueByAddress[] in_Signals);
		
		/// Returns the size of the area in bytes.
		uint AreaSize { get; }
	}

	public interface IRemoteRuntimeManager : IDisposable {
		/// Creates and returns an interface of an already registered instance of a virtual controller.
		///
		/// The instance could have been registered via the application or another application that uses the Simulation Runtime API.
		IInstance CreateInterface (int in_InstanceID);
		IInstance CreateInterface (string in_InstanceName);

		/// Closes the connection to the remote Runtime Manager. 
		///
		/// Note:
		///
		///     All applications that are connected to the remote Runtime Manager lose this connection.
		void Disconnect ();

		/// Registers a new instance of a virtual controller in Runtime Manager. Creates and returns an interface of this instance.
		IInstance RegisterCustomInstance (string in_VplcDll);
		IInstance RegisterCustomInstance (string in_VplcDll, string in_InstanceName);

		/// Registers a new instance of a virtual controller in Runtime Manager. Creates and returns an interface of this instance.
		///
		/// Parameters:
		///
		///    in_CPUType - Defines which CPU type is simulated at the start of the instance.
		///
		///        The default setting is "ECPUType.Unspecified".  When a different CPU type is loaded via STEP 7 or
		///        from the Virtual SIMATIC Memory Card, this CPU type applies.
		///
		///    in_InstanceName - Name to be assigned to the instance.
		///
		///        Every instance must have a unique name. If no name is assigned when registering a new
		///        instance, the instance is given the name `Instance_#` (# is the ID of the instance). If
		///        this name already exists, the name "Instance_#.#" is used, in which the second #
		///        is a counter that is incremented until the name is unique. The length of the name must be
		///        less than DINSTANCE_NAME_LENGTH. See Data types (Page 340).
		///
		/// Return Values:
		///
		///     If the function is successful, an interface of a virtual controller. Otherwise, a Null pointer.
		///
		/// Exceptions
		///
		///     ERuntimeErrorCode.InterfaceRemoved - The interface is disconnected from the remote Runtime Manager.
		///     ERuntimeErrorCode.Timeout          - The function does not return on time.
		///     ERuntimeErrorCode.WrongArgument    - The name is invalid.
		///     ERuntimeErrorCode.LimitReached     - There are already 16 instances regis- tered in Runtime Manager.
		///     ERuntimeErrorCode.AlreadyExists    - An instance with this name already ex- ists.
		///
		IInstance RegisterInstance ();
		/// See: <RegisterInstance>
		IInstance RegisterInstance (ECPUType in_CPUType);
		/// See: <RegisterInstance>
		IInstance RegisterInstance (ECPUType in_CPUType, string in_InstanceName);
		/// See: <RegisterInstance>
		IInstance RegisterInstance (string in_InstanceName);

		/// The event is triggered when the connection to the Remote Runtime Manager has been terminated.
		void RegisterOnConnectionLostEvent ();

		/// Unregisters the event object.
		void UnregisterOnConnectionLostEvent ();

		/// The function blocks the program until the registered event object is in the signaled state or the timeout interval is exceeded.
		bool WaitForOnConnectionLostEvent ();
		/// See: <WaitForOnConnectionLostEvent>
		bool WaitForOnConnectionLostEvent (uint in_Time_ms);

		/// Returns the IP address of the PC on which the remote Runtime Manager is running. If the function fails, the return value is 0.
		SIP IP { get; }

		/// Returns the open port of the PC on which the remote Runtime Manager is running. If the function fails, the return value is 0.
		ushort Port { get; }

		/// Returns information about an already registered instance.
		///
		/// You can use the ID or name of this instance to create an interface of this instance, see <CreateInterface>.
		SInstanceInfo[] RegisteredInstanceInfo { get; }

		/// Returns the name of the PC on which the remote Runtime Manager is running.
		string RemoteComputerName { get; }

		/// Returns the version of Runtime Manager. If the function fails, version 0.0 is returned.
		uint Version { get; }
		
		event Delegate_IRRTM OnConnectionLost;
	}

	public sealed class RuntimeConstants {
		
		public RuntimeConstants ();

		public const int InstanceNameLength = 64;
		public const int StoragePathMaxLength = 130;
		public const int TagNameMaxLength = 300;
		public const int TagArrayDimension = 6;
		public const int ControllerNameMaxLength = 128;
		public const int ControllerShortDesignationMaxLength = 32;
		public const string ApiDllNameX86 = "Siemens.Simatic.Simulation.Runtime.Api.x86.dll";
		public const string ApiDllNameX64 = "Siemens.Simatic.Simulation.Runtime.Api.x64.dll";
		public const string ApiDllPathRegistryKeyName = "SOFTWARE\\Wow6432Node\\Siemens\\Shared Tools\\PLCSIMADV_SimRT\\";
		public const string ApiDllPathRegistryValueName = "Path";
		public const string ApiDllPathVersionSubdirectory = "API\\1.0";
	}

	/// This structure contains the IP address and port of a TCP/IP connection.
	public struct SConnectionInfo {
		public SIP IP;
		public ushort Port;
	}

	/// This structure contains read/write data record information and data records.
	public struct SDataRecord {
		/// The data record information, see <SDataRecordInfo>
		public SDataRecordInfo Info;

		/// The array length of the user data DDATARECORD_MAX_SIZE
		public byte [] Data;
	}

	/// This structure contains read/write data record information.
	public struct SDataRecordInfo {

		/// The ID of the hardware module (hardware identifier)
		public uint HardwareId;

		/// The data record number
		public uint RecordIdx;

		/// The data record size
		public uint DataSize;
	}

	/// The structure contains the value and type of a PLC tag.
	public struct SDataValue {
		
		public string ToString ();
		
		public bool Bool { get; set; }
		public sbyte Char { get; set; }
		public double Double { get; set; }
		public float Float { get; set; }
		public short Int16 { get; set; }
		public int Int32 { get; set; }
		public long Int64 { get; set; }
		public sbyte Int8 { get; set; }
		public EPrimitiveDataType Type { get; set; }
		public ushort UInt16 { get; set; }
		public uint UInt32 { get; set; }
		public ulong UInt64 { get; set; }
		public byte UInt8 { get; set; }
		public char WChar { get; set; }
	}

	/// This structure represents a PLC tag that is accessed via its address.
	public struct SDataValueByAddress {
		
		public string ToString ();
		
		public uint Offset;
		public byte Bit;
		public SDataValue DataValue;
	}

	/// This structure represents a PLC tag that is called by its name.
	public struct SDataValueByName {
		
		public string ToString ();
		
		public string Name;
		public SDataValue DataValue;
	}

	/// This structure contains information about the dimension of a field.
	public struct SDimension {
		
		public int StartIndex;
		public uint Count;
	}

	public class SimulationInitializationException : Exception {
		
		public SimulationInitializationException (ERuntimeErrorCode in_ErrorCode);
		public SimulationInitializationException (string in_Message, Exception in_Inner);
		public SimulationInitializationException (string in_Message);
		public SimulationInitializationException ();
		
		public ERuntimeErrorCode RuntimeErrorCode { get; }
	}

	public class SimulationRuntimeException : Exception {
		
		public SimulationRuntimeException (ERuntimeErrorCode in_ErrorCode);
		public SimulationRuntimeException (string in_Message, Exception in_Inner);
		public SimulationRuntimeException (string in_Message);
		public SimulationRuntimeException ();
		
		public ERuntimeErrorCode RuntimeErrorCode {
			get;
		}
	}


	/// ISimulationRuntimeManager Interface of the Runtime Manager.
	///
	/// It is used to register new Runtime instances, to search through existing Runtime instances, and to receive an interface of a registered instance.
	///
	/// Up to 16 instances can be registered in one Runtime Manager.
	public sealed class SimulationRuntimeManager {
		
		public SimulationRuntimeManager ();

		/// Closes an open port and all open connections that another Runtime Manager has created to this open port.
		public static void ClosePort ();

		/// Creates and returns an interface of an already registered instance of a virtual controller.
		///
		/// The instance could have been registered via the application or another application that uses the Simulation Runtime API.
		public static IInstance CreateInterface (int in_InstanceID);
		public static IInstance CreateInterface (string in_InstanceName);

		/// Opens a port to which another Runtime Manager can connect.
		public static void OpenPort (ushort in_Port);

		/// See: <RegisterInstance>
		public static IInstance RegisterCustomInstance (string in_VplcDll);
		/// See: <RegisterInstance>
		public static IInstance RegisterCustomInstance (string in_VplcDll, string in_InstanceName);

		/// Registers a new instance of a virtual controller in Runtime Manager. Creates and returns an interface of this instance.
		///
		/// Parameters:
		///
		///    in_CPUType - Defines which CPU type is simulated at the start of the instance. 
		///
		///                 The default setting is "ECPUType.Unspecified".  When a different CPU type is loaded via STEP 7 or
		///                 from the Virtual SIMATIC Memory Card, this CPU type applies.
		///
		///    in_InstanceName - Name to be assigned to the instance.
		///
		///                      Every instance must have a unique name. If no name is assigned when registering a new
		///                      instance, the instance is given the name "Instance_#" (# is the ID of the instance). If
		///                      this name already exists, the name "Instance_#.#" is used, in which the second #
		///                      is a counter that is incremented until the name is unique. The length of the name must be
		///                      less than DINSTANCE_NAME_LENGTH. See Data types (Page 340).
		///
		/// Return Values:
		///
		///     If the function is successful, an interface of a virtual controller. Otherwise, a Null pointer.
		///
		/// Exceptions
		///
		///     ERuntimeErrorCode.InterfaceRemoved - The interface is disconnected from the remote Runtime Manager.
		///     ERuntimeErrorCode.Timeout          - The function does not return on time.
		///     ERuntimeErrorCode.WrongArgument    - The name is invalid.
		///     ERuntimeErrorCode.LimitReached     - There are already 16 instances regis- tered in Runtime Manager.
		///     ERuntimeErrorCode.AlreadyExists    - An instance with this name already ex- ists.
		///
		public static IInstance RegisterInstance ();
		/// See: <RegisterInstance>
		public static IInstance RegisterInstance (ECPUType in_CPUType);
		/// See: <RegisterInstance>
		public static IInstance RegisterInstance (ECPUType in_CPUType, string in_InstanceName);
		/// See: <RegisterInstance>
		public static IInstance RegisterInstance (string in_InstanceName);

		/// When the event occurs, the registered event object is set to the signaled state.
		///
		/// Only one event object can be registered for the event.
		///
		/// Registration of a new event object causes the previous event object to be deleted.
		public static void RegisterOnConfigurationChangedEvent ();
		/// See: <RegisterOnConfigurationChangedEvent>
		public static void RegisterOnRunTimeManagerLostEvent ();

		/// Creates a new connection to a remote Runtime Manager or uses an existing connection to create an <IRemoteRuntimeManager> interface.
		///
		/// Parameters:
		///
		///     in_IP3 - First part of the IP address of the remote PC.
		///     in_IP2 - Second part of the IP address of the remote PC.
		///     in_IP1 - Third part of the IP address of the remote PC.
		///     in_IP0 - Last part of the IP address of the remote PC.
		///     in_IP - IP address of the remote PC.
		///     in_Port - Port number
		public static IRemoteRuntimeManager RemoteConnect (byte in_IP3, byte in_IP2, byte in_IP1, byte in_IP0, ushort in_Port);

		/// See: <RemoteConnect>
		///
		/// Parameters:
		///
		///     in_IP - IP address of the remote PC.
		///     in_Port - Port number
		public static IRemoteRuntimeManager RemoteConnect (SIP in_IP, ushort in_Port);

		/// See: <RemoteConnect>
		public static IRemoteRuntimeManager RemoteConnect (string in_ConnectionString);

		/// Ends communication with Runtime Manager and clears the interfaces.
		///
		/// Call this function in the following cases:
		///
		/// - Immediately before the API library (DLL) is unregistered (native C++).
		///
		/// - When your application is no longer using Runtime Manager.
		public static void Shutdown ();

		/// Unregisters the callback function. When the event occurs, no callback function is called.
		public static void UnregisterOnConfigurationChangedEvent ();
		/// See: <UnregisterOnConfigurationChangedEvent>
		public static void UnregisterOnRunTimeManagerLostEvent ();

		/// The function blocks the program until the registered event object is set to the signaled state or the timeout interval is exceeded.
		public static bool WaitForOnConfigurationChangedEvent ();
		/// See: <WaitForOnConfigurationChangedEvent>
		public static bool WaitForOnConfigurationChangedEvent (uint in_Time_ms);
		/// See: <WaitForOnConfigurationChangedEvent>
		public static bool WaitForOnRunTimeManagerLostEvent ();
		/// See: <WaitForOnConfigurationChangedEvent>
		public static bool WaitForOnRunTimeManagerLostEvent (uint in_Time_ms);
		
		/// Returns a value that indicates whether the API was successfully initialized.
		public static bool IsInitialized { get; }

		/// The function returns false when the connection to Runtime Manager is interrupted. This happens only when the Runtime Manager process is closed.
		public static bool IsRuntimeManagerAvailable { get; }

		/// Returns the open port. If no port is open or the function fails, the return value is 0.
		public static ushort Port { get; }

		/// Returns information about an already registered instance.
		///
		/// You can use the ID or name of this instance to create an interface of this instance, see CreateInterface().
		public static SInstanceInfo[] RegisteredInstanceInfo { get; }

		/// Returns an array of information about all open connections.
		public static SConnectionInfo[] RemoteConnectionInfo { get; }

		/// Returns the version of Runtime Manager. If the function fails, version 0.0 is returned.
		public static uint Version { get; }
		
		public event Delegate_SRCC_UINT32_UINT32_INT32 OnConfigurationChanged;
		public event Delegate_Void OnRunTimemanagerLost;
	}

	public class SimulationRuntimeWarning : Exception {
		
		public SimulationRuntimeWarning (ERuntimeErrorCode in_ErrorCode);
		public SimulationRuntimeWarning (string in_Message, Exception in_Inner);
		public SimulationRuntimeWarning (string in_Message);
		public SimulationRuntimeWarning ();
		
		public ERuntimeErrorCode RuntimeErrorCode { get; }
	}

	/// This structure contains runtime instance info
	public struct SInstanceInfo {
		
		public string ToString ();
		
		/// The ID of the instance
		public int ID;
		/// The name of the instance
		public string Name;
	}

	/// This structure contains an IPv4 address.
	public struct SIP {
		
		public string ToString ();
		
		public byte [] IPArray { get; set; }
		public uint IPDWord { get; set; }
		public string IPString { get; set; }
	}

	/// This structure contains an IPv4 suite.
	public struct SIPSuite4 {
		
		public SIPSuite4 (byte [] in_IPAddress, byte [] in_SubnetMask, byte [] in_DefaultGateway);
		public SIPSuite4 (string in_IPAddress, string in_SubnetMask, string in_DefaultGateway);
		public SIPSuite4 (uint in_IPAddress, uint in_SubnetMask, uint in_DefaultGateway);
		
		public string ToString ();
		
		/// The IP address
		public SIP IPAddress;

		/// The subnet mask
		public SIP SubnetMask;

		/// The standard gateway
		public SIP DefaultGateway;
	}

	/// This structure contains information about a PLC tag.
	public struct STagInfo {

		public string ToString ();

		/// The name of the tag
		public string Name;

		/// The CPU area where the tag is located.
		public EArea Area;

		/// The CPU data type of the tag
		public EDataType DataType;

		/// The primitive data type of the tag
		public EPrimitiveDataType PrimitiveDataType;

		/// The size of the tag in bytes
		public ushort Size;

		/// The byte offset of the tag if it is not located in a data block.
		public uint Offset;

		/// The bit offset of the tag if it is not located in a data block.
		public byte Bit;

		/// The index of the tag
		public uint Index;

		/// If this tag is embedded in another tag (for example, an element of a structure), this value then displays the index of the parent tag. 
		///
		/// The value is 0 if the tag has no parent tag.
		public uint ParentIndex;

		/// Information about each dimension of the field
		public SDimension[] Dimension;
	}
}
