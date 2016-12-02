using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace CorridorGravity.Services
{
    /// <summary>
    /// Represents all ingame texture/sprite/sources types
    /// </summary>
    public enum TTypes
    {
        INTRO,
        BACKGROUND,
        PAUSED,
        PLAYER,
        ENEMY,
        BOSS,
        MAGIC,

        LABELS,
        INTRO_NAME,
        PAUSE,
        DEAD,
        SCORE,
        PRESS,
        FADE_BLACK,
        HEALTH,

        EYE_BLACK,
        EYE_WHITE,
        EYE_RED
    }

    /// <summary>
    /// Manages all pipeline work with names/loads
    /// </summary>
    class PipelineManager
    {
        protected ContentManager Content;

        private static PipelineManager _Instance;
        public static PipelineManager Instance
        { get {
                if (_Instance == null)
                    _Instance = new PipelineManager();
                return _Instance;
            } }

        private PipelineManager() { }

        public void Initialize(ContentManager content)
        {
            this.Content = content;
        }

        /// <summary>
        /// Root directory for audio
        /// </summary>
        public readonly string ARoot = "Audio\\";

        /// <summary>
        /// Contains all audio file names and types assosiated with them
        /// </summary>
        public readonly Dictionary<SongTypes, string> AList = new Dictionary<SongTypes, string>() {
                                                   { SongTypes.LAUGH,    "klankbeeld-laugh"                  },
                                                   { SongTypes.DEAD,     "rocotilos-game-over-evil"          },
                                                   { SongTypes.INGAME,   "joshuaempyre-arcade-music-loop"    },
                                                   { SongTypes.INTRO,    "xdimebagx-atmosphere-horror-loop"  } };
        /// <summary>
        /// Root directory for textures
        /// </summary>
        public readonly string TRoot = "Textures\\";

        /// <summary>
        /// Contains all texture file names and types assosiated with them
        /// </summary>
        public readonly Dictionary<TTypes, string> TList = new Dictionary<TTypes, string>() {
                                                   { TTypes.INTRO,      "intro-background"  },
                                                   { TTypes.BACKGROUND, "world-background"  },
                                                   { TTypes.BOSS,       "magolor-soul"      },
                                                   { TTypes.LABELS,     "labels"            },
                                                   { TTypes.PLAYER,     "player"            },
                                                   { TTypes.ENEMY,      "enemy"             },
                                                   { TTypes.MAGIC,      "magic"             } };
        /// <summary>
        /// Contains all sources for specific textures and types assosiated with them
        /// </summary>
        public readonly Dictionary<TTypes, Rectangle> TSources = new Dictionary<TTypes, Rectangle>() {
                                                   { TTypes.DEAD,       new Rectangle(42, 232, 299, 143)},
                                                   { TTypes.SCORE,      new Rectangle(369, 231, 184, 48)},
                                                   { TTypes.FADE_BLACK, new Rectangle(471, 387, 80, 80) },
                                                   { TTypes.PAUSE,      new Rectangle(3, 379, 435, 201) },
                                                   { TTypes.EYE_WHITE,  new Rectangle(483, 134, 66, 55) },
                                                   { TTypes.PRESS,      new Rectangle(7, 586, 231, 39)  },
                                                   { TTypes.INTRO_NAME, new Rectangle(25, 2, 535, 206)  },
                                                   { TTypes.HEALTH,     new Rectangle(465, 527, 36, 38) },
                                                   { TTypes.EYE_RED,    new Rectangle(501, 95, 27, 26)  },
                                                   { TTypes.EYE_BLACK,  new Rectangle(510, 71, 8, 8)    } };
        /// <summary>
        /// Font's name used ingame
        /// </summary>
        public readonly string FONT = "score-font";

        public Song         ALoad(string fileName)
        {
            return Content.Load<Song>(ARoot + fileName);
        }
        public SoundEffect  SLoad(string fileName)
        {
            return Content.Load<SoundEffect>(ARoot + fileName);
        }
        public Texture2D    TLoad(string fileName)
        {
            return Content.Load<Texture2D>(TRoot + fileName);
        }
        public SpriteFont   FLoad(string fileName)
        {
            return Content.Load<SpriteFont>(fileName);
        }

        public void UnloadContent()
        {
            this.Content.Unload();
        }
    }
}
