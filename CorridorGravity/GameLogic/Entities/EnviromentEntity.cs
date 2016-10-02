using System; 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.GameLogic
{
    class EnviromentEntity : Entity
    {
        private static Color TintColor = Color.White;

        public override float X { get; set; } 
        public override float Y { get; set; }

        private int LevelHeight { get; set; }
        private int LevelWidth { get; set; }

        private bool EntityDirection;

        public override Texture2D EntitySprite { get; }

        public EnviromentEntity(ContentManager content, string contentName, int levelHeight, int levelWidth )
        {
            EntitySprite = content.Load<Texture2D>(contentName);
            LevelHeight = levelHeight;
            LevelWidth = levelWidth;
        } 

        public Rectangle GetEnviromentSizes()
        { 
            return new Rectangle((int)X, (int)Y, EntitySprite.Width, EntitySprite.Height);
        }

        public void Init(int X, int Y, bool EntityDirection)
        {
            this.X = X;
            this.Y = Y;
            this.EntityDirection = EntityDirection;
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = SpriteEffects.None;

            if (EntityDirection)
                effectsApplyed = SpriteEffects.FlipHorizontally;

            var rotationAngle = .0f;

            batcher.Draw(EntitySprite, new Vector2(X, Y), new Rectangle(0, 0, EntitySprite.Width, EntitySprite.Height), TintColor,
                                        rotationAngle, new Vector2(1, 1), 0.5f, effectsApplyed, .0f); 
        }
    }
}
