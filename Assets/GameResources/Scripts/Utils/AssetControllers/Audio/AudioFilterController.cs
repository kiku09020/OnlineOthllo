using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.Audio
{
	/// <summary> AudioFilter操作クラス </summary>
	[RequireComponent(typeof(AudioBehaviour))]
	public class AudioFilterController : MonoBehaviour
	{
		/// <summary> Filterの有効化/無効化 </summary>
		/// <typeparam name="T">Type of filter</typeparam>
		public void SetFiltersEnable<T>(bool enable) where T : Behaviour
		{
			var filter = gameObject.GetComponent<T>();

			if (filter != null) {
				filter.enabled = enable;
			}
		}

		//--------------------------------------------------
		/// <summary> 高い周波数をカットするフィルターを追加する </summary>
		/// <param name="frequency">カットする周波数(Hz)</param>
		/// <param name="resonanceQ">自己共振の減衰値?</param>
		public void AddLowPassFilter(float cutoffFrequency = 5000, float resonanceQ = 1)
		{
			var filter = gameObject.AddComponent<AudioLowPassFilter>();

			filter.cutoffFrequency = cutoffFrequency;
			filter.lowpassResonanceQ = resonanceQ;
		}
		
		/// <summary> 低い周波数をカットするフィルターを追加する </summary>
		/// <param name="frequency">カットする周波数(Hz)</param>
		/// <param name="resonanceQ">自己共振の減衰値?</param>
		public void AddHighPassFilter(float cutoffFrequency = 5000, float resonanceQ = 1)
		{
			var filter = gameObject.AddComponent<AudioHighPassFilter>();

			filter.cutoffFrequency = cutoffFrequency;
			filter.highpassResonanceQ = resonanceQ;
		}



	}
}
