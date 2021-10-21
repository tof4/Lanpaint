using Lanchat.Core.Api;
using Lanpaint.Models;

namespace Lanpaint.Handlers
{
    public class PixelHandler : ApiHandler<Pixel>
    {
        protected override void Handle(Pixel data)
        {
            Main.Pixels.Add(data);
        }
    }
}