using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
    using Cysharp.Threading.Tasks;
    using Disk;
    using Fusion;
    using Game.Player;
    using GameController.Game;
    using System.Linq;

    /// <summary> �{�[�h�Ǘ��N���X </summary>
    public class BoardManager : NetworkBehaviour {
        /* Fields */
        const int row = 8;  // �s
        const int col = 8;  // ��

        [Header("Components")]
        [SerializeField, Tooltip("")] BoardCellGenerator cellGenerator;
        [SerializeField] TurnManager turnManager;

        BoardCell[,] boardCells = new BoardCell[row, col];
        Vector2Int boardSize = new Vector2Int(row, col);

        int whiteDiskCount;
        int blackDiskCount;

        //-------------------------------------------------------------------
        /* Properties */
        [Networked, Capacity(col * row)]
        public NetworkArray<BoardCell> BoardCells { get; }

        //-------------------------------------------------------------------
        /* Messages */
        void Awake()
        {
            turnManager.OnTurnStart += RPC_CheckBoard;
        }

        //-------------------------------------------------------------------
        /* Methods */
        /// <summary> �{�[�h�̏����z�u������ </summary>
        public void SetStartBoard(NetworkRunner runner)
        {
            boardCells = cellGenerator.GenerateCells(runner, boardSize);

            boardCells[3, 3].RPC_SetDiskState(DiskState.white, true);
            boardCells[4, 3].RPC_SetDiskState(DiskState.black, true);
            boardCells[3, 4].RPC_SetDiskState(DiskState.black, true);
            boardCells[4, 4].RPC_SetDiskState(DiskState.white, true);

            RPC_CheckBoard(runner.GetPlayerObject(runner.LocalPlayer).GetComponent<PlayerObject>());
        }

        [Rpc(RpcSources.All,RpcTargets.All)]
        public void RPC_CheckBoard(PlayerObject currentPlayer)
        {
            if(!Object.HasStateAuthority) return;

            for (int y = 0; y < col; y++) {
                for (int x = 0; x < row; x++) {
                    BoardCells.Set(y * row + x, boardCells[y, x]);
                }
            }

            var puttableCells = new List<BoardCell>();

            for (int y = 0; y < boardCells.GetLength(0); y++) {
                for (int x = 0; x < boardCells.GetLength(1); x++) {
                    var currentCell = BoardCells.Get(y * row + x);

                    // ���ɒu����Ă����玟�̃}�X��
                    if (currentCell.State != DiskState.empty) continue;

                    // �e������T��
                    for (int dirY = -1; dirY <= 1; dirY++) {
                        for (int dirX = -1; dirX <= 1; dirX++) {
                            var reversibleDirCells = CheckDir(new Vector2Int(dirX, dirY), currentCell, currentPlayer);

                            currentCell.RPC_SetPuttable(reversibleDirCells != null);

                            // �z�u�\��������A�z�u�\�}�X�Ƃ��Ďw�肷��
                            if (reversibleDirCells != null) {
                                currentCell.RPC_SetPuttable(true);
                                puttableCells.Add(currentCell);

                                print(currentCell.Position);

                                break;
                            }
                        }
                    }
                }
            }
        }

        // �������𒲂ׂ�
        IEnumerable<BoardCell> CheckDir(Vector2Int dir, BoardCell currentCell, PlayerObject currentPlayer)
        {
            var reversibleCells = new List<BoardCell>();

            int count = 0;
            Vector2Int targetCellPos;
            BoardCell targetCell = null;

            // ���̕����̎��̃}�X������̃}�X��������A���Ԃ��\���X�g�ɒǉ�
            do {
                count++;
                targetCellPos = currentCell.Position + new Vector2Int(dir.x * count, dir.y * count);

                // �͈͊O�ɏo����I��
                if (targetCellPos.x < 0 || targetCellPos.x >= boardSize.x ||
                    targetCellPos.y < 0 || targetCellPos.y >= boardSize.y) {
                    return null;
                }

                targetCell = boardCells[targetCellPos.y, targetCellPos.x];

                reversibleCells.Add(targetCell);
            } while ((targetCell.State != DiskState.empty) && (targetCell.State != currentPlayer.PlayersDiskState));


            // �Ō�̃}�X���v���C���[�̐F��������A���߂�
            if (targetCell.State == currentPlayer.PlayersDiskState) {
                return reversibleCells;
            }

            // �Ȃ����null
            return null;
        }
    }
}