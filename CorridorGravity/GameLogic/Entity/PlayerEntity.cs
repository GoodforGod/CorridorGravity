using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CorridorGravity.GameLogic;
using Microsoft.Xna.Framework.Content;
using CorridorGravity.GameLogic.AnimatedEntity;

namespace CorridorGravity.GameLogic
{
    class PlayerEntity : Entity
    {
        public override float X { get; set; }

        public override float Y { get; set; }

        public override Texture2D EntitySprite { get; }

        private Animation CurrentAnimation { get; set; }

        private Bob SpriteAlice;

        private static Color TintColor = Color.White;

        public PlayerEntity(ContentManager content)  {
            EntitySprite = content.Load<Texture2D>("player-2-white");
        }

        public PlayerEntity(ContentManager content, string contentName) {
            EntitySprite = content.Load<Texture2D>(contentName);
        }

        public override void Init() {
            SpriteAlice = new Bob();
            CurrentAnimation = SpriteAlice.Idle;
        }

        public override void Update(GameTime gameTime) { CurrentAnimation.Update(gameTime); }

        public override void Draw(SpriteBatch bather) {
            Vector2 topLeftOfSprite = new Vector2(200,200);
            var sourceRectangle = CurrentAnimation.CurrentRectangle;
            bather.Draw(EntitySprite, topLeftOfSprite, sourceRectangle, TintColor);
        }

        public override void Touch() { base.Touch(); }
    }
}
