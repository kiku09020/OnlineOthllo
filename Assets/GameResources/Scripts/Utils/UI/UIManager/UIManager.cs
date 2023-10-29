using GameUtils.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.UI {
    public class UIManager : MonoBehaviour {
        [SerializeField, Tooltip("初めに有効になるUIGroup")] 
        UIGroup startUIGroup;
        [SerializeField] List<UIGroup> _uiGroupList = new List<UIGroup>();

        [SerializeField,Tooltip("最初に全て非表示にするか")] 
        bool hideAllAtStart;

        static readonly List<UIGroup> uiGroupList = new List<UIGroup>();
        static UIGroup currentUIGroup;                                      // 現在有効なUIGroup
        
        static Stack<UIGroup> histroy = new Stack<UIGroup>();               // UIGroupの履歴

        //--------------------------------------------------
        private void Awake()
        {
            ResetUIHistory();                   // 履歴リセット

            foreach (var uiGroup in _uiGroupList) {
                uiGroup.Initialize();
                uiGroupList.Add(uiGroup);       // 登録

                if (hideAllAtStart) {
                    uiGroup.Hide();             // 登録されたUIを、全て非表示にする
                }
            }

            // 初期UIGroupを表示
            if (startUIGroup != null) {
                ShowUIGroup(startUIGroup);
            }
        }

        private void OnDestroy()
        {
            uiGroupList.Clear();
        }

        //--------------------------------------------------
        /// <summary> <typeparamref name="T"/>型のUIGroupを取得する </summary>
        public static T GetUIGroup<T>() where T : UIGroup
        {
            // T型のUIGroupを検索する
            foreach (var uiGroup in uiGroupList) {
                if (uiGroup is T targetUI) {
                    return targetUI;
                }
            }

            return null;
        }

        //--------------------------------------------------
        /// <summary> UIGroupを表示する </summary>
        /// <param name="remember">履歴に残すか</param>
        public static void ShowUIGroup<T>(bool remember = true) where T : UIGroup
        {
            foreach (var uiGroup in uiGroupList) {
                if (uiGroup is T) {
                    if (currentUIGroup) {
                        // 履歴に残す場合、Stackに追加
                        if (remember) {
                            histroy.Push(currentUIGroup);
                        }

                        currentUIGroup.Hide();      // 現在のUIを非表示にする
                    }

                    uiGroup.Show();                 // UI表示
                    currentUIGroup = uiGroup;       // 現在のUIを指定されたUIにする

                    ShowCommon();
                    return;
                }
            }
        }

        /// <summary> UIGroupを表示する </summary>
        /// <param name="remember">履歴に残すか</param>
        public static void ShowUIGroup(UIGroup uiGroup, bool remember = true)
        {
            if (currentUIGroup) {
                if (remember) {
                    histroy.Push(currentUIGroup);
                }

                currentUIGroup.Hide();
            }

            uiGroup.Show();
            currentUIGroup = uiGroup;

            ShowCommon();
        }

        /// <summary> 一つ前のUIGroupを表示する </summary>
        public static void ShowLastUIGroup()
        {
            if (histroy.Count != 0) {
                ShowUIGroup(histroy.Pop(), false);      // 履歴から取り出して表示
            }
        }

        //--------------------------------------------------

        /// <summary> UIGroupを非表示にする </summary>
        public static void HideUIGroup(UIGroup uigroup)
        {
            uigroup.Hide();

            currentUIGroup = null;
        }

        /// <summary> UIGroupを非表示にする </summary>
        public static void HideUIGroup<T>() where T : UIGroup
        {
            foreach (var uiGroup in uiGroupList) {
                if (uiGroup is T) {
                    HideUIGroup(uiGroup);
                }
            }
        }

        /// <summary> 全てのUIを非表示にする </summary>
        public static void HideAllUIGroups()
        {
            currentUIGroup = null;      // 現在のUIをリセット

            foreach (var uiGroup in uiGroupList) {
                uiGroup.Hide();
            }
        }
        //--------------------------------------------------
        /// <summary> 履歴をリセットする </summary>
        public static void ResetUIHistory()
        {
            histroy.Clear();
        }

        //-------------------------------------------
        // 共通処理
        static void ShowCommon()
        {
            print($"{nameof(currentUIGroup)} = {currentUIGroup}");
        }
    }
}