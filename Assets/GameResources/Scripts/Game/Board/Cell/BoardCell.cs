using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
    using Cysharp.Threading.Tasks;
    using Disk;
    using Fusion;
    using Game.Player;

    /// <summary> 盤のマス </summary>
    public class BoardCell : NetworkBehaviour {
        /* Fields */
        [Header("Generating")]
        [SerializeField] NetworkPrefabRef diskPrefab;

        [Header("Components")]
        [SerializeField] Collider col;

        Disk diskObj;

        //-------------------------------------------------------------------
        /* Properties */
        /// <summary> マスの位置 </summary>
        public Vector2Int Position { get; private set; }

        /// <summary> マスの状態 </summary>
        [Networked]
        public DiskState State { get; private set; } = DiskState.empty;

        /// <summary> 配置可能か </summary>
        [Networked]
        public bool IsPuttable { get; private set; }

        public BoardCellInputProvider InputProvider { get; private set; }

        //-------------------------------------------------------------------
        /* Messages */
        public override void Spawned()
        {
            col.enabled = false;

            InputProvider = GetComponent<BoardCellInputProvider>();
            InputProvider.ClickedUpEvent += OnClickedUpEventHandler;
        }

        //--------------------------------------------------
        /* Events */
        /// <summary> ボード確認されたときのイベント </summary>
        public event System.Action<bool> OnChecked;

        //-------------------------------------------------------------------
        /* Methods */
        /// <summary> 生成されたときの処理 </summary>
        public void OnGenerated(Vector2Int pos)
        {
            Position = pos;
        }

        /// <summary> 配置可能 </summary>

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SetPuttable(bool value)
        {
            if (Object.HasStateAuthority) {
                IsPuttable = value;
                col.enabled = value;

                OnChecked?.Invoke(value);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public async void RPC_SetDiskState(DiskState state, bool isGeneratedOnStart = false)
        {
            if (Object.HasStateAuthority) {
                // マスの状態をセット
                if (state == DiskState.empty) return;
                State = state;

                // 石の生成
                diskObj = Runner.Spawn(diskPrefab, transform.position).GetComponent<Disk>();
                diskObj.transform.SetParent(transform);

                diskObj.SetDisk(state, !isGeneratedOnStart);              // 石の状態セット、アニメーションなど

                // 選択不可にする
                InputProvider.InvalidateSelectable();

                if (!isGeneratedOnStart) {
                    // ターン変更
                    foreach (var player in Runner.ActivePlayers) {
                        await UniTask.WaitUntil(() => {
                            var result = Runner.TryGetPlayerObject(player, out var playerObj);
                            playerObj.GetComponent<PlayerObject>().RPC_ToggleOperable();

                            return result;
                        });
                    }
                }
            }
        }

        //--------------------------------------------------
        async void OnClickedUpEventHandler()
        {
            col.enabled = false;

            foreach (var player in Runner.ActivePlayers) {
                await UniTask.WaitUntil(() => Runner.TryGetPlayerObject(player, out var playerObj));
            }

            foreach (var player in Runner.ActivePlayers) {
                var playerObj = Runner.GetPlayerObject(player).GetComponent<PlayerObject>();

                if (playerObj.HasStateAuthority) {
                    RPC_SetDiskState(playerObj.PlayersDiskState);
                }
            }
        }
    }
}