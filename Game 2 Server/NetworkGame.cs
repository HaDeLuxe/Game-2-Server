using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Game_2_Server
{
    class NetworkGame
    {
        #region fields

        private long _player1 = 0;

        private long _player2 = 0;

        private bool _player1Ingame;

        private bool player2Ingame;


        #endregion

        #region properties

        public long Player1 {
            get {
                return _player1;
            }
            set {
                if (_player1 == 0)
                    _player1 = value;
            }
        }

        public long Player2 {
            get {
                return _player2;
            }
            set {
                if (_player2 == 0)
                {
                    _player2 = value;
                }
            }
        }

        public bool gameActive { get; set; }

        public int numberOfPlayer {
            get {
                if (_player1 != 0 && _player2 != 0)
                    return 2;
                else if ((_player1 != 0 && _player2 == 0) || (_player1 == 0 && _player2 != 0))
                    return 1;
                else return 0;
            }
        }

        #endregion


        #region methods

        #endregion
    }
}
