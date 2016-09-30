using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorridorGravity.GameLogic.Entities
{
    class SpellEntity
    {
        public SpellEntity(float x, float y, int type)
        {
            X = x;
            Y = y;
            SpellType = type;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public DateTime LastHitTime {get; set;}

        public int SpellType { get; set; }
    }
}
