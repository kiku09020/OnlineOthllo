using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
	using Disk;
    using Fusion;

    /// <summary> �{�[�h�Ǘ��N���X </summary>
    public class BoardManager : MonoBehaviour {
		/* Fields */
		const int row = 8;  // �s
		const int col = 8;  // ��

		[Header("Components")]
		[SerializeField, Tooltip("")] BoardCellGenerator cellGenerator;


		BoardCell[,] boardCells = new BoardCell[row, col];
		Vector2Int boardSize = new Vector2Int(row, col);

		//-------------------------------------------------------------------
		/* Properties */

		//-------------------------------------------------------------------
		/* Messages */
		void Awake()
		{
			
		}

		//-------------------------------------------------------------------
		/* Methods */
		/// <summary> �{�[�h�̏����z�u������ </summary>
		public void SetStartBoard(NetworkRunner runner)
		{
            boardCells = cellGenerator.GenerateCells(runner, boardSize);

            boardCells[3, 3].SetDiskState(runner, DiskState.white, true);
            boardCells[4, 3].SetDiskState(runner, DiskState.black, true);
            boardCells[3, 4].SetDiskState(runner, DiskState.black, true);
            boardCells[4, 4].SetDiskState(runner, DiskState.white, true);
        }
	}
}