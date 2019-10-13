using Game_2_Server.Snake;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Game_2_Server
{

    enum SendMessageType
    {
        GET_NUMBER_PLAYER_IN_GAME,
        JOINED_GAME_SUCCESS_PLAYER_1,
        JOINED_GAME_SUCCESS_PLAYER_2,
        JOINED_GAME_FAILURE,
        DISCOVERY,
        MOVE
    }

    
    
    class Server
    {
        #region fields

        private NetServer _server;

        private NetworkGame netGame1;

        #endregion

        #region properties

        public Dictionary<long, NetConnection> ConnectionDic { get; set; }

        #endregion

        #region methods

        public void StartServer(NetworkGame pNetworkGame)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Game 2")
            { Port = 8080};
            config.EnableUPnP = true;

            _server = new NetServer(config);
            _server.Start();
            Console.WriteLine("Server is active");
            checkForMessages();
            
            netGame1 = pNetworkGame;

            // attempt to forward port 14242
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            //_server.UPnP.ForwardPort(8080, "Text detail here");

            ConnectionDic = new Dictionary<long, NetConnection>();
        }

       

        public void closingServer()
        {
            _server.Shutdown("bye");
        }

        

        public void checkForMessages()
        {
            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        //handle custom messages
                        var data = msg.ReadString();
                        switch (data)
                        {
                            case "Connect To Game":
                                Console.WriteLine("Connect To Game");
                                short state = joinGame(msg.SenderConnection.RemoteUniqueIdentifier);
                                if (state == 1)
                                {
                                    sendMsg(SendMessageType.JOINED_GAME_SUCCESS_PLAYER_1, msg.SenderConnection);
                                    ConnectionDic.Add(msg.SenderConnection.RemoteUniqueIdentifier, msg.SenderConnection);
                                }
                                else if (state == 2)
                                {
                                    sendMsg(SendMessageType.JOINED_GAME_SUCCESS_PLAYER_2, msg.SenderConnection);
                                    ConnectionDic.Add(msg.SenderConnection.RemoteUniqueIdentifier, msg.SenderConnection);
                                }
                                else
                                    sendMsg(SendMessageType.JOINED_GAME_FAILURE, msg.SenderConnection);
                                break;
                            case "Get number of players in Game":
                                //sendMsg(SendMessageType.GET_NUMBER_PLAYER_IN_GAME, msg.SenderConnection);
                                break;
                            case "JOIN_SERVER":
                                Console.WriteLine("Player " + msg.SenderConnection.RemoteUniqueIdentifier + " connected to Server.");
                                break;
                            case "ENTER_GAME":
                                    netGame1.playerEnteredGameStatusChange(msg.SenderConnection.RemoteUniqueIdentifier);
                                break;
                            case "LEFT":
                                netGame1.rotatePlayer(msg.SenderConnection.RemoteUniqueIdentifier,false);
                                break;
                            case "RIGHT":
                                netGame1.rotatePlayer(msg.SenderConnection.RemoteUniqueIdentifier, true);

                                break;
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        //handle connection status mesasages
                        switch (msg.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                break;
                        }
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        //handle Debug messages
                        //only received when compiled in DEBUG mode
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        //Create a response
                        NetOutgoingMessage response = _server.CreateMessage();
                        response.Write("Server is here");
                        _server.SendDiscoveryResponse(response, msg.SenderEndPoint);
                        break;
                    default:
                        Console.WriteLine("unhandled message with type: " + msg.MessageType);
                        break;
                }
            }
        }

        public void sendMsg(SendMessageType pSendMsgType, NetConnection pReceiver)
        {
            var msg = _server.CreateMessage();
            switch (pSendMsgType)
            {
                case SendMessageType.GET_NUMBER_PLAYER_IN_GAME:
                    msg.Write("Numbers of players in Game: " + netGame1.numberOfPlayer.ToString());
                    break;
                case SendMessageType.JOINED_GAME_SUCCESS_PLAYER_1:
                    msg.Write("JOINED_GAME_SUCCESS_PLAYER_1");
                    break;
                case SendMessageType.JOINED_GAME_SUCCESS_PLAYER_2:
                    msg.Write("JOINED_GAME_SUCCESS_PLAYER_2");
                    break;
                case SendMessageType.JOINED_GAME_FAILURE:
                    msg.Write("JOINED_GAME_FAILURE");
                    break;
            }
            _server.SendMessage(msg, pReceiver, NetDeliveryMethod.ReliableOrdered);

        }

        

        /// <summary>
        /// Server Sided Network Messages while Main Game is running
        /// </summary>
        /// <param name="pSendMsgType"></param>
        /// <param name="pReceiver"></param>
        public void SendMainGameMsg(SendMessageType pSendMsgType, NetConnection pReceiver, PlayerComponent pPlayerComponent)
        {
            var msg = _server.CreateMessage();
            {
                switch (pSendMsgType)
                {
                    case SendMessageType.MOVE:
                        msg.Write("MOVE");
                        msg.WriteVariableInt32((int)pPlayerComponent.CurrentPosition.X);
                        msg.WriteVariableInt32((int)pPlayerComponent.CurrentPosition.Y);
                        msg.Write(pPlayerComponent.Rotation);
                        break;
                   
                }
                _server.SendMessage(msg, pReceiver, NetDeliveryMethod.ReliableOrdered);
            }
        }


        public short joinGame(long pIdentifier)
        {
            if (netGame1.Player1 == 0)
            {
                netGame1.Player1 = pIdentifier;
                return 1;
            }
            else if (netGame1.Player2 == 0 && netGame1.Player1 != pIdentifier)
            {
                netGame1.Player2 = pIdentifier;
                return 2;
            }
            else return 0;
        }

        #endregion

    }
}
