using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Point _previousPosition;
        private bool _hold;

        public Canvas(
            SpriteBatch spriteBatch,
            GraphicsDevice graphicsDevice,
            IP2P network,
            Rectangle size, Color brushColor)
        {
            _spriteBatch = spriteBatch;
            _network = network;
            _size = size;
            _brushColor = brushColor;
            _pixels = new Color[_size.Width * _size.Height];
            _canvasTexture = new Texture2D(graphicsDevice, _size.Width, _size.Height);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            var newPixel = new Pixel
            {
                X = mouseState.X,
                Y = mouseState.Y
            };

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                newPixel.Color = _brushColor;
                newPixel.Size = 6;
            }
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                newPixel.Color = Color.Transparent;
                newPixel.Size = 20;
            }
            else
            {
                _hold = false;
                return;
            }

            var pixels = new List<Pixel>();
            if (_hold && PointsAreDifferent(mouseState.Position))
            {
                Trace.WriteLine($"{mouseState.Position.X}{mouseState.Position.Y}");
                foreach (var (x, y) in Tools.GetPointsOnLine(
                    newPixel.X,
                    newPixel.Y,
                    _previousPosition.X,
                    _previousPosition.Y))
                {
                    pixels.Add(new Pixel
                    {
                        X = x,
                        Y = y,
                        Color = newPixel.Color,
                        Size = newPixel.Size
                    });
                }
            }

            pixels.Add(newPixel);
            pixels.ForEach(x =>
            {
                AddPixel(x);
                _network.Broadcast.SendData(x);
            });

            _hold = true;
            _previousPosition = mouseState.Position;
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

        private bool PointsAreDifferent(Point newPoint)
        {
            var (x, y) = newPoint;
            return x - _previousPosition.X != 0 || y - _previousPosition.Y != 0;
        }
    }
}