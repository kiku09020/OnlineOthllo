using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

namespace GameController.Online
{
    /// <summary> �Z�b�V����UI </summary>
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