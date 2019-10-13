using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Game_2_Server.Snake
{
    class Body : PlayerComponent
    {
        #region fields

        private Vector2 _moveDir;

        private float _currentRot;

        #endregion

        #region properties


        #endregion


        #region methods

        public Body( Vector2 pPosition, float pRotation) : base(pPosition, pRotation)
        {
        }



        
        public override void MoveSnake(object source, ElapsedEventArgs e)
        {
            CurrentPosition -= _moveDir;

            _currentRot = Lerp(PreviousRotation, Rotation, 100000);
        }

        public override void PreviousPosLogic(object source, ElapsedEventArgs e)
        {
            PreviousPosition = CurrentPosition;
            PreviousRotation = Rotation;
            _moveDir = PreviousPosition - NewPosition;
            _moveDir.Normalize();



        }

        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        public override void Update(GameTime gameTime)
        {
        }

        #endregion
    }
}
