using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorUtils.Fields {
	public partial class FieldUtils {
		//--------------------------------------------------

		//--------------------------------------------------
		/* Text */
		/// <summary>
		/// 複数行の
		/// </summary>
		/// <param name="text"></param>
		/// <param name="options"></param>
		public static void TextArea(ref string text, params GUILayoutOption[] options)
			=> TextArea(ref text, GUI.skin.textArea, options);

		public static void TextArea(ref string text, string label, params GUILayoutOption[] options)
			=> TextArea(ref text, label, GUI.skin.textArea, options);

		public static void TextArea(ref string text, string label, GUIStyle style, params GUILayoutOption[] options)
		{
			LabelUtils.BasicLabel(label);
			TextArea(ref text, style, options);
		}

		public static void TextArea(ref string text, GUIStyle style, params GUILayoutOption[] options)
			=> text = EditorGUILayout.TextArea(text, style, options);

		public static void TextFiled(ref string text, string label, params GUILayoutOption[] options)
			=> TextFiled(ref text, label, GUI.skin.textField, options);

		public static void TextFiled(ref string text, string label, GUIStyle style, params GUILayoutOption[] options)
		{
			TextFiled(ref text, new GUIContent(label), style, options);
		}

		public static void TextFiled(ref string text, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
			=> text = EditorGUILayout.TextField(content, text, style, options);


		//--------------------------------------------------
		/**/
		public static void PrefixLabel(string label,ref int value)
		{
			value = EditorGUILayout.IntField(label, value);
		}

		//--------------------------------------------------
		/**/


		//--------------------------------------------------
		/**/
	}
}