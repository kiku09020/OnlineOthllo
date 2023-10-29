using DesignPatterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.Audio {
	public class BGMManager : AudioManager<BGMManager> {
		[Header("Settings")]
		[SerializeField, Tooltip("シーンまたいで再生するか")]
		bool dontDestroyOnLoad = true;

		DuplicateRemver<BGMManager> singleton = new DuplicateRemver<BGMManager>();

		//--------------------------------------------------
		/* Messages */
		protected override void Awake()
		{
			base.Awake();

			if (dontDestroyOnLoad) {
				singleton.RemoveDuplicates(this);
			}
		}

		//--------------------------------------------------
		/* Methods */
		protected override void PlayAudio_Derived(AudioClip clip)
		{
			source.clip = clip;
			source.Play();

			SetLoop();
		}
	}
}