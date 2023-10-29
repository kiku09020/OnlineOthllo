using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GameUtils.SceneController
{
	/// <summary> シーン操作クラス </summary>
	public class SceneController
	{
		//--------------------------------------------------
		/* Properties */
		/// <summary> 現在のシーン番号 </summary>
		static int ActiveSceneIndex => SceneManager.GetActiveScene().buildIndex;

		//--------------------------------------------------
		/* Events */
		/// <summary> シーン変更時のイベントを追加する </summary>
		/// <param name="action">型引数：(現在のシーン、次のシーン)</param>
		public static void AddSceneChangedEvent(UnityAction<Scene, Scene> action)
		{ SceneManager.activeSceneChanged += action; }

		/// <summary> シーン変更時のイベントを削除する </summary>
		public static void RemoveSceneChangedEvent(UnityAction<Scene, Scene> action)
		{ SceneManager.activeSceneChanged -= action; }

		//--------------------------------------------------
		/* Checking Methods */
		// カスタムエラーハンドリング
		static System.Exception CustomException()
		{
			return new System.Exception("シーンが読み込めませんでした");
		}

		// シーン番号判定
		static bool CheckLoadable(int index)
		{
			if (index >= 0 && index < SceneManager.sceneCountInBuildSettings) {
				return true;
			}

			throw CustomException();
		}

		// シーン名判定
		static bool CheckLoadable(string name)
		{
			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
				var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
				var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

				if (name == sceneName) {
					return true;
				}
			}

			throw CustomException();
		}

		//--------------------------------------------------
		/* Loading Methods */
		/// <summary> シーン番号を指定してシーンを読み込む </summary>
		public static void LoadScene(int index, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (CheckLoadable(index)) {
				SceneManager.LoadScene(index, mode);
			}
		}

		/// <summary> シーン名を指定してシーンを読み込む </summary>
		public static void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (CheckLoadable(name)) {
				SceneManager.LoadScene(name, mode);
			}
		}

		/// <summary> 現在のシーンを再読み込みする </summary>
		public static void ReloadScene() => LoadScene(ActiveSceneIndex);

		/// <summary> 次のシーンを読み込む </summary>
		public static void LoadNextScene() => LoadScene(ActiveSceneIndex + 1);

		/// <summary> 前のシーンを読み込む </summary>
		public static void LoadPrevScene() => LoadScene(ActiveSceneIndex - 1);

		// --------------------------------------------------
		/* Async Loading Methods */
		/// <summary> シーン番号を指定して非同期でシーンを読み込む </summary>
		public static AsyncOperation LoadSceneAsync(int index, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (CheckLoadable(index)) {
				return SceneManager.LoadSceneAsync(index, mode);
			}

			return null;
		}

		/// <summary> シーン名を指定して非同期でシーンを読み込む </summary>
		public static AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (CheckLoadable(name)) {
				return SceneManager.LoadSceneAsync(name, mode);
			}

			return null;
		}

		/// <summary> 現在のシーンを非同期で再読み込みする </summary>
		public static AsyncOperation ReloadSceneAsync() => LoadSceneAsync(ActiveSceneIndex);

		/// <summary> 次のシーンを非同期で読み込む </summary>
		public static AsyncOperation LoadNextSceneAsync() => LoadSceneAsync(ActiveSceneIndex + 1);

		/// <summary> 前のシーンを非同期で読み込む </summary>
		public static AsyncOperation LoadPrevSceneAsync() => LoadSceneAsync(ActiveSceneIndex - 1);
	}
}