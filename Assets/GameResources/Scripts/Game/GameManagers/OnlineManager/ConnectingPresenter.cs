using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

namespace GameController.Online
{
    /// <summary> ƒZƒbƒVƒ‡ƒ“UI </summary>
    public class ConnectingPresenter : MonoBehaviour
    {
        /* Fields */
        [Header("Components")]
        [SerializeField] SessionConnecter sessionManager;

        [Header("UI")]
        [SerializeField] TextMeshProUGUI playerCountText;

        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Messages */
        void Awake()
        {
            sessionManager.OnConnected += SetPlayerCountText;
        }

        //-------------------------------------------------------------------
        /* Methods */
        void SetPlayerCountText(NetworkRunner runner)
        {
            playerCountText.text = $"PlayerCount:{sessionManager.GetPlayerCountText(runner)}";
        }
    }
}