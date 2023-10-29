using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
	/// <summary>
	/// ParticleManagerのPoolで取得されたパーティクルに追加されるコンポーネント。
	/// <para>パーティクルの再生が終了したときにPoolに戻す</para>
	/// </summary>
    public class ParticleCallback : MonoBehaviour
    {
		/// <summary> パーティクル停止時のイベント </summary>
		public event System.Action OnStopped;

		//--------------------------------------------------

		private void OnParticleSystemStopped()
		{
			OnStopped?.Invoke();
		}
	}
}
