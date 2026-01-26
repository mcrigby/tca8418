using System.Device.Gpio;
using System.Device.I2c;

namespace CutilloRigby.Devices.Tca8418;

public sealed class TCA8418
{
    private readonly I2cDevice _i2CDevice;

    public TCA8418(I2cDevice i2CDevice)
    {
        _i2CDevice = i2CDevice;

        Begin();
    }

    private bool Begin() 
    {
      //  GPIO
      //  set default all GIO pins to INPUT
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_DIR_1, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_DIR_2, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_DIR_3, 0x00);

      //  add no pins to key events
      _i2CDevice.WriteRegister(TCA8418Register.GPI_EM_1, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.GPI_EM_2, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.GPI_EM_3, 0x00);

      //  set all pins to FALLING interrupts
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_INT_LVL_1, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_INT_LVL_2, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_INT_LVL_3, 0x00);

      //  add all pins to interrupts
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_INT_EN_1, 0xFF);
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_INT_EN_2, 0xFF);
      _i2CDevice.WriteRegister(TCA8418Register.GPIO_INT_EN_3, 0xFF);

      return true;
    }

    public bool Matrix(byte rows, byte columns) {
      if ((rows > 8) || (columns > 10))
        return false;

      //  MATRIX
      //  skip zero size matrix
      if ((rows != 0) && (columns != 0)) 
      {
        // setup the keypad matrix.
        byte mask = 0x00;
        for (int r = 0; r < rows; r++)
        {
          mask <<= 1;
          mask |= 1;
        }
        _i2CDevice.WriteRegister(TCA8418Register.KP_GPIO_1, mask);

        mask = 0x00;
        for (int c = 0; c < columns && c < 8; c++)
        {
          mask <<= 1;
          mask |= 1;
        }
        _i2CDevice.WriteRegister(TCA8418Register.KP_GPIO_2, mask);

        mask = 0x00;
        if (columns == 8)
            mask = 0x01;
        if (columns == 9)
            mask = 0x03;
        _i2CDevice.WriteRegister(TCA8418Register.KP_GPIO_3, mask);
      }

      return true;
    }

    public byte EventsAvailable()
    {
      byte eventCount = _i2CDevice.ReadRegister(TCA8418Register.KEY_LCK_EC);
      eventCount &= 0x0F; //  lower 4 bits only
      return eventCount;
    }

    public byte GetEvent() => _i2CDevice.ReadRegister(TCA8418Register.KEY_EVENT_A);

    public byte Flush() 
    {
      //  flush key events
      byte count = 0;
      while (GetEvent() != 0)
        count++;

      // flush gpio events
      _i2CDevice.ReadRegister(TCA8418Register.GPIO_INT_STAT_1);
      _i2CDevice.ReadRegister(TCA8418Register.GPIO_INT_STAT_2);
      _i2CDevice.ReadRegister(TCA8418Register.GPIO_INT_STAT_3);

      // clear INT_STAT register
      _i2CDevice.WriteRegister(TCA8418Register.INT_STAT, 3);

      return count;
    }

    public PinValue DigitalRead(TCA8418Pin pinnum) 
    {
      if (pinnum > TCA8418Pin.COL9)
        return 0xFF;

      TCA8418Register reg = (TCA8418Register)((byte)TCA8418Register.GPIO_DAT_STAT_1 + ((byte)pinnum / 8));
      byte mask = (byte)(1 << ((byte)pinnum % 8));

      // LEVEL  0 = LOW  other = HIGH
      byte value = _i2CDevice.ReadRegister(reg);
      if ((value & mask) == mask)
        return PinValue.High;
      
      return PinValue.Low;
    }

    public bool DigitalWrite(TCA8418Pin pinnum, PinValue value) {
      if (pinnum > TCA8418Pin.COL9)
        return false;

      TCA8418Register reg = (TCA8418Register)((byte)TCA8418Register.GPIO_DAT_OUT_1 + ((byte)pinnum / 8));
      byte mask = (byte)(1 << ((byte)pinnum % 8));

      // LEVEL  0 = LOW  other = HIGH
      byte readValue = _i2CDevice.ReadRegister(reg);
      if (value == PinValue.Low)
        readValue &= (byte)~mask;
      else
        readValue |= mask;

      _i2CDevice.WriteRegister(reg, readValue);

      return true;
    }

    public bool PinMode(TCA8418Pin pinnum, PinMode mode) 
    {
      if (pinnum > TCA8418Pin.COL9)
        return false;
      // if (mode > INPUT_PULLUP) return false; ?s

      TCA8418Register reg = (TCA8418Register)((byte)TCA8418Register.GPIO_DIR_1 + ((byte)pinnum / 8));
      byte mask = (byte)(1 << ((byte)pinnum % 8));

      // MODE  0 = INPUT   1 = OUTPUT
      byte value = _i2CDevice.ReadRegister(reg);
      if (mode == System.Device.Gpio.PinMode.Output)
        value |= mask;
      else
        value &= (byte)~mask;
      _i2CDevice.WriteRegister(reg, value);

      // PULLUP  0 = enabled   1 = disabled
      reg = (TCA8418Register)((byte)TCA8418Register.GPIO_PULL_1 + ((byte)pinnum / 8));
      value = _i2CDevice.ReadRegister(reg);
      if (mode == System.Device.Gpio.PinMode.InputPullUp)
        value &= (byte)~mask;
      else
        value |= mask;
      _i2CDevice.WriteRegister(reg, value);

      return true;
    }

    public bool PinIRQMode(TCA8418Pin pinnum, PinEventTypes eventType) 
    {
      if (pinnum > TCA8418Pin.COL9)
        return false;
      if ((eventType != PinEventTypes.Rising) && (eventType != PinEventTypes.Falling))
        return false;

      //  MODE  0 = FALLING   1 = RISING
      TCA8418Register reg = (TCA8418Register)((byte)TCA8418Register.GPIO_INT_LVL_1+ ((byte)pinnum / 8));
      byte mask = (byte)(1 << ((byte)pinnum % 8));

      byte value =_i2CDevice.ReadRegister(reg);
      if (eventType == PinEventTypes.Rising)
        value |= mask;
      else
        value &= (byte)~mask;
      _i2CDevice.WriteRegister(reg, value);

      // ENABLE INTERRUPT
      reg = (TCA8418Register)((byte)TCA8418Register.GPIO_INT_EN_1+ ((byte)pinnum / 8));
      value = _i2CDevice.ReadRegister(reg);
      value |= mask;
      _i2CDevice.WriteRegister(reg, value);

      return true;
    }

    public void EnableInterrupts() 
    {
      byte value = _i2CDevice.ReadRegister(TCA8418Register.CFG);
      value |= (byte)(TCA8418ConfigurationRegister.GPI_IEN | TCA8418ConfigurationRegister.KE_IEN);
      _i2CDevice.WriteRegister(TCA8418Register.CFG, value);
    }

    public void DisableInterrupts() 
    {
      byte value = _i2CDevice.ReadRegister(TCA8418Register.CFG);
      value &= (byte)~(TCA8418ConfigurationRegister.GPI_IEN | TCA8418ConfigurationRegister.KE_IEN);
      _i2CDevice.WriteRegister(TCA8418Register.CFG, value);
    }

    public void EnableMatrixOverflow() 
    {
      byte value = _i2CDevice.ReadRegister(TCA8418Register.CFG);
      value |= (byte)TCA8418ConfigurationRegister.OVR_FLOW_M;
      _i2CDevice.WriteRegister(TCA8418Register.CFG, value);
    }

    public void DisableMatrixOverflow() 
    {
      byte value = _i2CDevice.ReadRegister(TCA8418Register.CFG);
      value &= (byte)~TCA8418ConfigurationRegister.OVR_FLOW_M;
      _i2CDevice.WriteRegister(TCA8418Register.CFG, value);
    }

    public void EnableDebounce() 
    {
      _i2CDevice.WriteRegister(TCA8418Register.DEBOUNCE_DIS_1, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.DEBOUNCE_DIS_2, 0x00);
      _i2CDevice.WriteRegister(TCA8418Register.DEBOUNCE_DIS_3, 0x00);
    }

    public void DisableDebounce() 
    {
      _i2CDevice.WriteRegister(TCA8418Register.DEBOUNCE_DIS_1, 0xFF);
      _i2CDevice.WriteRegister(TCA8418Register.DEBOUNCE_DIS_2, 0xFF);
      _i2CDevice.WriteRegister(TCA8418Register.DEBOUNCE_DIS_3, 0xFF);
    }

    public void Dump(Action<string> output)
    {
        foreach(TCA8418Register tCA8418Register in Enum.GetValues<TCA8418Register>()
          .Where(x => x != TCA8418Register.RESERVED && x != TCA8418Register.RESERVED_2))
          output($"{tCA8418Register}: 0x{_i2CDevice.ReadRegister(tCA8418Register):X2}");
    }
}
