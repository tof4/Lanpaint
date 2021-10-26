using System;
using System.Diagnostics;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanpaint.Elements;
using Lanpaint.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lanpaint
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private P2P _network;
        private Rectangle _size;
        private Canvas _canvas;
        private Chat _chat;
        private DebugLog _debugLog;
        private KeyboardInput _keyboardInput;
        private PaperSoccer _paperSoccer;
        private SpriteBatch _spriteBatch;

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

            var rsaDatabase = new RsaDatabase();
            _network = new P2P(
                Program.Config,
                rsaDatabase,
                x => { x.Instance.Messaging.MessageReceived += _chat.MessagingOnMessageReceived; },
                new[] { typeof(PixelHandler) }
            );


            base.Initialize();
        }


        protected override void LoadContent()
        {
            var defaultFont = Content.Load<SpriteFont>("DefaultFont");
            var board = Content.Load<Texture2D>("board");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _debugLog = new DebugLog(_spriteBatch, defaultFont);
            _chat = new Chat(_spriteBatch, Window, _network, defaultFont, _size);
            _paperSoccer = new PaperSoccer(_spriteBatch, board);
            _keyboardInput = new KeyboardInput(_debugLog, _paperSoccer);
            _canvas = new Canvas(_spriteBatch, GraphicsDevice, _network, _size);

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
            _canvas.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _paperSoccer.Draw();
            _canvas.Draw();
            _chat.Draw();
            _debugLog.Draw();
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}