using Cysharp.Threading.Tasks;
using Fusion;
using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Online {
    public class PlayerManager : MonoBehaviour {
        /* Fields */
        [Header("PlayerObject")]
        [SerializeField] NetworkPrefabRef playerObjectPrefab;
        [SerializeField] Transform playerParent;


        //-------------------------------------------------------------------
        /* Properties */
        public List<PlayerObject> Players { get; private set; }

        //-------------------------------------------------------------------
        /* Messages */
        public event System.Func<NetworkRunner, PlayerRef, bool> OnSetTurnOrder;
        public event System.Action<PlayerObject> OnSetData;

        //-------------------------------------------------------------------
        /* Methods */
        public async UniTask SpawnPlayer(NetworkRunner runner)
        {
            // ローカルプレイヤー設定
            var playerUIObj = runner.Spawn(playerObjectPrefab);
            runner.SetPlayerObject(runner.LocalPlayer, playerUIObj);

            // 全てのプレイヤーオブジェクトが生成されるまで待機  
            foreach (var player in runner.ActivePlayers) {
                await UniTask.WaitUntil(() => runner.TryGetPlayerObject(player, out var playerObj));
            }

            // 各プレイヤーの設定
            foreach (var player in runner.ActivePlayers) {
                var plObj = runner.GetPlayerObject(player);

                SetPlayerObject(plObj, runner, player);
            }
        }

        void SetPlayerObject(NetworkObject playerUIObj, NetworkRunner runner, PlayerRef player)
        {
            playerUIObj.transform.SetParent(playerParent);
            var playerRT = playerUIObj.GetComponent<RectTransform>();

            // データ設定
            var playerObj = playerUIObj.GetComponent<PlayerObject>();

            // ターン順決める
            if (OnSetTurnOrder != null)
                playerObj.IsFirstTurn = OnSetTurnOrder.Invoke(runner, player);


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
            OnSetData?.Invoke(playerObj);
        }
    }
}