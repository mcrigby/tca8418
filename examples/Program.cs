using System.Device.I2c;

I2cConnectionSettings _i2CConnectionSettings;
I2cDevice _i2CDevice;
CutilloRigby.Devices.Tca8418.TCA8418 tca8418;

const int busId = 1;
const int deviceAddress = 0x34;
const byte columns = 10;
const byte rows = 8;

CancellationTokenSource cancellationTokenSource = new();
Console.CancelKeyPress += (s, e) =>
{
    cancellationTokenSource.Cancel();
    e.Cancel = true;    
};

_i2CConnectionSettings = new(busId, deviceAddress);
_i2CDevice = I2cDevice.Create(_i2CConnectionSettings);

tca8418 = new(_i2CDevice);
tca8418.Matrix(rows, columns);   
tca8418.Flush(); 

CancellationToken cancellationToken = cancellationTokenSource.Token;

while(!cancellationToken.IsCancellationRequested )
{
    if (tca8418.EventsAvailable() == 0)
    {
        await Task.Delay(50, cancellationToken);
        continue;
    }

    do
    {
        byte key = tca8418.GetEvent();
        bool pressed = (key & 0x80) == 0x80;
        byte value = (byte)((key & 0x7f) -1);
        byte row = (byte)(value / 10);
        byte column = (byte)(value % 10);
        string pressedString = pressed ? "Pressed" : "Released";

        try
        {
            Console.WriteLine($"CxR: {column}x{row}. {pressedString}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CxR: {column}x{row}. Error: {ex.Message}");
        }
    } while(tca8418.EventsAvailable() > 0);
}
