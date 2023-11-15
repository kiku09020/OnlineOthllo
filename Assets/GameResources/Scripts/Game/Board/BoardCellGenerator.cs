using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
    /// <summary> �{�[�h�}�X�I�u�W�F�N�g�����N���X </summary>
    public class BoardCellGenerator : MonoBehaviour {
        /* Fields */
        [Header("Generate")]
        [SerializeField, Tooltip("")] NetworkPrefabRef cellPrefab;
        [SerializeField, Tooltip("")] Transform boardTransform;

        [Header("Parameters")]
        [SerializeField, Tooltip("�����J�n�ʒu(�Ղ̍���̃}�X�̃��[���h���W)")]
        Vector3 genStartWorldPos = new Vector3(-3.5f, .38f, 3.5f);

        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Messages */

        //-------------------------------------------------------------------
        /* Methods */
        /// <summary> �}�X�̐��� </summary>
        public BoardCell[,] GenerateCells(NetworkRunner runner, Vector2Int boardSize)
        {
            var cells = new BoardCell[boardSize.y, boardSize.x];

            for (int y = 0; y < boardSize.y; y++) {
                for (int x = 0; x < boardSize.x; x++) {
                    var boardPos = new Vector2Int(x, y);
                    var cellObj = runner.Spawn(cellPrefab, inputAuthority: runner.LocalPlayer).GetComponent<BoardCell>();             // ����
                    cellObj.transform.SetParent(boardTransform);

                    SetCellWorldPosition(cellObj, boardPos);            // ���[���h���W�w��
                    cellObj.OnGenerated(boardPos);                      // �}�X�̔퐶������

                    cells[y, x] = cellObj;
                }
            }

            return cells;
        }

        // �}�X�̃��[���h���W���w�肷��
        void SetCellWorldPosition(BoardCell cell, Vector2Int boardPos)
        {
            float x = genStartWorldPos.x + boardPos.x;
            float y = genStartWorldPos.y;
            float z = genStartWorldPos.z - boardPos.y;

            cell.transform.position = new Vector3(x, y, z);
        }
    }
}