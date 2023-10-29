using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EditorUtils
{
	[ExecuteInEditMode]
	public class ScriptTemplateGenerator : AssetPostprocessor
	{
		/* Constants */
		const string SETTINGS_PATH = "Assets/Plugins/ScriptTemplateGenerator/Data/ScriptTemplateGeneratorSettings.asset";
		const string MENU_PATH = "Assets/Create/ScriptTemplate/";                       // CreateMenuのパス

		const int MENU_PRIORITY = 80;                                                   // メニューの優先度

		static List<string> prevImportedTemplates = new List<string>();                 // 以前のimportedAssetsのリスト

		static ScriptTemplateGeneratorSettings settings;

		//--------------------------------------------------
		/* Messages */
		[InitializeOnLoadMethod]
		static void Initialize()
		{
			LoadSettings();

			EditorApplication.delayCall += AddTemplateMenuItem;
		}

		// ファイル変更時
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			if (importedAssets.Length == 0 && deletedAssets.Length == 0 && movedAssets.Length == 0 && movedFromAssetPaths.Length == 0) return;

			List<string> importedTemplates = new List<string>(GetTemplates(importedAssets));
			List<string> deletedTemplates = new List<string>(GetTemplates(deletedAssets));

			if (!deletedTemplates.Any(asset => asset.Contains(settings.TemplateDirPath)) &&
			   !importedTemplates.Any(asset => asset.Contains(settings.TemplateDirPath))) return;

			LoadSettings();

			// テンプレートフォルダ内のファイルが削除された場合、メニューアイテムを削除する
			if (deletedTemplates.Count != 0) {
				RemoveTemplateMenuItem(deletedTemplates);
			}

			// テンプレートフォルダ内のファイルが変更された場合、メニューを更新する
			if (importedTemplates.Count != 0) {
				AddTemplateMenuItem(importedTemplates);
			}

			prevImportedTemplates = importedTemplates;
			//----------------------------------------
			// フォルダ内のファイルを取得
			string[] GetTemplates(string[] assets)
			{
				var templates = new List<string>();

				foreach (var asset in assets) {
					if (asset.Contains(settings.TemplateDirPath)) {
						templates.Add(asset);
					}
				}

				return templates.ToArray();
			}
		}

		//--------------------------------------------------
		/* Methods */
		static void AddTemplateMenuItem()
		{ AddTemplateMenuItem(GetTemplateFileNames()); }

		// テンプレートファイルからメニューアイテムを作成
		static void AddTemplateMenuItem(IEnumerable<string> templates)
		{
			foreach (var file in templates) {
				var fileName = System.IO.Path.GetFileNameWithoutExtension(file);

				MenuHelper.AddMenuItem(MENU_PATH + fileName, MENU_PRIORITY, () => {
					CreateScriptFile(fileName, fileName);
				});

				Log($"{fileName} menuItem was added.");
			}

			// 名前が変更されたファイルを削除
			RemoveDifferences(templates);

			MenuHelper.Update();
		}

		// メニューアイテムからテンプレートファイルを削除
		static void RemoveTemplateMenuItem(IEnumerable<string> templates)
		{
			foreach (var file in templates) {
				var fileName = System.IO.Path.GetFileNameWithoutExtension(file);
				MenuHelper.RemoveMenuItem(MENU_PATH + fileName);

				Log($"{fileName} menuItem was removed.");
			}

			MenuHelper.Update();
		}

		// 名前変更されたファイルを削除
		static void RemoveDifferences(IEnumerable<string> templates)
		{
			var difference = prevImportedTemplates.Except(templates);
			RemoveTemplateMenuItem(difference);
		}

		//--------------------------------------------------
		// テンプレートファイルを取得
		static string[] GetTemplateFileNames()
		{
			// テンプレートフォルダが存在しない場合、作成する
			if(!System.IO.Directory.Exists(settings.TemplateDirPath)) {
				System.IO.Directory.CreateDirectory(settings.TemplateDirPath);
				Log(settings.TemplateDirPath + " was created.");
			}

			return System.IO.Directory.GetFiles(settings.TemplateDirPath, "*" + settings.TemplateFileExtension, System.IO.SearchOption.AllDirectories);
		}

		// テンプレートファイルから、スクリプトファイルを作成する
		static void CreateScriptFile(string templateFileName, string createdFileName)
		{
			string path = System.IO.Path.Combine(settings.TemplateDirPath, templateFileName);
			ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
				path + settings.TemplateFileExtension, createdFileName + settings.CreatedFileExtension);
		}

		//--------------------------------------------------
		/* Other */
		static void Log(string message)
		{
			if (settings.EnableDebugLog) {
				Debug.Log($"{nameof(ScriptTemplateGenerator)}: {message}");
			}
		}

		// ScriptableObjectを読み込む
		static void LoadSettings()
		{
			settings = AssetDatabase.LoadAssetAtPath<ScriptTemplateGeneratorSettings>(SETTINGS_PATH);

			Log("Settings was loaded.");
		}
	}
}
