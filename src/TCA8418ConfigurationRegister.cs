namespace CutilloRigby.Devices.Tca8418;

[Flags]
public enum TCA8418ConfigurationRegister : byte
{
    AI = 0x80,           ///< Auto-increment for read/write
    GPI_E_CGF = 0x40,    ///< Event mode config
    OVR_FLOW_M = 0x20,   ///< Overflow mode enable
    INT_CFG = 0x10,      ///< Interrupt config
    OVR_FLOW_IEN = 0x08, ///< Overflow interrupt enable
    K_LCK_IEN = 0x04,    ///< Keypad lock interrupt enable
    GPI_IEN = 0x02,      ///< GPI interrupt enable
    KE_IEN = 0x01,       ///< Key events interrupt enable
}
