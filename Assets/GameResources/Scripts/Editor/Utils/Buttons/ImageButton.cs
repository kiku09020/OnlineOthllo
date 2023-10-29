using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EditorUtils.Button
{
    public partial class ButtonUtils 
    {
		/// <summary> 画像付きのボタン </summary>
		/// <param name="texture">画像</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, Action action, params GUILayoutOption[] options)
		{ return ImageButton(texture, GUI.skin.button, default, action, options); }

		/// <summary> 画像付きのボタン </summary>
		/// <param name="texture">画像</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, GUIStyle style, Color color, Action action, params GUILayoutOption[] options)
		{ return ButtonBase(new GUIContent(texture), style, color, action, options); }

		/// <summary> 画像付きのボタン </summary>
		/// <param name="texture">画像</param>
		/// <param name="size">サイズ</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, Vector2 size, Action action, params GUILayoutOption[] options)
		{ return ImageButton(texture, GUI.skin.button, default, size, action, options); }

		/// <summary> 画像付きのボタン </summary>
		/// <param name="texture">画像</param>
		/// <param name="size">ボタンサイズ</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, GUIStyle style, Color color, Vector2 size, Action action, params GUILayoutOption[] options)
		{ return ButtonBase(new GUIContent(texture), style, color, size, action, options); }

		/// <summary> 画像付きのボタン(ツールチップつき) </summary>
		/// <param name="texture">画像</param>
		/// <param name="tooltip">説明</param>
		/// <param name="size">ボタンサイズ</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, string tooltip, GUIStyle style, Color color, Vector2 size, Action action, params GUILayoutOption[] options)
		{ return ButtonBase(new GUIContent(texture, tooltip), style, color, size, action, options); }

		/// <summary> 画像付きのボタン(ツールチップつき) </summary>
		/// <param name="texture">画像</param>
		/// <param name="tooltip">説明</param>
		/// <param name="size">ボタンサイズ</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, string tooltip, Color color, Vector2 size, Action action, params GUILayoutOption[] options)
		{ return ImageButton(texture, tooltip, GUI.skin.button, color, size, action, options); }

		/// <summary> 画像付きのボタン(ツールチップつき) </summary>
		/// <param name="texture">画像</param>
		/// <param name="tooltip">説明</param>
		/// <param name="size">ボタンサイズ</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, string tooltip, Vector2 size, Action action, params GUILayoutOption[] options)
		{ return ImageButton(texture, tooltip, GUI.skin.button, default, size, action, options); }

		/// <summary> 画像付きのボタン(ツールチップつき) </summary>
		/// <param name="texture">画像</param>
		/// <param name="tooltip">説明</param>
		/// <param name="action">ボタンが押されたときの処理</param>
		public static bool ImageButton(Texture texture, string tooltip, Action action, params GUILayoutOption[] options)
		{ return ButtonBase(new GUIContent(texture, tooltip), GUI.skin.button, default, action, options); }
	}
}