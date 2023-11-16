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

        /// <summary> 先行かどうか </summary>
        public bool IsFirstTurn {
            get => PlayerPrefs.GetInt("IsFirst") == 1;
            set => PlayerPrefs.SetInt("IsFirst", (value == true) ? 1 : 0);
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
        public bool IsOperable { get; private set; }

        /// <summary> 自分の石の数 </summary>
        public int DiskCount { get; private set; }

        //-------------------------------------------------------------------
        /* Event */
        public event System.Action<PlayerNetworkData> OnSetDataSync;

        public override void Spawned()
        {
            raycaster = Camera.main.GetComponent<PhysicsRaycaster>();
            raycaster.enabled = false;
        }

        //-------------------------------------------------------------------
        /* Methods */
        public void SetDataSync(string nickName, bool isFirstTurn)
        {
            NetworkData.NickName = nickName;
            NetworkData.IsFirstTurn = isFirstTurn;

            SetDataSyncProcess();
        }

        public void SetDataSync(PlayerNetworkData data)
        {
            NetworkData = data;
            SetDataSyncProcess();
        }

        void SetDataSyncProcess()
        {
            // 先攻プレイヤーを操作可能にする
            if (Object.HasStateAuthority && NetworkData.IsFirstTurn) {
                ToggleOperable();
            }

            OnSetDataSync?.Invoke(NetworkData);
        }

        //--------------------------------------------------
        /// <summary> 操作可能性を切り替える </summary>
        public void ToggleOperable()
        {
            IsOperable = !IsOperable;

            raycaster.enabled = IsOperable;
        }
    }
}