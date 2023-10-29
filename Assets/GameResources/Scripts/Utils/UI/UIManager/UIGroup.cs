using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.UI {
    public class UIGroup : MonoBehaviour {

		/// <summary> UIの初期化 </summary>
		public virtual void Initialize() { }

		/// <summary> UIを非表示にする </summary>
		public virtual void Hide() => gameObject.SetActive(false);

		/// <summary> UIを表示する </summary>
		public virtual void Show() => gameObject.SetActive(true);
	}
}