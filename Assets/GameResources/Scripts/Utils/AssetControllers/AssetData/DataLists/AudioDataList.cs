using GameUtils.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.Data.Asset {
	[CreateAssetMenu(menuName = "GameController/Audio/AudioDataList", fileName = "AudioDataList")]
	public class AudioDataList : AssetDataListBase<AssetDataUnit<AudioClip>, AudioClip> { }
}