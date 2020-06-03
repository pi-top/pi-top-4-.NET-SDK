using System;
using System.Threading;
using System.Threading.Tasks;

using NetMQ;
using NetMQ.Sockets;

namespace PiTop
{
    internal class Client : IDisposable
    {
        private readonly SubscriberSocket _socket;
        private NetMQPoller _poller;
        private CancellationTokenSource _cancellationSource;
        public event EventHandler<PiTopMessage> MessageReceived;

        public Client()
        {
            _socket = new SubscriberSocket();

        }

        public void Start()
        {
            _cancellationSource = new CancellationTokenSource();
            _socket.Connect("tcp://127.0.0.1:3781");
            _socket.Subscribe("");

            _socket.ReceiveReady += SocketOnReceiveReady;
            _poller = new NetMQPoller
            {
                _socket
            };

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
            _poller.Stop();
            _poller.Dispose();
            _socket.Dispose();
        }
    }
}