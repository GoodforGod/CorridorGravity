using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.GameLogic
{
    public abstract class Entity
    {
        public Entity() { }

        public Entity(ContentManager content) { }

        public Entity(ContentManager content, string contentName) { }

        abstract public Texture2D EntitySprite { get; }

        abstract public float X { get; set; }

        abstract public float Y { get; set; }

        public virtual void Init() { }

        public virtual void Draw() { }

        public virtual void Draw(SpriteBatch bather) { }

        public virtual void Update() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Touch() { }
    }
}
