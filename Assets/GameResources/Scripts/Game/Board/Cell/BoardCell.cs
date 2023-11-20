using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
    using Cysharp.Threading.Tasks;
    using Disk;
    using Fusion;
    using Game.Player;

    /// <summary> �Ղ̃}�X </summary>
    public class BoardCell : NetworkBehaviour {
        /* Fields */
        [Header("Generating")]
        [SerializeField] NetworkPrefabRef diskPrefab;

        [Header("Components")]
        [SerializeField] Collider col;

        Disk diskObj;

        //-------------------------------------------------------------------
        /* Properties */
        /// <summary> �}�X�̈ʒu </summary>
        public Vector2Int Position { get; private set; }

        /// <summary> �}�X�̏�� </summary>
        [Networked]
        public DiskState State { get; private set; } = DiskState.empty;

        /// <summary> �z�u�\�� </summary>
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
        /// <summary> �{�[�h�m�F���ꂽ�Ƃ��̃C�x���g </summary>
        public event System.Action<bool> OnChecked;

        //-------------------------------------------------------------------
        /* Methods */
        /// <summary> �������ꂽ�Ƃ��̏��� </summary>
        public void OnGenerated(Vector2Int pos)
        {
            Position = pos;
        }

        /// <summary> �z�u�\ </summary>

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
                // �}�X�̏�Ԃ��Z�b�g
                if (state == DiskState.empty) return;
                State = state;

                // �΂̐���
                diskObj = Runner.Spawn(diskPrefab, transform.position).GetComponent<Disk>();
                diskObj.transform.SetParent(transform);

                diskObj.SetDisk(state, !isGeneratedOnStart);              // �΂̏�ԃZ�b�g�A�A�j���[�V�����Ȃ�

                // �I��s�ɂ���
                InputProvider.InvalidateSelectable();

                if (!isGeneratedOnStart) {
                    // �^�[���ύX
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