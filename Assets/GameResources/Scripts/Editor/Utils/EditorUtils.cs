using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Extentions;

namespace EditorUtils
{
	public class EditorUtils : Editor
	{
		/* Content Direction */
		/// <summary> GUIの並ぶ方向 </summary>
		public enum ContentDirection
		{
			Horizontal,
			Vertical,
		}

		//-------------------------------------------------------------------
		/* Line */
		const float DEFAULT_LINE_THICKNESS = 3;

		/// <summary> 区切り線の挿入 </summary>
		public static void AddLine(float thickness = DEFAULT_LINE_THICKNESS)
		=> AddLine(Color.black, thickness);

		/// <summary> 区切り線の挿入 </summary>
		public static void AddLine(Color color, float thickness = DEFAULT_LINE_THICKNESS)
		{
			ColorUtils.SetGUIColor(color, () => {
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(thickness));
			});
		}
	}
}