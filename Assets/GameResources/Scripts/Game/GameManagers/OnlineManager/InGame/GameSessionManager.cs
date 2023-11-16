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
    /// <summary> ゲーム内のセッションの管理クラス </summary>
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

            // デバッグ時に、接続シーンを経由せずにこのシーンを実行した場合、
            // 自動でNetworkRunnerを確保する
            if (Debug.isDebugBuild && runner == null) {
                runner = await SessionConnecter.Connect(runnerPrefab);
            }

            runner.AddCallbacks(this);

            // 接続されるまで待機
            await UniTask.WaitUntil(() => runner.ActivePlayers.ToList().Count >= 2);

            // ローカルプレイヤー設定
            var playerUIObj = runner.Spawn(playerObjectPrefab);
            runner.SetPlayerObject(runner.LocalPlayer, playerUIObj);


            // 各プレイヤーの設定
            foreach (var player in runner.ActivePlayers) {
                await UniTask.WaitUntil(() => {
                    if (runner.TryGetPlayerObject(player, out var playerObj)) {
                        SpawnPlayerObject(playerObj, player);
                        return true;
                    }

                    return false;
                });
            }

            // マスタークライアントが接続されたら生成
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
            print("プレイヤーが退出しました");
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
        // プレイヤー生成

        void SpawnPlayerObject(NetworkObject playerUIObj, PlayerRef player)
        {
            playerUIObj.transform.SetParent(playerParent);
            var playerRT = playerUIObj.GetComponent<RectTransform>();

            // データ設定
            var playerObj = playerUIObj.GetComponent<PlayerObject>();

            // ターン順決める
            playerObj.NetworkData.IsFirstTurn = turnManager.SetTurnOrder(runner, player);


            // 自身のとき、左下に表示
            if (player == runner.LocalPlayer) {
                playerObj.PersonalName = "じぶん";

                playerRT.anchorMin = Vector2.zero;
                playerRT.anchorMax = Vector2.zero;
                playerRT.pivot = Vector2.zero;
                playerRT.anchoredPosition = new Vector3(32, 32, 0);
            }

            // 相手のとき、右上に表示
            else {
                playerObj.PersonalName = "あいて";

                playerRT.anchorMin = Vector2.one;
                playerRT.anchorMax = Vector2.one;
                playerRT.pivot = Vector2.one;
                playerRT.anchoredPosition = new Vector3(-32, -32, 0);
            }

            playerObj.SetDataSync(playerObj.NetworkData);
        }
    }
}