using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Player {
    /// <summary> �v���C���[��� </summary>
    public class PlayerNetworkData {
        /// <summary> �v���C���[�� </summary>
        public string NickName {
            get => PlayerPrefs.GetString("NickName", "Player");
            set => PlayerPrefs.SetString("NickName", value);
        }

        /// <summary> ��s���ǂ��� </summary>
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

        /// <summary> �l�̖� </summary>
        /// <remarks> ���Ԃ�E���Ȃ� </remarks>
        public string PersonalName { get; set; } = "Player";

        /// <summary> ����\�� </summary>
        public bool IsOperable { get; private set; }

        /// <summary> �����̐΂̐� </summary>
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
            // ��U�v���C���[�𑀍�\�ɂ���
            if (Object.HasStateAuthority && NetworkData.IsFirstTurn) {
                ToggleOperable();
            }

            OnSetDataSync?.Invoke(NetworkData);
        }

        //--------------------------------------------------
        /// <summary> ����\����؂�ւ��� </summary>
        public void ToggleOperable()
        {
            IsOperable = !IsOperable;

            raycaster.enabled = IsOperable;
        }
    }
}