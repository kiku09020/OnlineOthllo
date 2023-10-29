using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorUtils.Button
{
	public partial class ButtonUtils
	{
		/* Base */
		#region Base
		static bool ButtonBase(GUIContent content, GUIStyle style, Color color, Action action, params GUILayoutOption[] options)
		{
			bool isClicked = false;

			ColorUtils.SetGUIBackColor(color, () => {
				isClicked = GUILayout.Button(content, style, options);

				if (isClicked) {
					action?.Invoke();
				}
			});

			return isClicked;
		}

		static bool ButtonBase(GUIContent content, GUIStyle style, Color color, Vector2 size, Action action, params GUILayoutOption[] options)
		{
			var optionsList = options.ToList();
			optionsList.Add(GUILayout.Width(size.x));
			optionsList.Add(GUILayout.Height(size.y));

			return ButtonBase(content, style, color, action, optionsList.ToArray());
		}

		#endregion
	}
}