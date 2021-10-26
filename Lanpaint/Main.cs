using System;
using System.Diagnostics;
using System.Net;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanpaint.Elements;
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
        private static readonly Color Color = RandomColor.GetColor();
        private static Texture2D _canvas;
        private static Rectangle _size;
        private static Color[] _pixels;
        private readonly GraphicsDeviceManager _graphics;
        private DebugLog _debugLog;
        private Board _board;
        private KeyboardInput _keyboardInput;
        private Chat _chat;
        private SpriteBatch _spriteBatch;
        private Drawing _drawing;

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

            _size = GraphicsDevice.PresentationParameters.Bounds;
            _pixels = new Color[_size.Width * _size.Height];

            var rsaDatabase = new RsaDatabase();
            _network = new P2P(
                Program.Config,
                rsaDatabase,
                x =>
                {
                    x.Instance.Messaging.MessageReceived += _chat.MessagingOnMessageReceived;
                },
                new[] { typeof(PixelHandler) }
            );

            _canvas = new Texture2D(
                _graphics.GraphicsDevice,
                _size.Width,
                _size.Height);

            _keyboardInput = new KeyboardInput();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            var boardImage = Content.Load<Texture2D>("board");
            var font = Content.Load<SpriteFont>("DefaultFont");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugLog = new DebugLog(_spriteBatch, font);
            _board = new Board(_spriteBatch, boardImage);

            _drawing = new Drawing(_spriteBatch, font, _size);
            _chat = new Chat(Window, _drawing, _network);

            Trace.Listeners.Add(new TraceListener(_debugLog));
            try
            {
                _network.Start();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardInput.UpdateState();

            if (_keyboardInput.CheckKey(Keys.F1))
            {
                _debugLog.Show = !_debugLog.Show;
            }
            else if (_keyboardInput.CheckKey(Keys.F2))
            {
                _board.Show = !_board.Show;
            }

            AddPixel();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _canvas.SetData(_pixels, 0, _drawing.Size.Width * _drawing.Size.Height);
            _board.Draw();
            _spriteBatch.Draw(_canvas, new Rectangle(0, 0, _drawing.Size.Width, _drawing.Size.Height), Color.White);
            _chat.Draw();
            _debugLog.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void DrawPixel(Pixel pixel)
        {
            var offset = pixel.Size / 2;

            for (var x = 0 - offset; x < offset; x++)
            {
                for (var y = 0 - offset; y < offset; y++)
                {
                    var index = (pixel.Y + y) * _size.Width + (pixel.X + x);
                    if (index >= 0 && index < _pixels.Length)
                    {
                        _pixels[index] = pixel.Color;
                    }
                }
            }
        }

        private void AddPixel()
        {
            var mouseState = Mouse.GetState();
            var draw = new Pixel
            {
                X = mouseState.X,
                Y = mouseState.Y,
            };

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                draw.Color = Color;
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

            DrawPixel(draw);
            _network.Broadcast.SendData(draw);
        }
    }
}