using Lanchat.Core.Api;
using Lanpaint.Models;

namespace Lanpaint.Handlers
{
    public class PixelHandler : ApiHandler<Pixel>
    {
        protected override void Handle(Pixel pixel)
        {
           Main.Canvas.AddPixel(pixel);
        }
    }
}