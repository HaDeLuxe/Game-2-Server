using Game_2_Server.Snake;
using Lidgren.Network;
using Microsoft.Xna.Framework;
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

        private bool _player1Ingame = false;

        private bool _player2Ingame = false;

        

        private Server _server = null;



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


        /// <summary>
        /// True if both players are in PLAYFIELD Mode
        /// </summary>
        public bool GameActive { get; set; }
        
        public bool GameRuntimeStarted { get; set; }

        public int numberOfPlayer {
            get {
                if (_player1 != 0 && _player2 != 0)
                    return 2;
                else if ((_player1 != 0 && _player2 == 0) || (_player1 == 0 && _player2 != 0))
                    return 1;
                else return 0;
            }
        }

        public List<PlayerComponent> _player1List { get; set; }

        public List<PlayerComponent> _player2List { get; set; }

        #endregion

        #region methods

        public NetworkGame(Server pServer)
        {
            _player1List = new List<PlayerComponent>();

            _player2List = new List<PlayerComponent>();

            _server = pServer;


            GameActive = false;
            
        }

        public void InitGame()
        {
            
            _player1List.Add(new Head(new Vector2(100,100), (float)0));
            _player2List.Add(new Head(new Vector2(500, 500), (float)Math.PI));
        }


        public void Update(GameTime gameTime)
        {
            if (!GameActive)
            {
                if (_player1Ingame && _player2Ingame)
                {
                    GameActive = true;
                    InitGame();
                }

            }

            if (GameActive)
            {
                UpdatePlayers(gameTime);
                
                _server.SendMainGameMsg(SendMessageType.MOVE, _server.ConnectionDic[Player1], _player1List[0]);
                _server.SendMainGameMsg(SendMessageType.MOVE, _server.ConnectionDic[Player2], _player2List[0]);


            }
        }

        public void UpdatePlayers(GameTime gameTime)
        {
            _updatePlayer(_player1List, gameTime, _player1);
            _updatePlayer(_player2List, gameTime, _player2);
        }

        /// <summary>
        /// Return element of snake as PlayerComponent
        /// </summary>
        /// <param name="pPlayerID"></param>
        /// <param name="pItem"></param>
        /// <returns></returns>
        public PlayerComponent getPlayerComponent(long pPlayerID, int pItem)
        {
            if (pPlayerID == _player1)
            {
                if (pItem >= 0 && pItem <= _player1List.Count)
                {
                    return _player1List[pItem];
                }
                else return null;
            }
            else if (pPlayerID == _player2)
            {
                if (pItem >= 0 && pItem <= _player1List.Count)
                {
                    return _player2List[pItem];
                }
                else return null;
            }
            else return null;
        }

        private void _updatePlayer(List<PlayerComponent> pPlayerList, GameTime gameTime, long pPlayerID)
        {
            //for (int i = 1; i <= pPlayerList.Count - 1; i++)
            //{
            //    if (pPlayerList[i - 1].PreviousPosition != null || pPlayerList[i - 1].PreviousPosition == pPlayerList[i - 1].CurrentPosition)
            //    {
            //        pPlayerList[i].NewPosition = pPlayerList[i - 1].PreviousPosition;
            ////        pPlayerList[i].Rotation = pPlayerList[i - 1].PreviousRotation;
            //    }
            //}


            for(int i = 0; i < pPlayerList.Count; i++)
            {
                pPlayerList[i].Update(gameTime);
            }
            

            //if (_foodList.Count > 0 && pPlayerList[0].CheckCollision(_foodList[0].Rectangle))
            //{
            //    _foodList.RemoveAt(0);
            //    for (int i = 0; i < 1; i++)
            //    {
            //        if (pPlayerID == 1)
            //            pPlayerList.Add(new Body(_snake_Body_Pl1_Texture, pPlayerList[(pPlayerList.Count - 1)].PreviousPosition, (float)Math.PI));
            //        else if (pPlayerID == 2)
            //            pPlayerList.Add(new Body(_snake_Head_Pl2_Texture, pPlayerList[(pPlayerList.Count - 1)].PreviousPosition, (float)Math.PI));
            //    }
            //}
        }

        /// <summary>
        /// changes the direction of a player
        /// pDir:
        ///     false : left
        ///     true : right
        /// </summary>
        /// <param name="pPlayerID"></param>
        /// <param name="dir"></param>
        public void rotatePlayer(long pPlayerID, bool pDir)
        {
            if(pPlayerID == _player1)
            {
                if (pDir)
                {
                    _player1List[0].Rotation += (float)Math.PI / 48;

                }
                else _player1List[0].Rotation -= (float)Math.PI / 48;
            }
            else if(pPlayerID == _player2)
            {
                if (pDir) _player2List[0].Rotation += (float)Math.PI / 48;
                else _player2List[0].Rotation -= (float)Math.PI / 48;
            }
        }

        /// <summary>
        /// When player enters to PLAYFIELD Mode the _playerIngame Boolean for specific user is changed to true
        /// </summary>
        /// <param name="pID"></param>
        public void playerEnteredGameStatusChange(long pID)
        {
            if (pID == Player1) _player1Ingame = true;
            else if (pID == Player2) _player2Ingame = true;
        }

        #endregion
    }

    
}
