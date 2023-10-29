using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorUtils
{
	public class LabelUtils
	{
		const int HEADER_LABEL_FONT_SIZE = 24;      // 見出しラベルのフォントサイズ

		//--------------------------------------------------
		/* Basic Label */
		#region Basic Label
		public static void BasicLabel(string label)
			=> BasicLabel(label, default);

		public static void BasicLabel(string label, Color color)
			=> BasicLabel(label, color, GUI.skin.label);

		public static void BasicLabel(string label, Color color, GUIStyle style, params GUILayoutOption[] options)
		{
			ColorUtils.SetGUIColor(color, () => {
				EditorGUILayout.LabelField(label, style, options);
			});
		}

		public static void BasicLabel(string label, string tooltip, GUIStyle style, params GUILayoutOption[] options)
			=> EditorGUILayout.LabelField(new GUIContent(label, tooltip), style, options);

		#endregion
		//--------------------------------------------------
		/* Double Label */
		#region Double Label
		/// <summary> 2つのラベル </summary>
		public static void DoubleLabel(string leftLabel, string rightLabel, params GUILayoutOption[] options)
			=> DoubleLabel(leftLabel, rightLabel, GUI.skin.label, options);

		/// <summary> 2つのラベル </summary>

		public static void DoubleLabel(string leftLabel, string rightLabel, GUIStyle style, params GUILayoutOption[] options)
			=> EditorGUILayout.LabelField(leftLabel, rightLabel, style, options);

		/// <summary> 2つのラベル </summary>
		public static void DoubleLabel(string leftLabel, string rightLabel, string tooltip, GUIStyle style, params GUILayoutOption[] options)
		{
			var labelContent = new GUIContent(leftLabel, tooltip);
			EditorGUILayout.LabelField(labelContent, new GUIContent(rightLabel), style, options);
		}

		#endregion
		//--------------------------------------------------
		/* Bold Label */
		#region Bold Label
		/// <summary> 太字のラベル </summary>
		public static void BoldLabel(string label, params GUILayoutOption[] options)
			=> BoldLabel(label, default, options);

		/// <summary> 太字のラベル </summary>
		public static void BoldLabel(string label, Color color, params GUILayoutOption[] options)
			=> BasicLabel(label, color, EditorStyles.boldLabel, options);

		/// <summary> 見出しラベル </summary>
		public static void HeaderLabel(string label, Color color, params GUILayoutOption[] options)
			=> BasicLabel(label, color, GetHeaderLabelStyle(new GUIStyle(EditorStyles.largeLabel), new GUIContent(label)), options);

		/// <summary> 見出しラベル </summary>
		public static void HeaderLabel(string label, params GUILayoutOption[] options)
			=> HeaderLabel(label, default, options);

		#endregion
		//--------------------------------------------------
		/* Other */
		/// <summary> 選択可能なラベル </summary>
		public static void SelectableLabel(string label, GUIStyle style, Color color, params GUILayoutOption[] options)
		{
			ColorUtils.SetGUIColor(color, () => {
				SelectableLabel(label, style, options);
			});
		}

		/// <summary> 選択可能なラベル </summary>
		public static void SelectableLabel(string label, GUIStyle style, params GUILayoutOption[] options)
		{
			EditorGUILayout.SelectableLabel(label, style, options);
		}

		/// <summary> 選択可能なラベル </summary>
		public static void SelectableLabel(string label, params GUILayoutOption[] options)
		{
			EditorGUILayout.SelectableLabel(label, options);
		}

		//--------------------------------------------------
		/* Getter */
		/// <summary> 見出しラベルのStyleを取得する </summary>
		public static GUIStyle GetHeaderLabelStyle(GUIStyle style, GUIContent content)
		{
			style.fontSize = HEADER_LABEL_FONT_SIZE;
			style.fontStyle = FontStyle.Bold;
			style.fixedWidth = style.CalcSize(content).x;
			style.fixedHeight = style.CalcHeight(content, style.fixedWidth);

			return style;
		}
	}
}