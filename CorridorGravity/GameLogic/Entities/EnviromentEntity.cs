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

        public EnviromentEntity(ContentManager content, int levelHeight, int levelWidth )
        {
            EntitySprite = content.Load<Texture2D>("pill-white");
            ConstractCommonParts(levelHeight, levelWidth );
        }

        public EnviromentEntity(ContentManager content, string contentName, int levelHeight, int levelWidth )
        {
            EntitySprite = content.Load<Texture2D>(contentName);
            ConstractCommonParts(levelHeight, levelWidth);
        }

        private void ConstractCommonParts(int levelHeight, int levelWidth)
        {
            LevelHeight = levelHeight/2;
            LevelWidth = levelWidth/2; 
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

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch batcher)
        {
            SpriteEffects effectsApplyed = SpriteEffects.None;

            if (EntityDirection)
                effectsApplyed = SpriteEffects.FlipHorizontally;

            var rotationAngle = .0f;
            /*
            switch (LEVEL_DIMENTION)                             // Choose sprite flip, depend on the current dimention
            {
                case 0:
                    rotationAngle = .0f;
                    if (!EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                case 1:
                    rotationAngle = 1.571f;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 2:
                    rotationAngle = .0f;
                    if (!EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    else effectsApplyed = SpriteEffects.FlipVertically;
                    break;

                case 3:
                    rotationAngle = 1.571f;
                    if (EntityDirection)
                        effectsApplyed = SpriteEffects.FlipHorizontally;
                    else effectsApplyed = SpriteEffects.None;
                    break;

                default: break;
            }
            */
            batcher.Draw(EntitySprite, new Vector2(X, Y), new Rectangle(0, 0, EntitySprite.Width, EntitySprite.Height), TintColor,
                                        rotationAngle, new Vector2(1, 1), 0.5f, effectsApplyed, .0f); 
        }

        public override void Touch()
        {
            base.Touch();
        }
    }
}
