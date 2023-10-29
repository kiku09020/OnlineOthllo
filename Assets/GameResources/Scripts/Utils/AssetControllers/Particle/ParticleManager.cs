using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils.Data.Asset;
using UnityEngine.Pool;

namespace GameUtils.Particle
{
	public class ParticleManager : AssetDataManagerBase<ParticleAssetUnit, ParticleSystem, ParticleManager>
	{
		[Header("ObjectPool")]
		[SerializeField] int defaultCapacity = 10;
		[SerializeField] int maxSize = 20;

		Dictionary<string, ObjectPool<ParticleSystem>> pools = new Dictionary<string, ObjectPool<ParticleSystem>>();
		static Dictionary<string, ParticleSystem> activeParticles = new Dictionary<string, ParticleSystem>();

		//--------------------------------------------------

		protected override void Awake()
		{
			activeParticles.Clear();

			base.Awake();

			foreach (var dataUnit in assetDataList.DataList) {
				var pool = new ObjectPool<ParticleSystem>(
					() => OnCreate(dataUnit),
					(ParticleSystem particle) => OnGet(particle, dataUnit.IsSetParent),
					OnRelease, OnDest, true, defaultCapacity, maxSize
					);

				pools.Add(dataUnit.Name, pool);
			}
		}

		//--------------------------------------------------
		/* ObjectPool */
		ParticleSystem OnCreate(ParticleAssetUnit particleAssetUnit)
		{
			var obj = Instantiate(particleAssetUnit.AssetData);

			obj.name += $"({pools[particleAssetUnit.Name].CountAll})";

			// 設定
			var main = obj.main;

			// コールバックの設定
			if (!main.loop && particleAssetUnit.IsStopOnRelease) {
				main.stopAction = ParticleSystemStopAction.Callback;
				var callBack = obj.gameObject.AddComponent<ParticleCallback>();
				callBack.OnStopped += () => pools[particleAssetUnit.Name].Release(obj);
			}

			return obj;
		}

		void OnGet(ParticleSystem particle, bool isSetParent)
		{
			if (isSetParent) {
				particle.transform.SetParent(transform);
			}

			particle.gameObject.SetActive(true);
			particle.transform.position = Vector3.zero;
			particle.transform.localPosition = Vector3.zero;

			activeParticles.Add(particle.gameObject.name, particle);
		}

		void OnRelease(ParticleSystem particle)
		{
			particle.gameObject.SetActive(false);

			activeParticles.Remove(particle.gameObject.name);
		}

		void OnDest(ParticleSystem particle)
		{
			Destroy(particle.gameObject);
		}

		/// <summary> パーティクル名を指定して生成 </summary>
		public ParticleSystem GenerateParticle(string name)
		{
			return pools[name].Get();
		}
		//--------------------------------------------------

		/// <summary> パーティクル削除後、再生 </summary>
		public void RestartParticle(ParticleSystem particle)
		{
			particle.Clear();
			particle.Play();
		}

		/// <summary> 全てのパーティクルの一時停止/再生 </summary>
		/// <param name="pause">true:Pause / false:Unpause</param>
		public static void PauseAllParticles(bool pause = true)
		{
			// 一時停止
			if (pause) {
				foreach (var particle in activeParticles.Values) {
					particle.Pause();
				}
			}

			// 一時停止終了
			else {
				foreach (var particle in activeParticles.Values) {
					particle.Play();
				}
			}
		}
	}
}