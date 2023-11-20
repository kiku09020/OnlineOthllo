using Cysharp.Threading.Tasks;
using Fusion;
using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Online {
    public class PlayerManager : MonoBehaviour {
        /* Fields */
        [Header("PlayerObject")]
        [SerializeField] NetworkPrefabRef playerObjectPrefab;
        [SerializeField] Transform playerParent;


        //-------------------------------------------------------------------
        /* Properties */
        public List<PlayerObject> Players { get; private set; }

        //-------------------------------------------------------------------
        /* Messages */
        public event System.Func<NetworkRunner, PlayerRef, bool> OnSetTurnOrder;
        public event System.Action<PlayerObject> OnSetData;

        //-------------------------------------------------------------------
        /* Methods */
        public async UniTask SpawnPlayer(NetworkRunner runner)
        {
            // ���[�J���v���C���[�ݒ�
            var playerUIObj = runner.Spawn(playerObjectPrefab);
            runner.SetPlayerObject(runner.LocalPlayer, playerUIObj);

            // �S�Ẵv���C���[�I�u�W�F�N�g�����������܂őҋ@  
            foreach (var player in runner.ActivePlayers) {
                await UniTask.WaitUntil(() => runner.TryGetPlayerObject(player, out var playerObj));
            }

            // �e�v���C���[�̐ݒ�
            foreach (var player in runner.ActivePlayers) {
                var plObj = runner.GetPlayerObject(player);

                SetPlayerObject(plObj, runner, player);
            }
        }

        void SetPlayerObject(NetworkObject playerUIObj, NetworkRunner runner, PlayerRef player)
        {
            playerUIObj.transform.SetParent(playerParent);
            var playerRT = playerUIObj.GetComponent<RectTransform>();

            // �f�[�^�ݒ�
            var playerObj = playerUIObj.GetComponent<PlayerObject>();

            // �^�[�������߂�
            if (OnSetTurnOrder != null)
                playerObj.IsFirstTurn = OnSetTurnOrder.Invoke(runner, player);


            // ���g�̂Ƃ��A�����ɕ\��
            if (player == runner.LocalPlayer) {
                playerObj.PersonalName = "���Ԃ�";

                playerRT.anchorMin = Vector2.zero;
                playerRT.anchorMax = Vector2.zero;
                playerRT.pivot = Vector2.zero;
                playerRT.anchoredPosition = new Vector3(32, 32, 0);
            }

            // ����̂Ƃ��A�E��ɕ\��
            else {
                playerObj.PersonalName = "������";

                playerRT.anchorMin = Vector2.one;
                playerRT.anchorMax = Vector2.one;
                playerRT.pivot = Vector2.one;
                playerRT.anchoredPosition = new Vector3(-32, -32, 0);
            }

            playerObj.SetDataSync(playerObj.NetworkData);
            OnSetData?.Invoke(playerObj);
        }
    }
}