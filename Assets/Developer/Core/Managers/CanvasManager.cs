namespace ManagerClasses { 
using UnityEngine;

    public class CanvasManager : MonoBehaviour
    {
        public void ToggleAutoRotate()
        {
            CameraAIv2.autoRotate = !CameraAIv2.autoRotate;
            foreach (CameraAIv2 cam in FindObjectsOfType<CameraAIv2>())
            {
                cam.targetPosHidden = GameObject.Find("HiddenTarget").GetComponent<FollowMouse>();
                cam.moveTarget = GameObject.Find("PlayerCameraTarget").transform;
                    //cam.targetPosHidden.transform;
            }
        }
        public void ToggleInvertY()
        {
            CameraAIv2.invertY = !CameraAIv2.invertY;
        }
        public void ShowBestiary()
        {
            if (Manager.GameState == ManagerStates.Paused)
            {
                Manager.bestiary.SetActive(!Manager.bestiary.activeSelf);
            }
        }
        public void ShowSettings()
        {
            if (Manager.GameState == ManagerStates.Paused)
            {
                Manager.settingsPanel.SetActive(!Manager.settingsPanel.activeSelf);
            }
        }
        public void Quit()
        {
            MultiplayerManager.ClearPlayers();
            if (Application.isEditor)
            {
                Debug.Break();
            }
            else
            {
                Application.Quit();
            }
        }
        void Update()
        {
            if (Manager.GameState != ManagerStates.GameOver && Manager.GameState != ManagerStates.Shopping)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    Manager.Pause();
                }
            }
        }
    }
}
