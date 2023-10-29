using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.Data.Asset
{


	[CreateAssetMenu(menuName = "GameController/Particle/ParticleDataList", fileName = "ParticleDataList")]
	public class ParticleDataList : AssetDataListBase<ParticleAssetUnit, ParticleSystem> { }

}