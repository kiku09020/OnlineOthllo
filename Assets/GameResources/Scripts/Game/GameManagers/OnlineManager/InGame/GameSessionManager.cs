using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Game.Board;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameController.Online {
	/// <summary> ゲーム内のセッションの管理クラス </summary>
	public class GameSessionManager : MonoBehaviour, INetworkRunnerCallbacks {
		/* Fields */
		[SerializeField] BoardManager boardManager;
		[SerializeField] PlayerManager playerManager;

		[Header("Debug")]
		[SerializeField] NetworkRunner runnerPrefab;

		bool isConnected;

		//-------------------------------------------------------------------
		/* Properties */

		//-------------------------------------------------------------------
		/* Messages */

		async void Start()
		{
			var runner = FindFirstObjectByType<NetworkRunner>();

			// デバッグ時に、接続シーンを経由せずにこのシーンを実行した場合、
			// 自動でNetworkRunnerを確保する
			if (Debug.isDebugBuild && runner == null) {
				runner = await SessionConnecter.Connect(runnerPrefab);
			}

			runner.AddCallbacks(this);

			// 接続されるまで待機
			await UniTask.WaitUntil(() => runner.ActivePlayers.ToList().Count >= 2);

			await playerManager.SpawnPlayer(runner);

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
	}
}