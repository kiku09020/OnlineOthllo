using GameUtils.SceneController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if false
namespace GameController.Online {
    public class OnlineRoomManager : MonoBehaviourPunCallbacks {
        /* Fields */

        //--------------------------------------------------

        //-------------------------------------------------------------------
        /* Properties */
        Room Room => PhotonNetwork.CurrentRoom;

        //--------------------------------------------------
        /* Events */
        public event System.Action<Room> OnConnectedToRoom;

        //-------------------------------------------------------------------
        /* Messages */
        // シーン遷移時にマスターサーバーに接続
        private void Start()
        {
            // マスターサーバーに接続
            PhotonNetwork.ConnectUsingSettings();
        }

        //--------------------------------------------------
        /*  */
        // マスターサーバー接続時
        public override void OnConnectedToMaster()
        {
            // ランダムな部屋に参加する。部屋がなければ作成する。
            PhotonNetwork.JoinRandomOrCreateRoom(roomName: "Room", roomOptions: SetRoomOption(), typedLobby: TypedLobby.Default);
        }

        // 部屋接続時
        public override void OnJoinedRoom()
        {
            OnConnectedToRoom?.Invoke(Room);

            // プレイヤー数が最大数に達したら、部屋に入れないようにする
            if (Room.PlayerCount >= Room.MaxPlayers) {
                Room.IsOpen = false;

                // シーン遷移
                photonView.RPC(nameof(LoadGameScene), RpcTarget.All);
            }
        }

        //--------------------------------------------------
        /* Failed EventHandlers */
        // マスターサーバー接続失敗時
        public override void OnDisconnected(DisconnectCause cause)
        {
            if (Debug.isDebugBuild) {
                Debug.LogError(cause);
            }
        }

        // 部屋作成失敗時
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (Debug.isDebugBuild) {
                Debug.LogError(message);
            }
        }

        //-------------------------------------------------------------------
        /* Methods */
        // 部屋設定
        RoomOptions SetRoomOption()
        {
            var roomOption = new RoomOptions() {
                MaxPlayers = 2,
            };

            return roomOption;
        }

        //--------------------------------------------------
        /* RPC */
        [PunRPC]
        void LoadGameScene()
        {
            SceneController.LoadNextScene();
        }
    }
}

#endif