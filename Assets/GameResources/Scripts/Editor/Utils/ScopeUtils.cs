using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorUtils
{
	/// <summary> GUIのまとまり(Scope)を作成するクラス </summary>
	public class ScopeUtils
	{
		const int DEFAULT_INDENT_LEVEL = 1;

		/* Base */
		#region Base
		// 指定された方向のScopeを取得する
		static GUI.Scope GetScope(EditorUtils.ContentDirection direction, bool isColored = false)
		{
			var style = isColored ? GUI.skin.box : GUIStyle.none;

			switch (direction) {
				case EditorUtils.ContentDirection.Horizontal:
					return new EditorGUILayout.HorizontalScope(style);
				case EditorUtils.ContentDirection.Vertical:
					return new EditorGUILayout.VerticalScope(style);
				default:
					return null;
			}
		}

		// Scope基底処理
		static void ScopeBase(EditorUtils.ContentDirection direction, Action action, bool isColored = false)
		{
			ScopeBase(direction, action, null, isColored);
		}

		static void ScopeBase(EditorUtils.ContentDirection direction, Action action, string label = null, bool isColored = false)
		{
			using (GetScope(direction, isColored)) {
				if (label != null) {
					EditorGUILayout.LabelField(label, EditorStyles.boldLabel);  // 見出し
				}

				action?.Invoke();
			}
		}



		static void ScopeBase(EditorUtils.ContentDirection direction, Color color, Action action, string label = null)
		{
			ColorUtils.SetGUIBackColor(color, () => {
				using (GetScope(direction, true)) {
					if (label != null) {
						EditorGUILayout.LabelField(label, EditorStyles.boldLabel);  // 見出し
					}

					action?.Invoke();
				}
			});
		}

		#endregion
		//-------------------------------------------------------------------
		/* Horizontal Scopes */
		#region HorizontalScopes
		/// <summary> 水平方向にGUIを並べる </summary>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <param name="isColored">着色するか</param>
		public static void HorizontalScope(Action guiDrawnAction, bool isColored = false)
			=> ScopeBase(EditorUtils.ContentDirection.Horizontal, guiDrawnAction, isColored);

		/// <summary> 色が指定されたHorizontalScope </summary>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <param name="color">色</param>
		public static void HorizontalScope(Action guiDrawnAction, Color color)
			=> ScopeBase(EditorUtils.ContentDirection.Horizontal, color, guiDrawnAction);

		/// <summary> ラベル付きのHorizontalScope </summary>
		/// <param name="label">ラベル</param>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <param name="isColored">着色するか</param>
		public static void HorizontalScope(string label, Action guiDrawnAction, bool isColored = false)
			=> ScopeBase(EditorUtils.ContentDirection.Horizontal, guiDrawnAction, label, isColored);

		/// <summary> ラベルと色が指定されたHorizontalScope </summary>
		/// <param name="label">ラベル</param>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		public static void HorizontalScope(string label, Action guiDrawnAction, Color color)
			=> ScopeBase(EditorUtils.ContentDirection.Horizontal, color, guiDrawnAction, label);

		#endregion
		//-------------------------------------------------------------------
		/* Vertical Scopes */
		#region VerticalScopes
		/// <summary> 垂直方向にGUIを並べる </summary>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <param name="considerHeight">高さを考慮するか</param>
		public static void VerticalScope(Action guiDrawnAction, bool isColored = false)
			=> ScopeBase(EditorUtils.ContentDirection.Vertical, guiDrawnAction, isColored);

		/// <summary>
		/// 色が指定されたVerticalScope
		/// </summary>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <param name="color">色</param>
		public static void VerticalScope(Action guiDrawnAction, Color color)
		=> ScopeBase(EditorUtils.ContentDirection.Vertical, color, guiDrawnAction);

		/// <summary> ラベル付きのVerticalScope </summary>
		/// <param name="label">ラベル</param>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		/// <param name="isColored">着色するか</param>
		public static void VerticalScope(string label, Action guiDrawnAction, bool isColored = false)
			=> ScopeBase(EditorUtils.ContentDirection.Vertical, guiDrawnAction, label, isColored);

		/// <summary> ラベルと色が指定されたVerticalScope </summary>
		/// <param name="label">ラベル</param>
		/// <param name="guiDrawnAction">GUI描画処理</param>
		public static void VerticalScope(string label, Action guiDrawnAction, Color color)
			=> ScopeBase(EditorUtils.ContentDirection.Vertical, color, guiDrawnAction, label);

		#endregion
		//-------------------------------------------------------------------
		/* Scroll Scopes */
		#region ScrollScopes
		/// <summary> スクロール可能なScope </summary>
		/// <param name="scrollPosition">スクロール位置</param>
		/// <param name="scopeSize">スクロール範囲サイズ</param>
		/// <param name="action">GUI描画処理</param>
		public static void ScrollScope(ref Vector2 scrollPosition, Vector2 scopeSize, Action action)
		{
			ScrollScope(ref scrollPosition, scopeSize, EditorUtils.ContentDirection.Vertical, action);
		}

		static void ScrollScope(ref Vector2 scrollPosition, Vector2 scopeSize, EditorUtils.ContentDirection direction, Action action)
		{
			using (var scrollView = new EditorGUILayout.ScrollViewScope(
				scrollPosition, GUILayout.Width(scopeSize.x), GUILayout.Height(scopeSize.y))) {

				using (GetScope(direction)) {
					scrollPosition = scrollView.scrollPosition;     // スクロール位置を更新
					action?.Invoke();
				}
			}
		}

		/// <summary> 垂直方向にスクロール可能なScope </summary>
		/// <param name="scrollPosition">スクロール位置</param>
		/// <param name="height">スクロール高さ</param>
		/// <param name="action">GUI描画処理</param>
		public static void VerticalScrollScope(ref Vector2 scrollPosition, float height, Action action)
			=> ScrollScope(ref scrollPosition, new Vector2(0, height), EditorUtils.ContentDirection.Vertical, action);

		/// <summary> 水平方向にスクロール可能なScope </summary>
		/// <param name="scrollPosition">スクロール位置</param>
		/// <param name="width">スクロール幅</param>
		/// <param name="action">GUI描画処理</param>
		public static void HorizontalScrollScope(ref Vector2 scrollPosition, float width, Action action)
			=> ScrollScope(ref scrollPosition, new Vector2(width, 0), EditorUtils.ContentDirection.Horizontal, action);

		#endregion
		//-------------------------------------------------------------------
		/* Toggle Scopes */
		/// <summary> トグルでGUIの有効化/無効化が可能なScope </summary>
		/// <param name="toggle">トグル</param>
		/// <param name="scopeName">表示されるスコープ名</param>
		/// <param name="action">GUI描画処理</param>
		public static void ToggleScope(ref bool toggle, string scopeName, Action action)
		=> ToggleScope(ref toggle, scopeName, default, action);

		/// <summary> トグルでGUIの有効化/無効化が可能なScope </summary>
		/// <param name="toggle">トグル</param>
		/// <param name="scopeName">表示されるスコープ名</param>
		/// <param name="action">GUI描画処理</param>
		public static void ToggleScope(ref bool toggle, string scopeName, Color color, Action action)
		{
			using (var toggleScope = new EditorGUILayout.ToggleGroupScope(scopeName, toggle)) {
				toggle = toggleScope.enabled;

				IndentableScope(() => {
					action?.Invoke();
				}, color);
			}
		}

		/// <summary> GUIを無効化するScope </summary>
		/// <param name="isValid">有効か</param>
		/// <param name="action">GUI描画処理</param>
		public static void DisabledScope(bool isValid, Action action)
			=> DisabledScope(isValid, null, action);

		/// <summary> GUIを無効化するScope </summary>
		/// <param name="isValid">有効か</param>
		/// <param name="scopeName">スコープ名</param>
		/// <param name="action">GUI描画処理</param>
		public static void DisabledScope(bool isValid, string scopeName, Action action)
			=> DisabledScope(isValid, scopeName, default, action);

		/// <summary> GUIを無効化するScope </summary>
		/// <param name="isValid">有効か</param>
		/// <param name="scopeName">スコープ名</param>
		/// <param name="color">色</param>
		/// <param name="action">GUI描画処理</param>
		public static void DisabledScope(bool isValid, string scopeName, Color color, Action action)
		{
			using (new EditorGUI.DisabledScope(isValid)) {
				EditorGUILayout.LabelField(scopeName, EditorStyles.boldLabel);

				IndentableScope(() => {
					action?.Invoke();
				}, color);
			}
		}

		/// <summary> 開閉できるScope </summary>
		/// <param name="isOpen">開閉状態</param>
		/// <param name="scopeName">スコープ名</param>
		/// <param name="action">GUI描画処理</param>
		/// <param name="labelClick">ラベルをクリックしても開くか</param>
		public static void OpenedScope(ref bool isOpen, string scopeName, Action action, bool labelClick = true)
			=> OpenedScope(ref isOpen, scopeName, default, action, labelClick);

		/// <summary> 開閉できるScope </summary>
		/// <param name="isOpen">開閉状態</param>
		/// <param name="scopeName">スコープ名</param>
		/// <param name="action">GUI描画処理</param>
		/// <param name="labelClick">ラベルをクリックしても開くか</param>
		public static void OpenedScope(ref bool isOpen, string scopeName, Color color, Action action, bool labelClick = true)
			=> OpenedScope(ref isOpen, scopeName, color, EditorStyles.foldout, action, labelClick);
		
		/// <summary> 開閉できるScope </summary>
		/// <param name="isOpen">開閉状態</param>
		/// <param name="scopeName">スコープ名</param>
		/// <param name="action">GUI描画処理</param>
		/// <param name="labelClick">ラベルをクリックしても開くか</param>
		public static void OpenedScope(ref bool isOpen, string scopeName, Color color, GUIStyle style, Action action, bool labelClick = true)
		{
			isOpen = EditorGUILayout.Foldout(isOpen, scopeName, labelClick, style);

			if (isOpen) {
				IndentableScope(() => {
					action?.Invoke();
				}, color);
			}
		}

		//-------------------------------------------------------------------
		/* Other Scopes */
		/// <summary> GUIをインデントできるScope </summary>
		/// <param name="action">GUI描画処理</param>
		/// <param name="indentLevel">インデント幅</param>
		public static void IndentableScope(Action action, int indentLevel = DEFAULT_INDENT_LEVEL)
			=> IndentableScope(action, default, indentLevel);

		/// <summary> GUIをインデントできるScope </summary>
		/// <param name="action">GUI描画処理</param>
		/// <param name="color">色</param>
		/// <param name="indentLevel">インデント幅</param>
		public static void IndentableScope(Action action, Color color, int indentLevel = DEFAULT_INDENT_LEVEL)
		{
			using (new EditorGUI.IndentLevelScope(indentLevel)) {
				VerticalScope(() => {
					action?.Invoke();
				}, color);
			}
		}
	}
}