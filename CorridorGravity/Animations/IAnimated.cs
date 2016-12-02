using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorridorGravity.Animations
{
    /// <summary>
    /// Interface for animate entities
    /// </summary>
    /// <typeparam name="TEnum"> <see cref="AnimationManager{TEnum}"/> </typeparam>
    interface IAnimated<TEnum>
        where TEnum : struct, IConvertible
    {
        /// <summary>
        /// Animation manager <see cref="AnimationManager{TEnum}"/>
        /// </summary>
        AnimationManager<TEnum> AniManager { get; set; }

        /// <summary>
        /// Loads entitie's assosiated animations from file
        /// </summary>
        /// <param name="name"> File's name to load animations from </param>
        void Load(string name);

        void Draw(SpriteBatch batcher, GameTime gameTime);
    }
}
