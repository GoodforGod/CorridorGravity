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

        public override Texture2D EntitySprite { get; }

        private EnviromentEntity(ContentManager content)
        {

        }

        private EnviromentEntity(ContentManager content, string contentName)
        {

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
