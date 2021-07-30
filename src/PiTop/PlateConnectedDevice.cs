using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Pocket;

namespace PiTop
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PlateConnectedDevice : IDisposable {
        /// <summary>
        /// Custom properties to use at display time.
        /// </summary>
        public ICollection<DisplayPropertyBase> DisplayProperties { get; } =
            new List<DisplayPropertyBase>();

        private readonly CompositeDisposable _disposables = new();

        /// <summary>
        /// Establishes a connection on the provided port.
        /// </summary>
        /// <param name="port">The port to connect to.</param>
        /// <exception cref="Exception"></exception>
        public void Connect(PlatePort port)
        {
            if (port is null)
            {
                throw new ArgumentNullException(nameof(port));
            }

            using var operation = Logger.Log.OnEnterAndConfirmOnExit();
            try
            {
                operation.Info($"{GetType().Name} on port {port.Name}");
                port.Device = this;
                Port = port;
                OnConnection();
                operation.Succeed();
            }
            catch (PlatePortInUseException pe)
            {

                operation.Fail(pe);
                throw;
            }
            catch (Exception e)
            {

                operation.Fail(e);
                Port = null;
                port.Device = null;
                throw;
            }
        }

        /// <summary>
        /// Called when on Connection.
        /// </summary>
        /// <returns></returns>
        protected abstract void OnConnection();

        /// <summary>
        /// Currently used port.
        /// </summary>
        public PlatePort? Port { get; private set; }

        /// <summary>
        /// Add to disposable.
        /// </summary>
        /// <param name="disposable"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected void AddToDisposables(IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }
            _disposables.Add(disposable);
        }


        /// <summary>
        /// Add to disposable.
        /// </summary>
        /// <param name="disposable"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected void AddToDisposables(Action disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            AddToDisposables(Disposable.Create(disposable));
        }


        /// <inheritdoc />
        public void Dispose()
        {
            _disposables.Dispose();
            if (Port != null)
            {
                Port.Device = null;
            }
        }
    }
}