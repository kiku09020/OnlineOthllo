using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.Data.Asset
{
	[System.Serializable]
	public class ParticleAssetUnit : AssetDataUnit<ParticleSystem>
	{
		[Header("Settings")]
		[SerializeField, Tooltip("親のオブジェクトの子として生成する")]
		bool isSetParent;
		[SerializeField, Tooltip("再生終了時にオブジェクトプールに戻す")]
		bool isStopOnRelease = true;

		public bool IsSetParent => isSetParent;
		public bool IsStopOnRelease => isStopOnRelease;
	}
}