using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanpaint.Handlers;
using Lanpaint.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lanpaint
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static P2P _network;
        private Config _config;

        public static List<Pixel> Pixels { get; } = new();

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            _config = Storage.LoadConfig();
            _config.DebugMode = true;
            _config.ConnectToSaved = false;
            _config.NodesDetection = false;
            
            var rsaDatabase = new RsaDatabase();
            _network = new P2P(
                _config,
                rsaDatabase,
                x => { },
                new[] { typeof(PixelHandler) }
            );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.F1))
            {
                _network.Start();
            }
            else if (keyboardState.IsKeyDown(Keys.F2))
            {
                _config.StartServer = false;
                _network.Start();
                _network.Connect(IPAddress.Loopback);
            }

            AddPixel();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            Pixels.ToList().ForEach(DrawPixel);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        
        private void AddPixel()
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton != ButtonState.Pressed ||
                Pixels.Any(x => x.X == mouseState.X && x.Y == mouseState.Y))
            {
                return;
            }

            var draw = new Pixel
            {
                X = mouseState.X,
                Y = mouseState.Y
            };

            Pixels.Add(draw);
            _network.Broadcast.SendData(draw);
        }

        private void DrawPixel(Pixel pixel)
        {
            var rect = new Texture2D(_graphics.GraphicsDevice, 5, 5);
            var data = new Color[5 * 5];
            for (var i = 0; i < data.Length; ++i) data[i] = Color.Black;
            rect.SetData(data);
            var position = new Vector2(pixel.X, pixel.Y);
            _spriteBatch.Draw(rect, position, Color.White);
        }
    }
}