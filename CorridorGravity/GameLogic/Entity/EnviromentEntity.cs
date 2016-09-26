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

namespace CorridorGravity.GameLogic
{
    class EnviromentEntity : Entity
    {
        public override float X { get; set; } 
        public override float Y { get; set; }

        private int LEVEL_HEIGHT { get; set; }
        private int LEVEL_WIDTH { get; set; }

        public override Texture2D EntitySprite { get; }

        public EnviromentEntity(ContentManager content, int levelHeight, int levelWidth)
        {
            EntitySprite = content.Load<Texture2D>("player-2-white-1");
            ConstractCommonParts(levelHeight, levelWidth);
        }

        public EnviromentEntity(ContentManager content, string contentName, int levelHeight, int levelWidth)
        {
            EntitySprite = content.Load<Texture2D>(contentName);
            ConstractCommonParts(levelHeight, levelWidth);
        }

        private void ConstractCommonParts(int levelHeight, int levelWidth)
        {
            LEVEL_HEIGHT = levelHeight/2;
            LEVEL_WIDTH = levelWidth/2;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Touch()
        {
            base.Touch();
        }
    }
}
