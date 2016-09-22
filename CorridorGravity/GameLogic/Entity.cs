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

        private Entity(ContentManager content) { }

        private Entity(ContentManager content, string contentName) { }

        public Texture2D EntitySprite { get; }

        public float X { get; set; }

        public float Y { get; set; }

        public virtual void Init() { }

        public virtual void Draw() { }

        public virtual void Update() { }

        public virtual void Touch() { }
    }
}
