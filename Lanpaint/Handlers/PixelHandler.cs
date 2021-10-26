using Lanchat.Core.Api;
using Lanpaint.Elements;
using Lanpaint.Models;

namespace Lanpaint.Handlers
{
    public class PixelHandler : ApiHandler<Pixel>
    {
        private readonly Canvas _canvas;

        public PixelHandler(Canvas canvas)
        {
            _canvas = canvas;
        }

        protected override void Handle(Pixel pixel)
        {
            _canvas.AddPixel(pixel);
        }
    }
}