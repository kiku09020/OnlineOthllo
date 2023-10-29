using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extentions
{
	/// <summary> レイヤー拡張機能 </summary>
	public static class LayerExtention
	{
		const int LAYERMASK_ALL = -1;

		//-------------------------------------------------------------------
		/* Methods */
		/// <summary> 指定した番号のレイヤーが含まれているか </summary>
		public static bool Contain(int id)
		{
			return ((1 << id) & LAYERMASK_ALL) != 0;
		}

		/// <summary> 指定した名前のレイヤーが含まれているか </summary>
		public static bool Contain(string name)
		{
			return Contain(LayerMask.NameToLayer(name));
		}

		/// <summary> 指定されたレイヤーが、指定されたレイヤー名か </summary>
		public static bool CheckLayerName(int layer, string name)
		{
			return layer == LayerMask.NameToLayer(name);
		}

		/// <summary> 指定されたレイヤーが、指定されたレイヤー名か </summary>
		public static bool CheckLayerNames(int layer, params string[] names)
		{
			foreach (var name in names) {
				if (CheckLayerName(layer, name)) {
					return true;
				}
			}

			return false;
		}

		/// <summary> LayerMaskに、複数指定したレイヤー名のいずれかが含まれているか </summary>
		public static bool Contain(params string[] names)
		{
			foreach (string name in names) {
				if (Contain(name)) {
					return true;
				}
			}

			return false;
		}

		/// <summary> LayerMaskに、指定したレイヤー名が全て含まれているか </summary>
		public static bool ContainAll(params string[] names)
		{
			foreach (string name in names) {
				if (!Contain(name)) {
					return false;
				}
			}

			return true;
		}

		//-------------------------------------------------------------------
		/* Extention Methods */
		/// <summary> LayerMaskに、指定した番号のレイヤーが含まれているか </summary>
		public static bool Contain(this LayerMask self, int id)
		{
			return ((1 << id) & self) != 0;
		}

		/// <summary>LayerMaskに、 指定した名前のレイヤーが含まれているか </summary>
		public static bool Contain(this LayerMask self, string name)
		{
			return self.Contain(LayerMask.NameToLayer(name));
		}

		/// <summary> LayerMaskに、複数指定したレイヤー名のいずれかが含まれているか </summary>
		public static bool Contain(this LayerMask self, params string[] names)
		{
			foreach (string name in names) {
				if (self.Contain(name)) {
					return true;
				}
			}

			return false;
		}

		/// <summary> LayerMaskに、指定したレイヤー名が全て含まれているか </summary>
		public static bool ContainAll(this LayerMask self, params string[] names)
		{
			foreach (string name in names) {
				if (!self.Contain(name)) {
					return false;
				}
			}

			return true;
		}

		//-------------------------------------------------------------------
	}
}