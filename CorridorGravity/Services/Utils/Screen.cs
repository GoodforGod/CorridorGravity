using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CorridorGravity.Services.Utils
{
    /// <summary>
    /// Is the main ingame screen entity, provides with basic screen logic
    /// </summary>
    class Screen
    {
        /// <summary>
        /// Source rectangle for fade texture
        /// </summary>
        protected Rectangle         FSource   { get; set; }

        /// <summary>
        /// Labes texture used in all screens
        /// </summary>
        protected Texture2D         TLabels   { get; set; }
        protected KeyboardState     PState    { get; set; }

        protected ScreenManager     SManager  { get; set; }
        protected ServiceLocator    SLocator  { get; set; }
        protected ContentManager    Content   { get; set; }
        protected GraphicsDevice    GraphDev  { get; set; }
        protected Game              CoreGame  { get; set; }

        protected int   FWidth          { get; private set; }
        protected int   FHeight         { get; private set; }
        public    bool  IsInitialized   { get; private set; }
        public    bool  IsFaded         { get; set; }
        public    float Alpha           { get; set; }
        public    float FadeRatio       { get; set; }

        /// <summary>
        /// Initializes screen to its default state
        /// </summary>
        public virtual void Initialize(GraphicsDevice graphdev, Game game, params object[] param)
        {
            this.SManager = ScreenManager.Instance;
            this.SLocator = ServiceLocator.Instance;

            this.GraphDev = graphdev;
            this.CoreGame = game;

            this.FWidth = graphdev.Viewport.Width;
            this.FHeight = graphdev.Viewport.Height;

            this.Alpha = 1f;
            this.FadeRatio = .02f;
            this.IsInitialized = true;
        }

        /// <summary>
        /// Loads screens resources
        /// </summary>
        public virtual void LoadContent(ContentManager content)
        {
            this.Content = new ContentManager(content.ServiceProvider, content.RootDirectory);
            this.TLabels = Content.Load<Texture2D>(SLocator.PLManager.TRoot + SLocator.PLManager.TList[TTypes.LABELS]);
            this.FSource = SLocator.PLManager.TSources[TTypes.FADE_BLACK];

            StartFade();
        }

        /// <summary>
        /// Unloads resources
        /// </summary>
        public virtual void UnloadContent() { this.Content.Unload(); }

        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Initiates screen's fade
        /// </summary>
        protected virtual void StartFade()
        {
            FadeRatio = -FadeRatio;
            IsFaded = false;
        }

        /// <summary>
        /// Logic to controll screen fade
        /// </summary>
        protected virtual void Fade(SpriteBatch batcher, GameTime gameTime)
        {
            Alpha += FadeRatio;

            batcher.Draw(TLabels, 
                            Vector2.Zero, 
                            FSource, 
                            Color.Black * Alpha, 
                            .0f, 
                            Vector2.Zero, 
                            14f, 
                            SpriteEffects.None, 
                            .0f);
                       
            if (Alpha < 0 || Alpha > 1)
                IsFaded = true;
        }

        public virtual void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            if (!IsFaded)
                Fade(batcher, gameTime);
        }
    }
}
