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
        [Networked]
        public bool IsOperable { get; private set; }

        [Networked]
        public bool IsFirstTurn { get; set; }

        /// <summary> �����̐΂̐� </summary>
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
            print($"�v���C���[�I�u�W�F�N�g({PersonalName})����������܂���");
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
            // ��U�v���C���[�𑀍�\�ɂ���
            if (IsFirstTurn) {
                ToggleOperable();
            }

            OnSetDataSync?.Invoke(NetworkData);
        }

        //--------------------------------------------------
        /// <summary> ����\����؂�ւ��� </summary>
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