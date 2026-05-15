using System.Device.Gpio;
using System.Device.I2c;

GpioController _gpioController = new();
CutilloRigby.Devices.TCA8418.Tca8418Configuration _tca8418Configuration= new(17, 27);

const byte columns = 10;
const byte rows = 8;

CancellationTokenSource cancellationTokenSource = new();
Console.CancelKeyPress += (s, e) =>
{
    cancellationTokenSource.Cancel();
    e.Cancel = true;    
};

using CutilloRigby.Devices.TCA8418.Tca8418 _tca8418 = CutilloRigby.Devices.TCA8418.Tca8418.Create(_gpioController, _tca8418Configuration);
CancellationToken cancellationToken = cancellationTokenSource.Token;
while(!cancellationToken.IsCancellationRequested )
{
    Console.WriteLine("Resetting");
    await _tca8418.Reset(cancellationTokenSource.Token);
    _tca8418.Matrix(rows, columns);

    CancellationTokenSource timedCancellation = new(10000);
    while(!timedCancellation.IsCancellationRequested)
    {
        try
        {
            if (_tca8418.EventsAvailable() == 0)
            {
                await Task.Delay(50, timedCancellation.Token);
                continue;
            }

            do
            {
                byte key = _tca8418.GetEvent();
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
            } while(_tca8418.EventsAvailable() > 0);
        }
        catch (OperationCanceledException ocex)
        {
            Console.WriteLine(ocex.Message);
        }
    }
}

_tca8418.Dump(Console.WriteLine);