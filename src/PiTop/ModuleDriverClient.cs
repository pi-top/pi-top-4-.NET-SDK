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
        private readonly SubscriberSocket _socket;
        private readonly NetMQPoller _poller;
        private readonly CancellationTokenSource _cancellationSource;
        public event EventHandler<PiTopMessage>? MessageReceived;

        public ModuleDriverClient()
        {
            _socket = new SubscriberSocket();
            _cancellationSource = new CancellationTokenSource();
            _socket.ReceiveReady += SocketOnReceiveReady;
            _poller = new NetMQPoller
            {
                _socket
            };
        }

        public void Start()
        {
            _socket.Connect("tcp://127.0.0.1:3781");
            _socket.Subscribe("");
            Task.Run(() => { _poller.Run(); }, _cancellationSource.Token);
        }

        private void SocketOnReceiveReady(object? sender, NetMQSocketEventArgs e)
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
            
            _socket.Disconnect("tcp://127.0.0.1:3781");
            _poller.RemoveAndDispose(_socket);
        }
    }
}