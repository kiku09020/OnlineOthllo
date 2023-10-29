using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using EditorUtils;
using Extentions.Editor;

using EditorUtils.Fields;
using EditorUtils.Button;

public class EditorTest : EditorWindow
{
	Vector2 scrollPos;
	Vector2 horizontalScrollPos;

	bool toggle;
	bool openedToggle;
	bool buttonOpenedToggle;
	bool textFieldOpenedToggle;

	bool toggleOpenedToggle;
	bool toggleFiledToggle1;
	bool toggleFiledToggle2;


	int intValue;

	static Texture buttonImage;

	string textAreaText = "YEAHYEAH";

	//--------------------------------------------------
	[MenuItem("Window/EditorTest")]
	static void ShowWindow()
	{
		var window = GetWindow<EditorTest>();
		window.titleContent = new GUIContent("EditorTest", buttonImage);
	}

	private void OnEnable()
	{
		Debug.Log("Enable");

		buttonImage = AssetDatabaseExtentions.LoadAssetAtPath<Texture>("Assets/GameAssets/Textures/EditorTextures/SmileIcon.png");
	}

	private void OnGUI()
	{
		ScopeUtils.ScrollScope(ref scrollPos, position.size, () => {
			for (int i = 0; i < 10; i++) {
				ScopeUtils.HorizontalScope(() => {
					GUILayout.FlexibleSpace();

					EditorGUILayout.LabelField("label");

					EditorGUILayout.LabelField(new GUIContent(buttonImage));

					ButtonUtils.BasicButton("Button", () => { });

					GUILayout.FlexibleSpace();
				});
			}

			ScopeUtils.HorizontalScrollScope(ref horizontalScrollPos, 200, () => {
				for (int i = 0; i < 10; i++) {
					EditorGUILayout.LabelField("label");
				}
			});

			ScopeUtils.ToggleScope(ref toggle, "Test", Color.red, () => {
				EditorGUILayout.LabelField("label");

				ScopeUtils.DisabledScope(false, "Hello", () => {
					EditorGUILayout.LabelField("label");

					ScopeUtils.DisabledScope(true, () => {
						EditorGUILayout.LabelField("label");
					});
				});
			});

			EditorUtils.EditorUtils.AddLine(Color.blue);

			// Labels
			ScopeUtils.OpenedScope(ref openedToggle, "Labels", Color.red, () => {
				LabelUtils.BoldLabel("BoldLabel");
				LabelUtils.HeaderLabel("AAA", Color.blue);
				LabelUtils.HeaderLabel("Header");
			});

			// Fields
			ScopeUtils.OpenedScope(ref textFieldOpenedToggle, "TextFields", () => {
				// Toggle
				ScopeUtils.OpenedScope(ref toggleOpenedToggle, "ToggleField", () => {
					FieldUtils.Toggle(ref toggleFiledToggle1, "Toggle");
					FieldUtils.ToggleLeft(ref toggleFiledToggle2, "Toggle");
				});

				FieldUtils.TextFiled(ref textAreaText, "TextAreaText");

				FieldUtils.TextArea(ref textAreaText, "TextAreaText", GUILayout.Height(100));

				FieldUtils.PrefixLabel("A", ref intValue);
			});

			// Buttons
			ScopeUtils.OpenedScope(ref buttonOpenedToggle, "Buttons", () => {
				ButtonUtils.BasicButton("Button", () => {

				});

				// Image Buttons
				ScopeUtils.HorizontalScope(() => {
					ButtonUtils.ImageButton(buttonImage, () => {
						Debug.Log("Pushed");
					}, GUILayout.Width(32), GUILayout.Height(32));

					ButtonUtils.ImageButton(buttonImage,"この顔可愛いね", new Vector2(48, 48), () => {
						Debug.Log("おされたー");
					});
				});

				// Mini Buttons
				ButtonUtils.MiniButtons(new Dictionary<string, System.Action>() {
					{ "Button1", () => Debug.Log("Clicked Left Button") },
					{ "Button2", () => Debug.Log("Clicked middle Button") },
					{ "Button3", () => Debug.Log("Clicked Right Button") }
				});

				ButtonUtils.MiniButtons("Button", new System.Action[]{
					() => Debug.Log("Left"),
					() => Debug.Log("Right")
				});

				ButtonUtils.MiniButtons("Button", default, 100, new System.Action[] {
					() => Debug.Log("Left"),
					() => Debug.Log("Middle"),
					() => Debug.Log("Middle"),
					() => Debug.Log("Middle"),
					() => Debug.Log("Middle"),
					() => Debug.Log("Right")
				});
			});
		});
	}
}

