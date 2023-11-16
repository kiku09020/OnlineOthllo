using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
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
		public Vector2Int Position { get; private set; }

		[Networked]
		public DiskState State { get; private set; } = DiskState.empty;

		public BoardCellInputProvider InputProvider { get; private set; }

		//-------------------------------------------------------------------
		/* Messages */
		public override void Spawned()
		{
			InputProvider = GetComponent<BoardCellInputProvider>();
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

		[Rpc(RpcSources.All, RpcTargets.All)]
		public void RPC_SetDiskState(DiskState state, bool isGeneratedOnStart = false)
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

				// ターン変更
				
			}
		}

		//--------------------------------------------------
		void OnClickedUpEventHandler()
		{
			col.enabled = false;

			// 逆の色にする
			RPC_SetDiskState(DiskState.black);
		}
	}
}