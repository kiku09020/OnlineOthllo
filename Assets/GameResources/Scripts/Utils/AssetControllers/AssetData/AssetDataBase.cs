using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils.Data.Asset {

	/// <summary> AssetListで管理される最小単位のデータ </summary>
	/// <typeparam name="Asset">管理されるアセット</typeparam>
	[System.Serializable]
	public class AssetDataUnit<Asset> where Asset : Object {
		[SerializeField] string name;
		[SerializeField] Asset assetData;

		public string Name => name;
		public Asset AssetData => assetData;

		/// <summary> アセット名をセット </summary>
		public void SetName()
		{
			if (assetData != null) {
				name = assetData.name;
			}

			else {
				name = "^-^";
			}
		}
	}

	/// <summary> アセットをScriptableObjectでリストで管理するクラス </summary>
	/// <typeparam name="AssetData">最小単位</typeparam>
	/// <typeparam name="Asset">管理対象のアセット</typeparam>
	public class AssetDataListBase<AssetData, Asset> : ScriptableObject
		where AssetData : AssetDataUnit<Asset> where Asset : Object {

		/* Fields */
		[SerializeField] List<AssetData> dataList = new List<AssetData>();
		Dictionary<string, AssetData> dataDictionary = new Dictionary<string, AssetData>();

		//--------------------------------------------------
		/* Properties */
		public IReadOnlyList<AssetData> DataList => dataList;
		public IReadOnlyDictionary<string, AssetData> DataDictionary => dataDictionary;

		//--------------------------------------------------
		/* Messages */
		private void OnEnable()
		{
			dataDictionary = dataList.ToDictionary(dataUnit => dataUnit.Name);
		}

		private void OnValidate()
		{
			dataList.ForEach(dataUnit => dataUnit.SetName());
		}
	}

	/// <summary> アセット管理基底クラス </summary>
	/// <typeparam name="AssetData">最小単位</typeparam>
	/// <typeparam name="Asset">管理対象のアセット</typeparam>
	public class AssetDataManagerBase<AssetData, Asset,T> : MonoBehaviour
		where AssetData : AssetDataUnit<Asset> where Asset : Object
		where T:AssetDataManagerBase<AssetData,Asset,T>{
		[SerializeField] protected AssetDataListBase<AssetData, Asset> assetDataList;

		protected static List<T> managers = new List<T>();

		protected virtual void Awake()
		{
			managers.Add(this as T);
		}
	}

}