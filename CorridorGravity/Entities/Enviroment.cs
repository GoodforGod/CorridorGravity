using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CorridorGravity.Entities
{
    /// <summary>
    /// # DEPRICATED #
    /// </summary>
    class Enviroment : Entity
    {
        public Enviroment(World world, Vector2 position, Texture2D sprite) : base(world, position, sprite) { }
    }
}
