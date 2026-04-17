namespace CutilloRigby.Devices.TCA8418;

[Flags]
public enum Tca8418InterruptStatusRegister : byte
{
    CAD_INT = 0x10,      ///< Ctrl-alt-del seq status
    OVR_FLOW_INT = 0x08, ///< Overflow interrupt status
    K_LCK_INT = 0x04,    ///< Key lock interrupt status
    GPI_INT = 0x02,      ///< GPI interrupt status
    K_INT = 0x01,        ///< Key events interrupt status  
}
