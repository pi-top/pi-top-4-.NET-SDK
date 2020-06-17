using Microsoft.Psi;
using Microsoft.Psi.Imaging;

namespace PiTop.Camera.Psi
{
    internal abstract class ImageStreamWrapper<T> : IProducer<Shared<Image>>
    {
        private readonly Emitter<Shared<Image>> _out;

        protected ImageStreamWrapper(IProducer<T> source, Pipeline pipeline)
        {
            _out = pipeline.CreateEmitter<Shared<Image>>(this, nameof(Out));
            source.Do((sourceImage, envelope) =>
            {
                using var ret = ProcessImage(sourceImage, envelope);
                _out.Post(ret, envelope.OriginatingTime);
            });

        }

        protected abstract Shared<Image>ProcessImage(T image, Envelope envelope);

        public Emitter<Shared<Image>> Out => _out;
    }
}