using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.GameLogic
{
    class WorldEntity : Entity
    {
        public int WORLD_WIDTH { get; }
        public int WORLD_HEIGHT { get; }

        public override Texture2D EntitySprite { get; }

        Rectangle WorldRectangle { get; }

        public WorldEntity(ContentManager content)
        {
            WORLD_WIDTH = 1024;
            WorldRectangle = new Rectangle(0, 0, WORLD_WIDTH, WORLD_HEIGHT);
            EntitySprite = content.Load<Texture2D>("world-background");
        }

        public WorldEntity(ContentManager content, int width, int height)
        {
            WORLD_WIDTH = width;
            WORLD_HEIGHT = height;
            WorldRectangle = new Rectangle(0, 0, WORLD_WIDTH, WORLD_HEIGHT);
            EntitySprite = content.Load<Texture2D>("world-background");
        }

        public WorldEntity(ContentManager content, string contentName)
        {
            WORLD_HEIGHT = 1024;
            WorldRectangle = new Rectangle(0, 0, WORLD_WIDTH, WORLD_HEIGHT);
            EntitySprite = content.Load<Texture2D>(contentName);
        }

        public WorldEntity(ContentManager content, string contentName, int width, int height)
        {
            WORLD_WIDTH = width;
            WORLD_HEIGHT = height;
            WorldRectangle = new Rectangle(0, 0, WORLD_WIDTH, WORLD_HEIGHT);
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
