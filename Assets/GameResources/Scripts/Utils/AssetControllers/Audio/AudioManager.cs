using GameUtils.Data.Asset;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.Audio {
	[RequireComponent(typeof(AudioSource))]
	public abstract class AudioManager<T> : AssetDataManagerBase<AssetDataUnit<AudioClip>, AudioClip,T> where T : AudioManager<T> {
		/* Fields */
		[Header("Components")]
		protected AudioSource source;

		AudioFilterController filterController;

		//-------------------------------------------------------------------
		/* Properties */
		public bool IsPlaying => source.isPlaying;

		/// <summary> フィルター操作クラス </summary>
		public AudioFilterController FilterController {
			get {
				// なければ自動で追加
				if(filterController == null) {
					filterController = gameObject.AddComponent<AudioFilterController>();
				}

				return filterController;
			}
		}

		//-------------------------------------------------------------------
		/* Messages */
		protected override void Awake()
		{
			base.Awake();

			source = GetComponent<AudioSource>();
		}

		protected virtual void OnDestroy()
		{
			managers.Clear();
		}

		//-------------------------------------------------------------------
		/* Play */
		/// <summary> 音声の再生処理用抽象メソッド </summary>
		protected abstract void PlayAudio_Derived(AudioClip clip);

		/// <summary> Clipを指定して、音声を再生する </summary>
		protected void PlayAudio(AudioClip clip, bool resetParams = false)
		{
			// パラメータリセット
			if (resetParams) {
				ResetSourceParameters();
			}

			// 再生処理
			PlayAudio_Derived(clip);
		}

		/// <summary> 音声名を指定して、音声を再生する </summary>
		public T PlayAudio(string audioName, bool resetParams = false)
		{ PlayAudio(GetAudioClip(audioName), resetParams); return this as T; }

		/// <summary> データリストから指定された名前のAudioClipを取得する </summary>
		protected AudioClip GetAudioClip(string audioName)
		{
			if (assetDataList.DataDictionary.TryGetValue(audioName, out var dataUnit)) {
				return dataUnit.AssetData;
			}

			throw new Exception("指定された名前の音声データが存在しません");
		}

		protected void SetAudioClip(string audioName)
		{
			source.clip = GetAudioClip(audioName);
		}

		//-------------------------------------------------------------------
		/* Pause, Mute */
		/// <summary> 音声を一時停止する </summary>
		public void Pause() { source.Pause(); }

		/// <summary> 音声の一時停止を解除する </summary>
		public void UnPause() { source.UnPause(); }

		/// <summary> ミュートにする </summary>
		public void Mute() { source.mute = true; }

		/// <summary> ミュート解除 </summary>
		public void Unmute() { source.mute = false; }

		//-------------------------------------------------------------------
		/* Audio Settings */
		/// <summary> ループ指定 </summary>
		public T SetLoop(bool loop = true) { source.loop = loop; return this as T; }

		/// <summary> 音量指定 </summary>
		public T SetVolume(float volume) { source.volume = volume; return this as T; }

		/// <summary> ピッチ指定 </summary>
		public T SetPitch(float pitch) { source.pitch = pitch; return this as T; }

		/// <summary> 優先度指定 </summary>
		public T SetPriority(int priority) { source.priority = priority; return this as T; }

		/// <summary> パンを指定 </summary>
		public T SetPanStereo(float pan) { source.panStereo = pan; return this as T; }

		/// <summary> パラメータリセット </summary>
		public void ResetSourceParameters()
		{
			source.loop = false;
			source.volume = 1;
			source.pitch = 1;
			source.priority = 128;
			source.panStereo = 0;
		}

		//-------------------------------------------------------------------
		/* Static */
		/// <summary> 全ての音声を一時停止する </summary>
		public static void PauseAllAudio() => managers.ForEach(manager => manager.Pause());

		/// <summary> 全ての音声の一時停止を解除する </summary>
		public static void UnPauseAllAudio() => managers.ForEach(manager => manager.UnPause());

		/// <summary> 全ての音声をミュートにする </summary>
		public static void MuteAllAudio() => managers.ForEach(manager => manager.Mute());

		/// <summary> 全ての音声のミュートを解除する </summary>
		public static void UnmuteAllAudio() => managers.ForEach(manager => manager.Unmute());
	}
}