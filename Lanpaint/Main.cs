using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private static P2P _network;
        private readonly Color _color = RandomColor.GetColor();
        private readonly GraphicsDeviceManager _graphics;
        private KeyboardInput _keyboardInput;
        private Config _config;
        private DebugLog _debugLog;
        private SpriteBatch _spriteBatch;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public static List<Pixel> Pixels { get; } = new();

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

            _keyboardInput = new KeyboardInput();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var font = Content.Load<SpriteFont>("DefaultFont");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugLog = new DebugLog(_spriteBatch, font);
            Trace.Listeners.Add(new TraceListener(_debugLog));
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardInput.UpdateState();
            if (_keyboardInput.CheckKey(Keys.F1))
            {
                _network.Start();
            }
            else if (_keyboardInput.CheckKey(Keys.F2))
            {
                _config.StartServer = false;
                _network.Start();
                _network.Connect(IPAddress.Loopback);
            }
            else if (_keyboardInput.CheckKey(Keys.F3))
            {
                _debugLog.ShowLog = !_debugLog.ShowLog;
            }

            AddPixel();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            Pixels.ToList().ForEach(DrawPixel);
            _debugLog.DrawLog();
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
                Y = mouseState.Y,
                Color = _color
            };

            Pixels.Add(draw);
            _network.Broadcast.SendData(draw);
        }

        private void DrawPixel(Pixel pixel)
        {
            var rect = new Texture2D(_graphics.GraphicsDevice, 5, 5);
            var data = new Color[5 * 5];
            for (var i = 0; i < data.Length; ++i) data[i] = pixel.Color;
            rect.SetData(data);
            var position = new Vector2(pixel.X, pixel.Y);
            _spriteBatch.Draw(rect, position, Color.White);
        }
    }
}