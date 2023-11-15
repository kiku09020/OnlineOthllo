using Cysharp.Threading.Tasks;
using Fusion;
using Game.Board;
using Game.Player;
using GameController.Game;
using GameUtils.SceneController;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameController.Online {
	/// <summary> ゲーム内のセッションの管理クラス </summary>
	public class GameSessionManager : MonoBehaviour {
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
		async void Awake()
		{
			runner = FindFirstObjectByType<NetworkRunner>();

			// デバッグ時に、接続シーンを経由せずにこのシーンを実行した場合、
			// 自動でNetworkRunnerを確保する
			if (Debug.isDebugBuild && runner == null) {
				runner = await SessionConnecter.Connect(runnerPrefab);
			}

			// 接続されるまで待機
			await UniTask.WaitUntil(() => runner.ActivePlayers.ToList().Count >= 2);

			// プレイヤー設定
			foreach (var player in runner.ActivePlayers) {
				SpawnPlayerObject(runner, player);      // PlayerObject生成(UI)
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

		//-------------------------------------------------------------------
		/* Methods */
		// プレイヤー生成
		void SpawnPlayerObject(NetworkRunner runner, PlayerRef player)
		{
			var playerUIObj = runner.Spawn(playerObjectPrefab);
			//runner.SetPlayerObject(player, playerUIObj);

			playerUIObj.transform.SetParent(playerParent);
			var playerRT = playerUIObj.GetComponent<RectTransform>();

			// データ設定
			var playerObj = playerUIObj.GetComponent<PlayerObject>();

			playerObj.NetworkData.IsFirstTurn = turnManager.SetTurnOrder(runner, player);   // ターン順決める

			// 自身のとき、左下に表示
			if (player == runner.LocalPlayer) {
				playerObj.NetworkData.NickName = "じぶん";

				playerRT.anchorMin = Vector2.zero;
				playerRT.anchorMax = Vector2.zero;
				playerRT.pivot = Vector2.zero;
				playerRT.anchoredPosition = new Vector3(32, 32, 0);
			}

			// 相手のとき、右上に表示
			else {
				playerObj.NetworkData.NickName = "あいて";

				playerRT.anchorMin = Vector2.one;
				playerRT.anchorMax = Vector2.one;
				playerRT.pivot = Vector2.one;
				playerRT.anchoredPosition = new Vector3(-32, -32, 0);
			}

			playerObj.SetData(playerObj.NetworkData);
		}
	}
}