using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;
using System.Linq;
using System.Reactive.Disposables;



namespace PiTop
{
    public class PiTopModule : 
        IDisposable, 
        II2CDeviceFactory, 
        IGpioControllerFactory,
        ISPiDeviceFactory
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly ConcurrentDictionary<Type, PiTopPlate> _plates = new ConcurrentDictionary<Type, PiTopPlate>();
        private readonly ConcurrentDictionary<int, I2cDevice> _i2cBusses = new ConcurrentDictionary<int, I2cDevice>();
        private readonly ConcurrentDictionary<SpiConnectionSettings, SpiDevice> _spiDevices = new ConcurrentDictionary<SpiConnectionSettings, SpiDevice>();
        private readonly ModuleDriverClient _moduleDriverClient;
        private readonly IGpioController _controller;
        private readonly Dictionary<Type, object> _deviceFactories = new Dictionary<Type, object>();

        private const int I2CBusId = 1;

        public PiTopButton UpButton { get; } = new PiTopButton();
        public PiTopButton DownButton { get; } = new PiTopButton();
        public PiTopButton SelectButton { get; } = new PiTopButton();
        public PiTopButton CancelButton { get; } = new PiTopButton();
        public Display Display { get; set; }

        public event EventHandler<BatteryState>? BatteryStateChanged;

        public PiTopModule(): this(new GpioController().AsManaged())
        {

        }

        public PiTopModule(IGpioController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _moduleDriverClient = new ModuleDriverClient();
            _moduleDriverClient.MessageReceived += ModuleDriverClientMessageReceived;
            _disposables.Add(Disposable.Create(() =>
            {
                var plates = _plates.Values.ToList();
                foreach (var piTopPlate in plates)
                {
                    piTopPlate.Dispose();
                }

                var busses = _i2cBusses.Values.ToList();
                foreach (var i2CDevice in busses)
                {
                    i2CDevice.Dispose();
                }

                var spiDevices = _spiDevices.Values.ToList();
                foreach (var spiDevice in spiDevices)
                {
                    spiDevice.Dispose();
                }
            }));
            _moduleDriverClient.Start();
            _disposables.Add(_moduleDriverClient);
            _disposables.Add(_controller);
        }

        public T GetOrCreatePlate<T>() where T : PiTopPlate
        {
            var key = typeof(T);
            var plate = _plates.GetOrAdd(key, plateType =>
           {
               var newPlate =( Activator.CreateInstance(plateType, args: new object[] { this }) as T)!;
               newPlate.RegisterForDisposal(() => _plates.TryRemove(key, out _));
               return newPlate;
           });

            return (plate as T)!;
        }

        private void ModuleDriverClientMessageReceived(object? sender, PiTopMessage message)
        {
            switch (message.Id)
            {
                case PiTopMessageId.REQ_PING:
                    break;
                case PiTopMessageId.REQ_GET_DEVICE_ID:
                    break;
                case PiTopMessageId.REQ_GET_BRIGHTNESS:
                    break;
                case PiTopMessageId.REQ_SET_BRIGHTNESS:
                    break;
                case PiTopMessageId.REQ_INCREMENT_BRIGHTNESS:
                    break;
                case PiTopMessageId.REQ_DECREMENT_BRIGHTNESS:
                    break;
                case PiTopMessageId.REQ_BLANK_SCREEN:
                    break;
                case PiTopMessageId.REQ_UNBLANK_SCREEN:
                    break;
                case PiTopMessageId.REQ_GET_BATTERY_STATE:
                    break;
                case PiTopMessageId.REQ_GET_PERIPHERAL_ENABLED:
                    break;
                case PiTopMessageId.REQ_GET_SCREEN_BLANKING_TIMEOUT:
                    break;
                case PiTopMessageId.REQ_SET_SCREEN_BLANKING_TIMEOUT:
                    break;
                case PiTopMessageId.REQ_GET_LID_OPEN_STATE:
                    break;
                case PiTopMessageId.REQ_GET_SCREEN_BACKLIGHT_STATE:
                    break;
                case PiTopMessageId.REQ_SET_SCREEN_BACKLIGHT_STATE:
                    break;
                case PiTopMessageId.REQ_GET_OLED_CONTROL:
                    break;
                case PiTopMessageId.REQ_SET_OLED_CONTROL:
                    break;
                case PiTopMessageId.RSP_ERR_SERVER:
                    break;
                case PiTopMessageId.RSP_ERR_MALFORMED:
                    break;
                case PiTopMessageId.RSP_ERR_UNSUPPORTED:
                    break;
                case PiTopMessageId.RSP_PING:
                    break;
                case PiTopMessageId.RSP_GET_DEVICE_ID:
                    break;
                case PiTopMessageId.RSP_GET_BRIGHTNESS:
                    break;
                case PiTopMessageId.RSP_SET_BRIGHTNESS:
                    break;
                case PiTopMessageId.RSP_INCREMENT_BRIGHTNESS:
                    break;
                case PiTopMessageId.RSP_DECREMENT_BRIGHTNESS:
                    break;
                case PiTopMessageId.RSP_GET_BATTERY_STATE:
                    break;
                case PiTopMessageId.RSP_GET_PERIPHERAL_ENABLED:
                    break;
                case PiTopMessageId.RSP_GET_SCREEN_BLANKING_TIMEOUT:
                    break;
                case PiTopMessageId.RSP_SET_SCREEN_BLANKING_TIMEOUT:
                    break;
                case PiTopMessageId.RSP_GET_LID_OPEN_STATE:
                    break;
                case PiTopMessageId.RSP_GET_SCREEN_BACKLIGHT_STATE:
                    break;
                case PiTopMessageId.RSP_SET_SCREEN_BACKLIGHT_STATE:
                    break;
                case PiTopMessageId.RSP_GET_OLED_CONTROL:
                    break;
                case PiTopMessageId.RSP_SET_OLED_CONTROL:
                    break;
                case PiTopMessageId.PUB_BRIGHTNESS_CHANGED:
                    break;
                case PiTopMessageId.PUB_PERIPHERAL_CONNECTED:
                    break;
                case PiTopMessageId.PUB_PERIPHERAL_DISCONNECTED:
                    break;
                case PiTopMessageId.PUB_SHUTDOWN_REQUESTED:
                    break;
                case PiTopMessageId.PUB_REBOOT_REQUIRED:
                    break;
                case PiTopMessageId.PUB_BATTERY_STATE_CHANGED:
                    break;
                case PiTopMessageId.PUB_SCREEN_BLANKED:
                    break;
                case PiTopMessageId.PUB_SCREEN_UNBLANKED:
                    break;
                case PiTopMessageId.PUB_LOW_BATTERY_WARNING:
                    BatteryStateChanged?.Invoke(this, BatteryState.Low);
                    break;
                case PiTopMessageId.PUB_CRITICAL_BATTERY_WARNING:
                    BatteryStateChanged?.Invoke(this, BatteryState.Critical);
                    break;
                case PiTopMessageId.PUB_LID_CLOSED:
                    break;
                case PiTopMessageId.PUB_LID_OPENED:
                    break;
                case PiTopMessageId.PUB_UNSUPPORTED_HARDWARE:
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_UP_PRESSED:
                    UpButton.State = PiTopButtonState.Pressed;
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_UP_RELEASED:
                    UpButton.State = PiTopButtonState.Released;
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_DOWN_PRESSED:
                    DownButton.State = PiTopButtonState.Pressed;
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_DOWN_RELEASED:
                    DownButton.State = PiTopButtonState.Released;
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_SELECT_PRESSED:
                    SelectButton.State = PiTopButtonState.Pressed;
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_SELECT_RELEASED:
                    SelectButton.State = PiTopButtonState.Released;
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_CANCEL_PRESSED:
                    CancelButton.State = PiTopButtonState.Pressed;
                    break;
                case PiTopMessageId.PUB_V3_BUTTON_CANCEL_RELEASED:
                    CancelButton.State = PiTopButtonState.Released;
                    break;
                case PiTopMessageId.PUB_KEYBOARD_DOCKED:
                    break;
                case PiTopMessageId.PUB_KEYBOARD_UNDOCKED:
                    break;
                case PiTopMessageId.PUB_KEYBOARD_CONNECTED:
                    break;
                case PiTopMessageId.PUB_FAILED_KEYBOARD_CONNECT:
                    break;
                case PiTopMessageId.PUB_OLED_CONTROL_CHANGED:
                    break;
                case PiTopMessageId.PUB_NATIVE_DISPLAY_CONNECTED:
                    break;
                case PiTopMessageId.PUB_NATIVE_DISPLAY_DISCONNECTED:
                    break;
                case PiTopMessageId.PUB_EXTERNAL_DISPLAY_CONNECTED:
                    break;
                case PiTopMessageId.PUB_EXTERNAL_DISPLAY_DISCONNECTED:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public I2cDevice GetOrCreateI2CDevice(int deviceAddress)
        {
            return _i2cBusses.GetOrAdd(deviceAddress, address => I2cDevice.Create(new I2cConnectionSettings(I2CBusId, deviceAddress)));
        }

        public SpiDevice GetOrCreateSpiDevice(SpiConnectionSettings connectionSettings)
        {
            return _spiDevices.GetOrAdd(connectionSettings, settings => SpiDevice.Create(settings));
        }

        public void Dispose()
        {
            _moduleDriverClient.MessageReceived -= ModuleDriverClientMessageReceived;
            _disposables.Dispose();
        }

        public IGpioController GetOrCreateController()
        {
            return _controller.Share();
        }

        public IConnectedDeviceFactory<TConnectionConfiguration, TDevice> GetDeviceFactory<TConnectionConfiguration, TDevice>()
            where TConnectionConfiguration : notnull
            where TDevice : IConnectedDevice
        {
            return (IConnectedDeviceFactory<TConnectionConfiguration, TDevice>) _deviceFactories[typeof(IConnectedDeviceFactory<TConnectionConfiguration, TDevice>)];
        }

        public void AddDeviceFactory<TConnectionConfiguration, TDevice>(IConnectedDeviceFactory<TConnectionConfiguration, TDevice> connectedDeviceFactory)
            where TConnectionConfiguration : notnull
            where TDevice : IConnectedDevice
        {
            if (connectedDeviceFactory == null)
            {
                throw new ArgumentNullException(nameof(connectedDeviceFactory));
            }

            _deviceFactories.Add(typeof(IConnectedDeviceFactory<TConnectionConfiguration, TDevice>), connectedDeviceFactory);
            _disposables.Add(connectedDeviceFactory);
        }

        public void AddDeviceFactory<TConnectionConfiguration, TDevice>(Func<Type, Func<TConnectionConfiguration, TDevice>>? defaultDeviceFactoryGenerator = null)
            where TConnectionConfiguration : notnull
            where TDevice : IConnectedDevice
        {
            var deviceFactory = new ConnectedDeviceFactory<TConnectionConfiguration, TDevice>(defaultDeviceFactoryGenerator);
            AddDeviceFactory(deviceFactory);
        }
    }
}