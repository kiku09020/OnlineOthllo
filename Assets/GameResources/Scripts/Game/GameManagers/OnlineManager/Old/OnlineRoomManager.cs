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
        // �V�[���J�ڎ��Ƀ}�X�^�[�T�[�o�[�ɐڑ�
        private void Start()
        {
            // �}�X�^�[�T�[�o�[�ɐڑ�
            PhotonNetwork.ConnectUsingSettings();
        }

        //--------------------------------------------------
        /*  */
        // �}�X�^�[�T�[�o�[�ڑ���
        public override void OnConnectedToMaster()
        {
            // �����_���ȕ����ɎQ������B�������Ȃ���΍쐬����B
            PhotonNetwork.JoinRandomOrCreateRoom(roomName: "Room", roomOptions: SetRoomOption(), typedLobby: TypedLobby.Default);
        }

        // �����ڑ���
        public override void OnJoinedRoom()
        {
            OnConnectedToRoom?.Invoke(Room);

            // �v���C���[�����ő吔�ɒB������A�����ɓ���Ȃ��悤�ɂ���
            if (Room.PlayerCount >= Room.MaxPlayers) {
                Room.IsOpen = false;

                // �V�[���J��
                photonView.RPC(nameof(LoadGameScene), RpcTarget.All);
            }
        }

        //--------------------------------------------------
        /* Failed EventHandlers */
        // �}�X�^�[�T�[�o�[�ڑ����s��
        public override void OnDisconnected(DisconnectCause cause)
        {
            if (Debug.isDebugBuild) {
                Debug.LogError(cause);
            }
        }

        // �����쐬���s��
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (Debug.isDebugBuild) {
                Debug.LogError(message);
            }
        }

        //-------------------------------------------------------------------
        /* Methods */
        // �����ݒ�
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