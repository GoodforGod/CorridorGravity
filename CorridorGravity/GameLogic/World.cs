using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.GameLogic
{
    class World
    {
        private World()
        {
            MAX_X_AXIS_COORDINATE = 1024;
            MAX_Y_AXIS_COORDINATE = 1024;
        }

        private World(ContentManager content)
        {
            MAX_X_AXIS_COORDINATE = 1024;
            MAX_Y_AXIS_COORDINATE = 1024;
            WorldSprite = content.Load<Texture2D>("World");
        }

        private World(ContentManager content, string contentName)
        {
            MAX_X_AXIS_COORDINATE = 1024;
            MAX_Y_AXIS_COORDINATE = 1024;
            WorldSprite = content.Load<Texture2D>(contentName);
        }

        public int MAX_X_AXIS_COORDINATE { get; }

        public int MAX_Y_AXIS_COORDINATE { get; }

        public Texture2D WorldSprite { get; }
    }
}
