using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.Audio {
	public class SEManager : AudioManager<SEManager> {
		[Header("Settings")]
		[SerializeField, Tooltip("PlayOneShotで、複数の音声の再生を可能にするか")]
		bool isOneShot = true;

		//--------------------------------------------------
		protected override void PlayAudio_Derived(AudioClip clip)
		{
			source.clip = clip;

			if (isOneShot) {
				source.PlayOneShot(clip);
			}

			else {
				source.Play();
			}
		}

		/// <summary> 遅延つきで再生する </summary>
		public void PlayDelayed(string audioName, float delay)
		{
			SetAudioClip(audioName);
			source.PlayDelayed(delay);
		}

		//-------------------------------------------------------------------
		/* Randomized SE */
		/// <summary> 完全ランダムな効果音を再生する </summary>
		public void PlayRandomSE(bool isParamReset = false)
		{
			PlayAudio(GetRandomClip(0, assetDataList.DataDictionary.Count), isParamReset);
		}

		/// <summary> 範囲指定でランダムな効果音を再生する </summary>
		public void PlayRandomSE(int rangeMin, int rangeMax, bool isParamReset = false)
		{
			PlayAudio(GetRandomClip(rangeMin, rangeMax), isParamReset);
		}

		// 指定された範囲の効果音をランダムで取得する
		AudioClip GetRandomClip(int rangeMin, int rangeMax)
		{
			if (rangeMin < 0 || rangeMax >= assetDataList.DataDictionary.Count) {
				throw new System.Exception("範囲が不正です");
			}

			int randomIndex = Random.Range(rangeMin, rangeMax);
			AudioClip randomClip = assetDataList.DataList[randomIndex].AssetData;

			return randomClip;
		}

		//-------------------------------------------------------------------
		/* Audio Settings */
		/// <summary> 一度に複数の音声を再生可能にするか </summary>
		public SEManager SetOneShot(bool oneShot = true) { isOneShot = oneShot; return this; }

		/// <summary> ピッチを範囲指定でランダムにする </summary>
		public SEManager SetRandomPitch(float min = 0.5f, float max = 1) { source.pitch = Random.Range(min, max); return this; }
	}
}