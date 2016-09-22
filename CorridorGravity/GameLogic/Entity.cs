using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CorridorGravity.GameLogic
{
    public abstract class Entity
    {
        public float X { get; set; }

        public float Y { get; set; }

        public virtual void Init() { }

        public virtual void Draw() { }

        public virtual void Update() { }
    }
}
