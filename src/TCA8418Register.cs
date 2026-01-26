namespace CutilloRigby.Devices.Tca8418;

public enum TCA8418Register : byte
{
    RESERVED = 0x00,
    CFG = 0x01,             ///< Configuration register
    INT_STAT = 0x02,        ///< Interrupt status
    KEY_LCK_EC = 0x03,      ///< Key lock and event counter
    KEY_EVENT_A = 0x04,     ///< Key event register A
    KEY_EVENT_B = 0x05,     ///< Key event register B
    KEY_EVENT_C = 0x06,     ///< Key event register C
    KEY_EVENT_D = 0x07,     ///< Key event register D
    KEY_EVENT_E = 0x08,     ///< Key event register E
    KEY_EVENT_F = 0x09,     ///< Key event register F
    KEY_EVENT_G = 0x0A,     ///< Key event register G
    KEY_EVENT_H = 0x0B,     ///< Key event register H
    KEY_EVENT_I = 0x0C,     ///< Key event register I
    KEY_EVENT_J = 0x0D,     ///< Key event register J
    KP_LCK_TIMER = 0x0E,    ///< Keypad lock1 to lock2 timer
    UNLOCK_1 = 0x0F,        ///< Unlock register 1
    UNLOCK_2 = 0x10,        ///< Unlock register 2
    GPIO_INT_STAT_1 = 0x11, ///< GPIO interrupt status 1
    GPIO_INT_STAT_2 = 0x12, ///< GPIO interrupt status 2
    GPIO_INT_STAT_3 = 0x13, ///< GPIO interrupt status 3
    GPIO_DAT_STAT_1 = 0x14, ///< GPIO data status 1
    GPIO_DAT_STAT_2 = 0x15, ///< GPIO data status 2
    GPIO_DAT_STAT_3 = 0x16, ///< GPIO data status 3
    GPIO_DAT_OUT_1 = 0x17,  ///< GPIO data out 1
    GPIO_DAT_OUT_2 = 0x18,  ///< GPIO data out 2
    GPIO_DAT_OUT_3 = 0x19,  ///< GPIO data out 3
    GPIO_INT_EN_1 = 0x1A,   ///< GPIO interrupt enable 1
    GPIO_INT_EN_2 = 0x1B,   ///< GPIO interrupt enable 2
    GPIO_INT_EN_3 = 0x1C,   ///< GPIO interrupt enable 3
    KP_GPIO_1 = 0x1D,       ///< Keypad/GPIO select 1
    KP_GPIO_2 = 0x1E,       ///< Keypad/GPIO select 2
    KP_GPIO_3 = 0x1F,       ///< Keypad/GPIO select 3
    GPI_EM_1 = 0x20,        ///< GPI event mode 1
    GPI_EM_2 = 0x21,        ///< GPI event mode 2
    GPI_EM_3 = 0x22,        ///< GPI event mode 3
    GPIO_DIR_1 = 0x23,      ///< GPIO data direction 1
    GPIO_DIR_2 = 0x24,      ///< GPIO data direction 2
    GPIO_DIR_3 = 0x25,      ///< GPIO data direction 3
    GPIO_INT_LVL_1 = 0x26,  ///< GPIO edge/level detect 1
    GPIO_INT_LVL_2 = 0x27,  ///< GPIO edge/level detect 2
    GPIO_INT_LVL_3 = 0x28,  ///< GPIO edge/level detect 3
    DEBOUNCE_DIS_1 = 0x29,  ///< Debounce disable 1
    DEBOUNCE_DIS_2 = 0x2A,  ///< Debounce disable 2
    DEBOUNCE_DIS_3 = 0x2B,  ///< Debounce disable 3
    GPIO_PULL_1 = 0x2C,     ///< GPIO pull-up disable 1
    GPIO_PULL_2 = 0x2D,     ///< GPIO pull-up disable 2
    GPIO_PULL_3 = 0x2E,     ///< GPIO pull-up disable 3
    RESERVED_2 = 0x2F

}
