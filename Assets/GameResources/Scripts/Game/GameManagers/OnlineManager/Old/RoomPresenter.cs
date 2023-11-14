using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if false
namespace GameController.Online {
    public class RoomPresenter : MonoBehaviour {
        /* Fields */
        [Header("Components")]
        [SerializeField, Tooltip("")] OnlineRoomManager roomManager;

        [Header("UI")]
        [SerializeField, Tooltip("")] TextMeshProUGUI playerCountText;

        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Messages */

        private void Awake()
        {
            roomManager.OnConnectedToRoom += SetPlayerCountText;
        }

        private void OnDestroy()
        {
            roomManager.OnConnectedToRoom -= SetPlayerCountText;
        }

        //-------------------------------------------------------------------
        /* Methods */
        void SetPlayerCountText(Room room)
        {
            playerCountText.text = $"PlayerCount:{room.PlayerCount} / {room.MaxPlayers}";
        }
    }
}

#endif