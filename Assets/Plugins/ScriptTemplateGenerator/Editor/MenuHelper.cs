using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;

namespace EditorUtils
{
	public static class MenuHelper
	{
		/* Add */
		public static void AddMenuItem(string name, Action action)
		{
			AddMenuItem(name, string.Empty, false, 0, action, null);
		}

		public static void AddMenuItem(string name, int priority, Action action)
		{
			AddMenuItem(name, string.Empty, false, priority, action, null);
		}

		public static void AddMenuItem(string name, string shortcut, bool isChecked, int priority, Action action, Func<bool> validate)
		{
			var menuMethod = typeof(Menu).GetMethod("AddMenuItem", BindingFlags.Static | BindingFlags.NonPublic);
			menuMethod?.Invoke(null, new object[] { name, shortcut, isChecked, priority, action, validate });
		}

		//--------------------------------------------------
		/* Remove */

		public static void RemoveMenuItem(string name)
		{
			var menuMethod = typeof(Menu).GetMethod("RemoveMenuItem", BindingFlags.Static | BindingFlags.NonPublic);
			menuMethod?.Invoke(null, new object[] { name });
		}

		//--------------------------------------------------
		/* Update */
		public static void Update()
		{
			var internalUpdateAllMenus = typeof(EditorUtility).GetMethod("Internal_UpdateAllMenus", BindingFlags.Static | BindingFlags.NonPublic);
			internalUpdateAllMenus?.Invoke(null, null);

			var shortcutIntegrationType = Type.GetType("UnityEditor.ShortcutManagement.ShortcutIntegration, UnityEditor.CoreModule");
			var instanceProp = shortcutIntegrationType?.GetProperty("instance", BindingFlags.Static | BindingFlags.Public);
			var instance = instanceProp?.GetValue(null);
			var rebuildShortcutsMethod = instance?.GetType().GetMethod("RebuildShortcuts", BindingFlags.Instance | BindingFlags.NonPublic);
			rebuildShortcutsMethod?.Invoke(instance, null);
		}
	}
}