namespace CutilloRigby.Devices.Tca8418;

[Flags]
public enum TCA8418KeyLockEventCounterRegister : byte
{
    K_LCK_EN = 0x40, ///< Key lock enable
    LCK_2 = 0x20,    ///< Keypad lock status 2
    LCK_1 = 0x10,    ///< Keypad lock status 1
    KLEC_3 = 0x08,   ///< Key event count bit 3
    KLEC_2 = 0x04,   ///< Key event count bit 2
    KLEC_1 = 0x02,   ///< Key event count bit 1
    KLEC_0 = 0x01,   ///< Key event count bit 0
}
