using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.GameLogic
{
    public abstract class Entity
    {
        abstract public Texture2D EntitySprite { get; }

        public virtual float X { get; set; }

        public virtual float Y { get; set; }

        public virtual void Init() { }

        public virtual void Draw() { }

        public virtual void Draw(SpriteBatch bather) { }

        public virtual void Update() { }

        public virtual void Update(GameTime gameTime) { }
    }
}
