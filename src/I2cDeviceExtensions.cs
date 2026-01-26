using CutilloRigby.Devices.TCA8418;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Device.I2c;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class I2cDeviceExtensions
{
    public static byte ReadRegister(this I2cDevice i2cDevice, Tca8418Register reg) 
    {
        Span<byte> writeBuffer = [(byte)reg];
        Span<byte> readBuffer = stackalloc byte[1];
        i2cDevice.WriteRead(writeBuffer, readBuffer);
        Thread.Sleep(1);
        return readBuffer[0];
    }

    public static void WriteRegister(this I2cDevice i2cDevice, Tca8418Register reg, byte value) 
    {
        Span<byte> buffer = [(byte)reg, value];
        i2cDevice.Write(buffer);
        Thread.Sleep(1);
    }
}
