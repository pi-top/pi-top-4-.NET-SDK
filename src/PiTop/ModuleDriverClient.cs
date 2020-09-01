using System;
using System.Threading;
using System.Threading.Tasks;

using NetMQ;
using NetMQ.Sockets;

namespace PiTop
{
    public interface IModuleDriverClient : IDisposable
    {
        event EventHandler<PiTopMessage>? MessageReceived;
        void Start();
    }

    internal class ModuleDriverClient : IModuleDriverClient
    {
        private readonly SubscriberSocket _responseSocket;
        private readonly NetMQPoller _poller;
        private readonly CancellationTokenSource _cancellationSource;
        public event EventHandler<PiTopMessage>? MessageReceived;

        public ModuleDriverClient()
        {
            _responseSocket = new SubscriberSocket();
            _cancellationSource = new CancellationTokenSource();
            _responseSocket.ReceiveReady += ResponseSocketOnReceiveReady;
            _poller = new NetMQPoller
            {
                _responseSocket
            };
        }

        public void AcquireDisplay()
        {
            using (var requestSocket = new RequestSocket())
            {
                requestSocket.Connect("tcp://127.0.0.1:3782");
                var request = new PiTopMessage(PiTop4MessageId.REQ_SET_OLED_CONTROL, "1");
                requestSocket.SendFrame(request.ToString());
            }
        }

        public void RequestBatteryState()
        {
            using (var requestSocket = new RequestSocket())
            {
                requestSocket.Connect("tcp://127.0.0.1:3782");
                var request = new PiTopMessage(PiTop4MessageId.REQ_GET_BATTERY_STATE);
                requestSocket.SendFrame(request.ToString());
            }
        }

        public void RequestDeviceId()
        {
            using (var requestSocket = new RequestSocket())
            {
                requestSocket.Connect("tcp://127.0.0.1:3782");
                var request = new PiTopMessage(PiTop4MessageId.REQ_GET_DEVICE_ID);
                requestSocket.SendFrame(request.ToString());
            }
        }

        public void ReleaseDisplay()
        {
            using (var requestSocket = new RequestSocket())
            {
                requestSocket.Connect("tcp://127.0.0.1:3782");
                var request = new PiTopMessage(PiTop4MessageId.REQ_SET_OLED_CONTROL, "0");
                requestSocket.SendFrame(request.ToString());
            }
        }

        public void Start()
        {
            _responseSocket.Connect("tcp://127.0.0.1:3781");
            _responseSocket.Subscribe("");
            Task.Run(() => { _poller.Run(); }, _cancellationSource.Token);
        }

        private void ResponseSocketOnReceiveReady(object? sender, NetMQSocketEventArgs e)
        {
            var message = PiTopMessage.Parse(e.Socket.ReceiveFrameString());
            MessageReceived?.Invoke(this, message);
        }

        public void Dispose()
        {
            _cancellationSource.Dispose();
            if (_poller.IsRunning)
            {
                _poller.Stop();
            }

            _responseSocket.Disconnect("tcp://127.0.0.1:3781");
            _poller.RemoveAndDispose(_responseSocket);
        }
    }
}