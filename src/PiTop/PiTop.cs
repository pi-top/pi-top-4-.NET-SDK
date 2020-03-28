using System;
using System.Device.I2c;

namespace PiTop
{
    public class PiTopModule : IDisposable
    {
        private readonly Client _client;

        private const int I2CBusId = 1;

        public PiTopButton UpButton { get; } = new PiTopButton();
        public PiTopButton DownButton { get; } = new PiTopButton();
        public PiTopButton SelectButton { get; } = new PiTopButton();
        public PiTopButton CancelButton { get; } = new PiTopButton();

        public PiTopModule()
        {
            _client = new Client();
            _client.MessageReceived += _client_MessageReceived;
        }

        private void _client_MessageReceived(object sender, PiTopMessage message)
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
                    break;
                case PiTopMessageId.PUB_CRITICAL_BATTERY_WARNING:
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

        public static I2cDevice CreateI2CDevice(int deviceAddress) => I2cDevice.Create(new I2cConnectionSettings(I2CBusId, deviceAddress));
        public void Dispose()
        {
            _client.MessageReceived -= _client_MessageReceived;
            _client.Dispose();
        }
    }
}