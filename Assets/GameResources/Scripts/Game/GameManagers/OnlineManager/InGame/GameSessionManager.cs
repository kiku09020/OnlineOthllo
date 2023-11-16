using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Game.Board;
using Game.Player;
using GameController.Game;
using GameUtils.SceneController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameController.Online {
    /// <summary> �Q�[�����̃Z�b�V�����̊Ǘ��N���X </summary>
    public class GameSessionManager : NetworkBehaviour, INetworkRunnerCallbacks {
        /* Fields */
        [SerializeField] BoardManager boardManager;
        [SerializeField] TurnManager turnManager;

        [Header("PlayerObject")]
        [SerializeField] NetworkPrefabRef playerObjectPrefab;
        [SerializeField] Transform playerParent;

        [Header("Debug")]
        [SerializeField] NetworkRunner runnerPrefab;

        bool isConnected;

        NetworkRunner runner;

        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Messages */
        async void Start()
        {
            runner = FindFirstObjectByType<NetworkRunner>();

            // �f�o�b�O���ɁA�ڑ��V�[�����o�R�����ɂ��̃V�[�������s�����ꍇ�A
            // ������NetworkRunner���m�ۂ���
            if (Debug.isDebugBuild && runner == null) {
                runner = await SessionConnecter.Connect(runnerPrefab);
            }

            runner.AddCallbacks(this);

            // �ڑ������܂őҋ@
            await UniTask.WaitUntil(() => runner.ActivePlayers.ToList().Count >= 2);

            // ���[�J���v���C���[�ݒ�
            var playerUIObj = runner.Spawn(playerObjectPrefab);
            runner.SetPlayerObject(runner.LocalPlayer, playerUIObj);


            // �e�v���C���[�̐ݒ�
            foreach (var player in runner.ActivePlayers) {
                await UniTask.WaitUntil(() => {
                    if (runner.TryGetPlayerObject(player, out var playerObj)) {
                        SpawnPlayerObject(playerObj, player);
                        return true;
                    }

                    return false;
                });
            }

            // �}�X�^�[�N���C�A���g���ڑ����ꂽ�琶��
            if (runner.IsSharedModeMasterClient && !isConnected) {
                boardManager.SetStartBoard(runner);
            }

            isConnected = true;
            print("connected!");
        }

        //--------------------------------------------------
        /* Callbacks */
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            print("�v���C���[���ޏo���܂���");
        }

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
        // �v���C���[����

        void SpawnPlayerObject(NetworkObject playerUIObj, PlayerRef player)
        {
            playerUIObj.transform.SetParent(playerParent);
            var playerRT = playerUIObj.GetComponent<RectTransform>();

            // �f�[�^�ݒ�
            var playerObj = playerUIObj.GetComponent<PlayerObject>();

            // �^�[�������߂�
            playerObj.NetworkData.IsFirstTurn = turnManager.SetTurnOrder(runner, player);


            // ���g�̂Ƃ��A�����ɕ\��
            if (player == runner.LocalPlayer) {
                playerObj.PersonalName = "���Ԃ�";

                playerRT.anchorMin = Vector2.zero;
                playerRT.anchorMax = Vector2.zero;
                playerRT.pivot = Vector2.zero;
                playerRT.anchoredPosition = new Vector3(32, 32, 0);
            }

            // ����̂Ƃ��A�E��ɕ\��
            else {
                playerObj.PersonalName = "������";

                playerRT.anchorMin = Vector2.one;
                playerRT.anchorMax = Vector2.one;
                playerRT.pivot = Vector2.one;
                playerRT.anchoredPosition = new Vector3(-32, -32, 0);
            }

            playerObj.SetDataSync(playerObj.NetworkData);
        }
    }
}