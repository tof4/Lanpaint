using System;
using Lanchat.Core.Network;
using Lanpaint.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lanpaint.Elements
{
    public class Canvas
    {
        private readonly Texture2D _canvasTexture;
        private readonly Color _brushColor;
        private readonly IP2P _network;
        private readonly Color[] _pixels;
        private readonly Rectangle _size;
        private readonly SpriteBatch _spriteBatch;

        public Canvas(
            SpriteBatch spriteBatch,
            GraphicsDevice graphicsDevice,
            IP2P network,
            Rectangle size)
        {
            _spriteBatch = spriteBatch;
            _network = network;
            _size = size;
            _pixels = new Color[_size.Width * _size.Height];
            _canvasTexture = new Texture2D(graphicsDevice, _size.Width, _size.Height);
            var random = new Random();
            _brushColor = new Color(random.Next(256), random.Next(256), random.Next(256));
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            var draw = new Pixel
            {
                X = mouseState.X,
                Y = mouseState.Y
            };

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                draw.Color = _brushColor;
                draw.Size = 5;
            }
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                draw.Color = Color.Transparent;
                draw.Size = 20;
            }
            else
            {
                return;
            }

            AddPixel(draw);
            _network.Broadcast.SendData(draw);
        }

        public void Draw()
        {
            _canvasTexture.SetData(_pixels, 0, _size.Width * _size.Height);
            _spriteBatch.Draw(_canvasTexture, new Rectangle(0, 0, _size.Width, _size.Height), Color.White);
        }

        public void AddPixel(Pixel pixel)
        {
            var offset = pixel.Size / 2;
            for (var x = 0 - offset; x < offset; x++)
            for (var y = 0 - offset; y < offset; y++)
            {
                var index = (pixel.Y + y) * _size.Width + pixel.X + x;
                if (index >= 0 && index < _pixels.Length) _pixels[index] = pixel.Color;
            }
        }
    }
}