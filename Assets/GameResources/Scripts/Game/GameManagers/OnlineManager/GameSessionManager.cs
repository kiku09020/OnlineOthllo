using Fusion;
using Fusion.Sockets;
using Game.Board;
using GameUtils.SceneController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Online {
    /// <summary> �Q�[�����̃Z�b�V�����̊Ǘ��N���X </summary>
    public class GameSessionManager : SimulationBehaviour, INetworkRunnerCallbacks {
        /* Fields */
        [SerializeField] BoardManager boardManager;

        [Header("Debug")]
        [SerializeField] NetworkRunner runnerPrefab;

        bool isConnected;

        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Messages */
        async void Awake()
        {
            // �f�o�b�O���ɁA�ڑ��V�[�����o�R�����ɂ��̃V�[�������s�����ꍇ�A
            // ������NetworkRunner���m�ۂ���
            if (Debug.isDebugBuild && Runner == null) {
                Runner = await SessionConnecter.Connect(runnerPrefab);
            }

            Runner.AddCallbacks(this);
        }

        private void OnDestroy()
        {
            Runner.RemoveCallbacks(this);
        }

        //--------------------------------------------------
        /* Callbacks */
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            // �}�X�^�[�N���C�A���g���ڑ����ꂽ�琶��
            if (runner.IsSharedModeMasterClient && !isConnected) {
                boardManager.SetStartBoard(runner);
            }

            isConnected = true;
            print("connected!");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }

        //-------------------------------------------------------------------
        /* Methods */

    }
}