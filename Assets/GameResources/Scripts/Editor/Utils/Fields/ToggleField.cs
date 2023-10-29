using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorUtils.Fields
{
	public partial class FieldUtils
	{
		/* Toggle */
		public static void Toggle(ref bool value, string label, params GUILayoutOption[] options)
			=> Toggle(ref value, new GUIContent(label), GUI.skin.toggle, options);

		public static void Toggle(ref bool value, string label, string tooltip, params GUILayoutOption[] options)
			=> Toggle(ref value, new GUIContent(label, tooltip), GUI.skin.toggle, options);

		public static void Toggle(ref bool value, GUIContent content, params GUILayoutOption[] options)
			=> Toggle(ref value, content, GUI.skin.toggle, options);

		public static void Toggle(ref bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
			=> value = EditorGUILayout.Toggle(content, value, style, options);

		public static void ToggleLeft(ref bool value, string label, params GUILayoutOption[] options)
			=> value = EditorGUILayout.ToggleLeft(label, value, options);

		public static void ToggleLeft(ref bool value, string label, string tooltip, params GUILayoutOption[] options)
			=> value = EditorGUILayout.ToggleLeft(new GUIContent(label, tooltip), value, options);

		public static void ToggleLeft(ref bool value, GUIContent content, params GUILayoutOption[] options)
			=> value = EditorGUILayout.ToggleLeft(content, value, options);

		public static void ToggleLeft(ref bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
			=> value = EditorGUILayout.ToggleLeft(content, value, style, options);

	}
}