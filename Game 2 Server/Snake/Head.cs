using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Game_2_Server.Snake
{
    [Serializable]
    class Head : PlayerComponent
    {

        #region fields

        #endregion

        #region properties

        #endregion

        #region methods

        public Head(Vector2 pPosition, float pRotation) : base(pPosition, pRotation)
        {
        }


        public override void Update(GameTime gameTime)
        {
        }

        public override void MoveSnake(object source, ElapsedEventArgs e)
        {

            RotateBy(Rotation);
            DirectionVector.Normalize();
            CurrentPosition += DirectionVector;

            Console.WriteLine("X: " + CurrentPosition.X + " Y: " + CurrentPosition.Y + " R: " + Rotation);
        }

        public override bool CheckCollision(Rectangle pRectangle)
        {
            if (Rectangle.Intersects(Rectangle))
            {
                return true;
            }
            else
                return false;
        }

        public override void PreviousPosLogic(object source, ElapsedEventArgs e)
        {
            PreviousPosition = CurrentPosition;
            PreviousRotation = Rotation;
        }
        #endregion
    }
}
