using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System;

namespace EditorUtils
{
	public class ColorUtils
	{
		/* Default Colors */
		public static readonly Color DEFAULT_COLOR = GUI.color;                            // デフォルト色
		public static readonly Color DEFAULT_BACKGROUND_COLOR = GUI.backgroundColor;       // デフォルト背景色
		public static readonly Color DEFAULT_CONTENT_COLOR = GUI.contentColor;             // デフォルトコンテンツ色

		//-------------------------------------------------------------------
		/* Methods */
		/// <summary> GUIの色を変更する </summary>
		/// <param name="color">色</param>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		public static void SetGUIColor(Color color, Action guiDrawnAction)
		{
			if (color == default) {
				color = DEFAULT_COLOR;
			}

			GUI.color = color;
			guiDrawnAction?.Invoke();
			GUI.color = DEFAULT_COLOR;
		}

		/// <summary> GUIの背景色を変更する </summary>
		/// <param name="color">色</param>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <remarks>ex:ボタンのボタン部分など</remarks>
		public static void SetGUIBackColor(Color color, Action guiDrawnAction)
		{
			if (color == default) {
				color = DEFAULT_BACKGROUND_COLOR;
			}

			GUI.backgroundColor = color;
			guiDrawnAction?.Invoke();
			GUI.backgroundColor = DEFAULT_BACKGROUND_COLOR;
		}

		/// <summary> GUIのコンテンツ色を変更する </summary>
		/// <param name="color">色</param>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <remarks>ex:ボタンのテキスト部分など</remarks>
		public static void SetGUIContentColor(Color color, Action guiDrawnAction)
		{
			if (color == default) {
				color = DEFAULT_CONTENT_COLOR;
			}

			GUI.contentColor = color;
			guiDrawnAction?.Invoke();
			GUI.contentColor = DEFAULT_CONTENT_COLOR;
		}
	}
}