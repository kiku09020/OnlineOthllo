using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Game.Board {
    using Disk;
    using Fusion;

    /// <summary> 盤のマス </summary>
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
        /// <summary> 生成されたときの処理 </summary>
        public void OnGenerated(Vector2Int pos)
        {
            Position = pos;
        }

        public void SetDiskState(NetworkRunner runner, DiskState state, bool isGeneratedOnStart = false)
        {
            // マスの状態をセット
            if (state == DiskState.empty) return;
            State = state;

            // 石の生成
            diskObj = runner.Spawn(diskPrefab, transform.position);
            diskObj.transform.localScale = new Vector3(1, 1, 1);
            diskObj.SetDisk(state, !isGeneratedOnStart, true);              // 石の状態セット、アニメーションなど

            //diskObj.OnCompleted += Completed;

            // 選択不可にする
            InputProvider.IsSelectable = false;
        }

        //--------------------------------------------------
        void OnClickedUpEventHandler()
        {
            col.enabled = false;

            // 逆の色にする
            SetDiskState(Runner, DiskState.black);
        }

        void Completed()
        {
            //diskObj.OnCompleted -= Completed;


        }
    }
}