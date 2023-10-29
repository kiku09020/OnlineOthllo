using GameUtils.SceneController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDebugger : MonoBehaviour {

    //--------------------------------------------------

    void Update()
    {
        if (!Debug.isDebugBuild) return;

        // 次のシーン
        if (Input.GetKeyDown(KeyCode.PageUp)) {
            SceneController.LoadNextScene();
        }

        // 前のシーン
        if (Input.GetKeyDown(KeyCode.PageDown)) {
            SceneController.LoadPrevScene();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneController.ReloadScene();
        }
    }
}
