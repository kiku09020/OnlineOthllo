using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player {
	/// <summary> プレイヤー情報 </summary>
	public class PlayerNetworkData {
		/// <summary> プレイヤー名 </summary>
		public string NickName {
			get => PlayerPrefs.GetString("NickName", "Player");
			set => PlayerPrefs.SetString("NickName", value);
		}

		/// <summary> 先行かどうか </summary>
		public bool IsFirstTurn {
			get => PlayerPrefs.GetInt("IsFirst") == 1;
			set => PlayerPrefs.SetInt("IsFirst", (value == true) ? 1 : 0);
		}
	}

	public class PlayerObject : NetworkBehaviour {
		/* Fields */
		//-------------------------------------------------------------------
		/* Properties */
		public PlayerNetworkData NetworkData { get; set; } = new PlayerNetworkData();

		public int DiskCount { get; private set; }

		//-------------------------------------------------------------------
		/* Event */
		public event System.Action<PlayerNetworkData> OnInit;

		//-------------------------------------------------------------------
		/* Methods */
		public void SetData(string nickName, bool isFirstTurn)
		{
			NetworkData.NickName = nickName;
			NetworkData.IsFirstTurn = isFirstTurn;

			OnInit?.Invoke(NetworkData);
		}

		public void SetData(PlayerNetworkData data)
		{
			NetworkData = data;
			OnInit?.Invoke(NetworkData);
		}
	}
}