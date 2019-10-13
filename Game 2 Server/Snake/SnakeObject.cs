using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_2_Server.Snake
{
    [Serializable]
    class SnakeObject
    {
        public float Rotation { get; set; }
        

        public float CurrentPositionX = 0;
        public float CurrentPositionY = 0;
    }
}
