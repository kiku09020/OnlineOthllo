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

    /// <summary> ボード管理クラス </summary>
    public class BoardManager : NetworkBehaviour {
        /* Fields */
        const int row = 8;  // 行
        const int col = 8;  // 列

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
        /// <summary> ボードの初期配置をする </summary>
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

                    // 既に置かれていたら次のマスへ
                    if (currentCell.State != DiskState.empty) continue;

                    // 各方向を探索
                    for (int dirY = -1; dirY <= 1; dirY++) {
                        for (int dirX = -1; dirX <= 1; dirX++) {
                            var reversibleDirCells = CheckDir(new Vector2Int(dirX, dirY), currentCell, currentPlayer);

                            currentCell.RPC_SetPuttable(reversibleDirCells != null);

                            // 配置可能だったら、配置可能マスとして指定する
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

        // 一定方向を調べる
        IEnumerable<BoardCell> CheckDir(Vector2Int dir, BoardCell currentCell, PlayerObject currentPlayer)
        {
            var reversibleCells = new List<BoardCell>();

            int count = 0;
            Vector2Int targetCellPos;
            BoardCell targetCell = null;

            // その方向の次のマスが相手のマスだったら、裏返し可能リストに追加
            do {
                count++;
                targetCellPos = currentCell.Position + new Vector2Int(dir.x * count, dir.y * count);

                // 範囲外に出たら終了
                if (targetCellPos.x < 0 || targetCellPos.x >= boardSize.x ||
                    targetCellPos.y < 0 || targetCellPos.y >= boardSize.y) {
                    return null;
                }

                targetCell = boardCells[targetCellPos.y, targetCellPos.x];

                reversibleCells.Add(targetCell);
            } while ((targetCell.State != DiskState.empty) && (targetCell.State != currentPlayer.PlayersDiskState));


            // 最後のマスがプレイヤーの色だったら、挟める
            if (targetCell.State == currentPlayer.PlayersDiskState) {
                return reversibleCells;
            }

            // なければnull
            return null;
        }
    }
}