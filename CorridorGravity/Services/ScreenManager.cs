using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using CorridorGravity.Services.Utils;

namespace CorridorGravity.Services
{
    public enum ScreenTypes
    {
        TITLES,
        GAME,
        DEAD
    }

    /// <summary>
    /// 
    /// </summary>
    class ScreenManager
    {
        protected       ContentManager  Content;

        private static  ScreenManager  _Instance;
        public static   ScreenManager   Instance
        { get {
                if (_Instance == null)
                    _Instance = new ScreenManager();
                return _Instance;
            } }

        Dictionary<ScreenTypes, Screen> Screens = new Dictionary<ScreenTypes, Screen>();

        Screen          CurrentScreen;
        GraphicsDevice  GraphDev;
        GameEngine      Core;

        private ScreenManager() { }

        public void Initialize(GraphicsDevice graphdev, Game core)
        {
            this.GraphDev = graphdev;
            this.Core = (GameEngine)core;

            Screens.Add(ScreenTypes.TITLES, new TitleScreen());
            Screens.Add(ScreenTypes.GAME,   new GameScreen());
            Screens.Add(ScreenTypes.DEAD,   new DeadScreen());

            this.CurrentScreen = Screens[ScreenTypes.TITLES];
        }

        public void LoadContent(ContentManager content)
        {
            this.Content = new ContentManager(content.ServiceProvider, content.RootDirectory);
            CurrentScreen.Initialize(GraphDev, Core);
            CurrentScreen.LoadContent(content);
        }

        public void UnloadContent()
        {
            foreach (var screen in Screens)
                screen.Value.UnloadContent();
            this.Content.Unload();
        }

        /// <summary>
        /// Set next screen as current.
        /// </summary>
        public void SwitchScreen(ScreenTypes type, params object[] param)
        {
            CurrentScreen.UnloadContent();
            CurrentScreen = Screens[type];

            if (!CurrentScreen.IsInitialized)
                CurrentScreen.Initialize(GraphDev, Core, param);

            CurrentScreen.LoadContent(Content);
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            CurrentScreen.Draw(batcher, gameTime);
        }
    }
}
