using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Game_2_Server.Snake
{
    
    abstract class PlayerComponent
    {
        #region fields


        private Timer _timer;
        private Timer _timer2;

        protected static readonly int _width = 64;
        protected static readonly int _height = 64;


        #endregion

        #region properties

        public Vector2 CurrentPosition { get; set; }

        public Vector2 NewPosition { get; set; }

        public Vector2 PreviousPosition { get; set; }

        /// <summary>
        /// Rotation in DEGREES!
        /// </summary>
        public float Rotation { get; set; }

        public float PreviousRotation { get; set; }

        public Vector2 DirectionVector { get; set; }



        public Rectangle Rectangle {
            get {
                return new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y, _width, _height);
            }
        }

        #endregion

        #region methods

        protected PlayerComponent(Vector2 pPosition, float pRotation)
        {
            _initTimer();
            _initTimer2();
            CurrentPosition = pPosition;
            PreviousPosition = pPosition;
            Rotation = pRotation;
            PreviousRotation = pRotation;
        }

        public void RotateBy(float a)
        {
            DirectionVector = new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
        }

        private void _initTimer()
        {
            _timer = new Timer(10);
            _timer.Elapsed += MoveSnake;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void _initTimer2()
        {
            _timer2 = new Timer(1000);
            _timer2.Elapsed += PreviousPosLogic;
            _timer2.AutoReset = true;
            _timer2.Enabled = true;
        }

        public abstract void PreviousPosLogic(Object source, ElapsedEventArgs e);

        public abstract void MoveSnake(Object source, ElapsedEventArgs e);
        
        public abstract void Update(GameTime gameTime);

        public virtual bool CheckCollision(Rectangle pRectangle)
        {
            return false;
        }



        #endregion
    }
}
