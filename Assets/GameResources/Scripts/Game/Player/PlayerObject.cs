using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Player {
    /// <summary> プレイヤー情報 </summary>
    public class PlayerNetworkData {
        /// <summary> プレイヤー名 </summary>
        public string NickName {
            get => PlayerPrefs.GetString("NickName", "Player");
            set => PlayerPrefs.SetString("NickName", value);
        }
    }

    public class PlayerObject : NetworkBehaviour {
        /* Fields */
        PhysicsRaycaster raycaster;

        //-------------------------------------------------------------------
        /* Properties */
        public PlayerNetworkData NetworkData { get; set; } = new PlayerNetworkData();

        /// <summary> 人称名 </summary>
        /// <remarks> じぶん・あなた </remarks>
        public string PersonalName { get; set; } = "Player";

        /// <summary> 操作可能か </summary>
        [Networked]
        public bool IsOperable { get; private set; }

        [Networked]
        public bool IsFirstTurn { get; set; }

        /// <summary> 自分の石の数 </summary>
        public int DiskCount { get; private set; }

        //-------------------------------------------------------------------
        /* Event */
        public event System.Action<PlayerNetworkData> OnSetDataSync;

        private void Awake()
        {
            raycaster = Camera.main.GetComponent<PhysicsRaycaster>();
            raycaster.enabled = false;
        }

        public override void Spawned()
        {
            print($"プレイヤーオブジェクト({PersonalName})が生成されました");
        }

        //-------------------------------------------------------------------
        /* Methods */
        public void SetDataSync(PlayerNetworkData data)
        {
            NetworkData = data;
            SetDataSyncProcess();
        }

        void SetDataSyncProcess()
        {
            // 先攻プレイヤーを操作可能にする
            if (IsFirstTurn) {
                ToggleOperable();
            }

            OnSetDataSync?.Invoke(NetworkData);
        }

        //--------------------------------------------------
        /// <summary> 操作可能性を切り替える </summary>
        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_ToggleOperable()
        {
            ToggleOperable();
        }

        public void ToggleOperable()
        {
            if (Object.HasStateAuthority) {

                IsOperable = !IsOperable;
                raycaster.enabled = IsOperable;
                print($"{PersonalName}:{nameof(IsOperable)}:{IsOperable}");
            }
        }
    }
}