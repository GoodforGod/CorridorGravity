using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorridorGravity.Animations;
using CorridorGravity.Entities;
using Microsoft.Xna.Framework;
using System.IO;

namespace CorridorGravity.Services
{
    /// <summary>
    /// Class provides entities <see cref="Entity"/>  with animation load method/logic
    /// </summary>
    class AnimationLoader
    {
        private static AnimationLoader _Instance;
        public static AnimationLoader Instance
        { get {
                if (_Instance == null)
                    _Instance = new AnimationLoader();
                return _Instance;
            } }

        private AnimationLoader() { }

        public static float INTERVAL_DEFAULT = 0.15f;

        /// <summary>
        /// Animation files extention
        /// </summary>
        public static string FILE_EXTENSION = "txt";

        public void Initialize() { }

        /// <summary>
        /// Uses .txt file in the ContentRoot directory to load up all animations <see cref="Animation"/>
        /// </summary>
        /// <typeparam name="TEnum">
        /// Represent flexible enum for animations states 
        /// (each animation type could have specific Enum State for it) <see cref="AnimationState"/>
        /// </typeparam>
        /// <param name="fileName">
        /// Name of the sprite, so animation texture/sprite and file for that animation, should have same name
        /// </param>
        /// <returns>
        /// Collection where each state like <see cref="AnimationState"/> represents
        /// with the specific Animation for it <see cref="Animation"/>
        /// </returns>
        public Dictionary<TEnum, Animation> LoadAnimations<TEnum>(string fileName)
                                                            where TEnum : struct, IConvertible
        {
            TEnum state = default(TEnum);

            var animationCollection = new Dictionary<TEnum, Animation>();
            var animation = new Animation();

            // Uses animation texture/sprite name and changes the file extention with .txt
            var dataFile = Path.Combine(ServiceLocator.Instance.Content.RootDirectory,
                                            Path.ChangeExtension(fileName, FILE_EXTENSION));
            var dataFileLines = File.ReadAllLines(dataFile);

            // Select all NonNullable rows and rows starting not with symbol '#', separate rows by symbol ';'
            // Line starts with # is comment, 
            // 2 cols = new Animation, 
            // 7 cols = Frame in Animation
            foreach (var cols in from row in dataFileLines
                                 where !string.IsNullOrEmpty(row) && !row.StartsWith("#")
                                 select row.Split(';'))
            {
                if(cols.Length == 1)
                {
                    var jj = 1;
                }
                if (cols.Length == 2)
                {
                    if (animation.FrameList.Count != 0)
                        animationCollection.Add(state, animation);
                    animation = new Animation();

                    continue;
                }
                else if (cols.Length != 7)
                    throw new InvalidDataException("Incorrect format data in file: " + fileName);

                if (!Enum.TryParse(cols[0], true, out state))
                    throw new ArgumentException("Incorrect name format in: " + fileName, cols[0]);

                Rectangle rectangle;

                try
                {
                    rectangle = new Rectangle(
                        int.Parse(cols[1]),
                        int.Parse(cols[2]),
                        int.Parse(cols[3]),
                        int.Parse(cols[4]));
                }
                catch (Exception ex)
                {
                    throw new Exception("Inccorect rectangle format in: " + fileName, ex);
                }

                double duration;
                if (!double.TryParse(cols[5], out duration))
                    throw new ArgumentException("Inccorect duration format in: " + fileName, cols[5]);

                int direction;
                if (!int.TryParse(cols[6], out direction))
                    throw new ArgumentException("Inccorect direction format in: " + fileName, cols[6]);

                var effect = Convert.ToBoolean(direction);

                animation.AddFrame(rectangle, TimeSpan.FromSeconds(duration));
            }

            animationCollection.Add(state, animation);

            return animationCollection;
        }
    }
}