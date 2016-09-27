using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.GameLogic
{
    class WorldEntity : Entity
    {
        public int LevelWidth { get; set; }
        public int LevelHeight { get; set; }

        public override Texture2D EntitySprite { get; }

        Rectangle WorldRectangle { get; }

        public WorldEntity(ContentManager content)
        {
            LevelWidth = 1024;
            WorldRectangle = new Rectangle(0, 0, LevelWidth, LevelHeight);
            EntitySprite = content.Load<Texture2D>("world-background");
        }

        public WorldEntity(ContentManager content, int width, int height)
        {
            LevelWidth = width;
            LevelHeight = height;
            WorldRectangle = new Rectangle(0, 0, LevelWidth, LevelHeight);
            EntitySprite = content.Load<Texture2D>("world-background");
        }

        public WorldEntity(ContentManager content, string contentName)
        {
            LevelHeight = 1024;
            WorldRectangle = new Rectangle(0, 0, LevelWidth, LevelHeight);
            EntitySprite = content.Load<Texture2D>(contentName);
        }

        public WorldEntity(ContentManager content, string contentName, int width, int height)
        {
            LevelWidth = width;
            LevelHeight = height;
            WorldRectangle = new Rectangle(0, 0, LevelWidth, LevelHeight);
            EntitySprite = content.Load<Texture2D>(contentName);
        }

        public override void Init() { }

        public override void Draw(SpriteBatch batcher)
        {
            batcher.Draw(EntitySprite, WorldRectangle, Color.White);
        }

        public override void Update()
        {
            base.Update();
        }

    }
}
