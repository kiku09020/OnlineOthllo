using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
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
		/// <summary> �������ꂽ�Ƃ��̏��� </summary>
		public void OnGenerated(Vector2Int pos)
		{
			Position = pos;
		}

		[Rpc(RpcSources.All, RpcTargets.All)]
		public void RPC_SetDiskState(DiskState state, bool isGeneratedOnStart = false)
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

				// �^�[���ύX
				
			}
		}

		//--------------------------------------------------
		void OnClickedUpEventHandler()
		{
			col.enabled = false;

			// �t�̐F�ɂ���
			RPC_SetDiskState(DiskState.black);
		}
	}
}