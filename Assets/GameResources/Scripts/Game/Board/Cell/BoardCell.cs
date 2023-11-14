using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Game.Board {
    using Disk;
    using Fusion;

    /// <summary> �Ղ̃}�X </summary>
    public class BoardCell : SimulationBehaviour {
        /* Fields */
        [Header("Generating")]
        [SerializeField] Disk diskPrefab;

        [Header("Components")]
        [SerializeField] Collider col;

        Disk diskObj;

        //-------------------------------------------------------------------
        /* Properties */
        public Vector2Int Position { get; private set; }
        public DiskState State { get; private set; } = DiskState.empty;

        public IBoardCellInputEvents InputProvider { get; private set; }

        //-------------------------------------------------------------------
        /* Messages */
        private void Awake()
        {
            InputProvider = GetComponent<IBoardCellInputEvents>();
            InputProvider.ClickedUpEvent += OnClickedUpEventHandler;
        }

        //--------------------------------------------------
        /* Events */


        //-------------------------------------------------------------------
        /* Methods */
        /// <summary> �������ꂽ�Ƃ��̏��� </summary>
        public void OnGenerated(Vector2Int pos)
        {
            Position = pos;
        }

        public void SetDiskState(NetworkRunner runner, DiskState state, bool isGeneratedOnStart = false)
        {
            // �}�X�̏�Ԃ��Z�b�g
            if (state == DiskState.empty) return;
            State = state;

            // �΂̐���
            diskObj = runner.Spawn(diskPrefab, transform.position);
            diskObj.transform.localScale = new Vector3(1, 1, 1);
            diskObj.SetDisk(state, !isGeneratedOnStart, true);              // �΂̏�ԃZ�b�g�A�A�j���[�V�����Ȃ�

            //diskObj.OnCompleted += Completed;

            // �I��s�ɂ���
            InputProvider.IsSelectable = false;
        }

        //--------------------------------------------------
        void OnClickedUpEventHandler()
        {
            col.enabled = false;

            // �t�̐F�ɂ���
            SetDiskState(Runner, DiskState.black);
        }

        void Completed()
        {
            //diskObj.OnCompleted -= Completed;


        }
    }
}