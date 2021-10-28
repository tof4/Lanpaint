using System;
using System.Diagnostics;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanpaint.Elements;
using Lanpaint.Handlers;
using Lanpaint.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lanpaint
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private P2P _network;
        private Rectangle _size;
        private Chat _chat;
        private DebugLog _debugLog;
        private KeyboardInput _keyboardInput;
        private PaperSoccer _paperSoccer;
        private SpriteBatch _spriteBatch;
        private Cursor _cursor;


        public static Canvas Canvas { get; private set; }

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            _size = GraphicsDevice.PresentationParameters.Bounds;

            _network = new P2P(
                Program.Config,
                new RsaDatabase(),
                x =>
                {
                    x.Instance.Messaging.MessageReceived += (_, s) =>
                    {
                        _chat.AddMessage(x.Instance.User.Nickname, s);
                    };
                },
                new[] { typeof(PixelHandler) }
            );

            base.Initialize();
        }


        protected override void LoadContent()
        {
            var defaultFont = Content.Load<SpriteFont>("DefaultFont");
            var board = Content.Load<Texture2D>("board");

            var random = new Random();
            var brushColor = new Color(random.Next(256), random.Next(256), random.Next(256));

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugLog = new DebugLog(_spriteBatch, defaultFont, GraphicsDevice, _size);
            _chat = new Chat(_spriteBatch, Window, _network, defaultFont, _size, GraphicsDevice);
            _paperSoccer = new PaperSoccer(_spriteBatch, board);
            _keyboardInput = new KeyboardInput(_chat, _debugLog, _paperSoccer);
            _cursor = new Cursor(_spriteBatch, GraphicsDevice, brushColor);
            Canvas = new Canvas(_spriteBatch, GraphicsDevice, _network, _size, brushColor);

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
            _keyboardInput.Update();
            Canvas.Update();
            _cursor.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _paperSoccer.Draw();
            Canvas.Draw();
            _chat.Draw();
            _debugLog.Draw();
            _cursor.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}