using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;

namespace EditorUtils.Button
{
    public partial class ButtonUtils
    {
		/// <summary> 複数並んだボタン </summary>
		/// <param name="lablels">各ボタンのラベルの配列</param>
		/// <param name="width">ボタン幅</param>
		/// <param name="actions">各ボタンが押されたときの処理の配列</param>
		public static bool[] MiniButtons(string[] lablels, Color color, float width, Action[] actions, params GUILayoutOption[] options)
		{
			var optionsList = options.ToList();
			optionsList.Add(GUILayout.Width(width));

			return MiniButtons(lablels, color, actions, optionsList.ToArray());
		}

		/// <summary> 複数並んだボタン </summary>
		/// <param name="labels">各ボタンのラベルの配列</param>
		/// <param name="actions">ボタンが押されたときの処理</param>
		public static bool[] MiniButtons(string[] labels, Color color, Action[] actions, params GUILayoutOption[] options)
		{
			if (labels.Length == 0) {
				Debug.LogError("ラベルがありません");
				return null;
			}

			// Actionの数が足りない場合はnullを追加
			if (labels.Length > actions.Length) {
				var actionList = actions.ToList();
				actionList.AddRange(Enumerable.Repeat<Action>(null, labels.Length - actions.Length));
				actions = actionList.ToArray();
			}

			List<bool> isClickedList = new List<bool>();

			// ボタン1つのみ
			if (labels.Length == 1) {
				bool isClicked = ButtonBase(new GUIContent(labels[0]), EditorStyles.miniButtonMid,
		   color, actions[0], options);

				isClickedList.Add(isClicked);
				return isClickedList.ToArray();
			}

			ScopeUtils.HorizontalScope(() => {
				// left
				bool isClickedLeft = ButtonBase(new GUIContent(labels[0]), EditorStyles.miniButtonLeft,
													color, actions[0], options);
				isClickedList.Add(isClickedLeft);

				// middle
				if (labels.Length > 2) {
					for (int i = 1; i < labels.Length - 1; i++) {
						bool isClickedMiddle = ButtonBase(new GUIContent(labels[i]), EditorStyles.miniButtonMid,
															color, actions[i], options);
						isClickedList.Add(isClickedMiddle);
					}
				}

				// right
				bool isClickedRight = ButtonBase(new GUIContent(labels[labels.Length - 1]), EditorStyles.miniButtonRight,
													color, actions[labels.Length - 1], options);
				isClickedList.Add(isClickedRight);
			});

			return isClickedList.ToArray();
		}

		/// <summary> 複数並んだボタン </summary>
		/// <param name="labels">各ボタンのラベルの配列</param>
		/// <param name="actions">各ボタンが押された時の処理の配列</param>
		public static bool[] MiniButtons(string[] labels, Action[] actions, params GUILayoutOption[] options)
		{ return MiniButtons(labels, default, actions, options); }

		/// <summary> 複数並んだボタン。labelとactionをDictionaryで指定する </summary>
		/// <param name="labelAndActions">各ボタンのラベルと押されたときの処理のDictionary</param>
		/// <param name="width">ボタン幅</param>
		public static bool[] MiniButtons(Dictionary<string, Action> labelAndActions, Color color, float width, params GUILayoutOption[] options)
		{
			var labels = labelAndActions.Keys.ToArray();
			var actions = labelAndActions.Values.ToArray();

			return MiniButtons(labels, color, width, actions, options);
		}

		/// <summary> 複数並んだボタン。labelとactionをDictionaryで指定する </summary>
		/// <param name="labelAndActions">各ボタンのラベルと押されたときの処理のDictionary</param>
		public static bool[] MiniButtons(Dictionary<string, Action> labelAndActions, Color color, params GUILayoutOption[] options)
		{
			var labels = labelAndActions.Keys.ToArray();
			var actions = labelAndActions.Values.ToArray();

			return MiniButtons(labels, color, actions, options);
		}

		/// <summary> 複数並んだボタン。labelとactionをDictionaryで指定する </summary>
		/// <param name="labelAndActions">各ボタンのラベルと押されたときの処理のDictionary</param>
		public static bool[] MiniButtons(Dictionary<string, Action> labelAndActions, params GUILayoutOption[] options)
		{ return MiniButtons(labelAndActions, default, options); }

		/// <summary> 複数並んだボタン。ラベルに番号がつく </summary>
		/// <param name="label">ラベル(左から順に自動で番号が付けられる)</param>
		/// <param name="width">ボタン幅</param>
		/// <param name="actions">各ボタンが押されたときの処理の配列</param>
		public static bool[] MiniButtons(string label, Color color, float width, Action[] actions, params GUILayoutOption[] options)
		{
			var optionsList = options.ToList();
			optionsList.Add(GUILayout.Width(width));

			return MiniButtons(label, color, actions, optionsList.ToArray());
		}

		/// <summary> 複数並んだボタン。ラベルに番号がつく </summary>
		/// <param name="label">ラベル</param>
		/// <param name="actions">各ボタンが押されたときの処理の配列</param>
		public static bool[] MiniButtons(string label, Color color, Action[] actions, params GUILayoutOption[] options)
		{
			var labels = new string[actions.Length];

			for (int i = 0; i < actions.Length; i++) {
				labels[i] = $"{label}({i})";
			}

			return MiniButtons(labels, color, actions, options);
		}

		/// <summary> 複数並んだボタン。ラベルに番号がつく </summary>
		/// <param name="label">ラベル</param>
		/// <param name="actions">各ボタンが押されたときの処理の配列</param>
		public static bool[] MiniButtons(string label, Action[] actions, params GUILayoutOption[] options)
		{ return MiniButtons(label, default, actions, options); }
	}
}