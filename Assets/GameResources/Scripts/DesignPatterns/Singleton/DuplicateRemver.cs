using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Singleton {

	/// <summary> 
	/// 継承せずに使えるシングルトン。
	/// <para>※重複削除用。インスタンスの静的参照とかはできないよ</para>
	/// </summary>
    public class DuplicateRemver<T> : Object where T:Component {

        //--------------------------------------------------

        static T instance;

		// インスタンスの重複削除
		public void RemoveDuplicates(T self)
		{
			// なければ登録
			if (instance == null) {
				instance = self;

				// 親から抜ける
				if (self.transform.parent != null) {
					self.transform.SetParent(null);
				}

				DontDestroyOnLoad(self.gameObject);
			}

			// 既にあれば、相手を削除
			else {
				Debug.LogError($"*{self} was destroyed.");
				Destroy(self.gameObject);
			}
		}
	}
}