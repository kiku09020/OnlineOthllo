using Fusion;
using Fusion.Sockets;
using GameUtils.SceneController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameController.Online {
    public class SessionConnecter : MonoBehaviour, INetworkRunnerCallbacks {
        /* Fields */
        [SerializeField] NetworkRunner runnerPrefab;

        NetworkRunner runner;

        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Events */
        public event System.Action<NetworkRunner> OnConnected;

        async void Start()
        {
            runner = await Connect(runnerPrefab);       // NetworkRunner�̐����A�ڑ�����
            runner.AddCallbacks(this);                  // �R�[���o�b�N�ǉ�
        }

        // �R�[���o�b�N����
        void OnDestroy()
        {
            runner.RemoveCallbacks(this);
        }

        //--------------------------------------------------
        /* Callbacks */

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            OnConnected?.Invoke(runner);

            if (runner.SessionInfo.PlayerCount >= runner.SessionInfo.MaxPlayers) {
                SceneController.LoadNextScene();
                print("�ő吔�ɒB���܂���");
            }
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
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }

        //-------------------------------------------------------------------
        /* Methods */
        /// <summary> �ڑ����� </summary>
        public async static Task<NetworkRunner> Connect(NetworkRunner runnerPrefab)
        {
            var runner = Instantiate(runnerPrefab);

            // �Q�[���J�n
            var connectResult = await runner.StartGame(new StartGameArgs {
                SessionName = "Room",
                PlayerCount = 2,

                GameMode = GameMode.Shared,
                SceneManager = runner.GetComponent<NetworkSceneManagerDefault>(),
            });

            // �ڑ��������Ƀv���C���[����
            if (connectResult.Ok) {
                print("�ڑ�����");
            }
            else {
                print("�ڑ����s");
            }

            return runner;
        }

        /// <summary> �v���C���[�̐� / �Z�b�V�����̍ő�v���C���[�� </summary>
        public string GetPlayerCountText(NetworkRunner runner)
            => $"{runner.SessionInfo.PlayerCount} / {runner.SessionInfo.MaxPlayers}";


    }
}