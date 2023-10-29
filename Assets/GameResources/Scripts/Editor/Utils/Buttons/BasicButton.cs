using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EditorUtils.Button
{
    public partial class ButtonUtils
    {
		/// <summary> シンプルなボタン </summary>
		/// <param name="buttonLabel">ラベル</param>
		/// <param name="pressedAction">ボタンが押されたときの処理</param>
		public static bool BasicButton(string buttonLabel, Action pressedAction)
		{ return BasicButton(buttonLabel, default, pressedAction); }

		/// <summary> シンプルなボタン </summary>
		/// <param name="buttonLabel">ラベル</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool BasicButton(string buttonLabel, Color color, Action action)
		{ return BasicButton(buttonLabel, GUI.skin.button, color, action, null); }

		/// <summary> シンプルなボタン </summary>
		/// <param name="buttonLabel">ラベル</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool BasicButton(string buttonLabel, GUIStyle style, Color color, Action action, GUILayoutOption[] options)
		{ return ButtonBase(new GUIContent(buttonLabel), style, color, action, options); }

		/// <summary> シンプルなボタン </summary>
		/// <param name="buttonLabel">ラベル</param>
		/// <param name="size">ボタンのサイズ</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		/// <returns></returns>
		public static bool BasicButton(string buttonLabel, GUIStyle style, Color color, Vector2 size, Action action, params GUILayoutOption[] options)
		{
			return ButtonBase(new GUIContent(buttonLabel), style, color, size, action, options);
		}
	}
}