namespace ManagerClasses
{
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.UI;
    using TMPro;
    using Players;

    public class SettingsManager : MonoBehaviour
    {
        public bool debug = false;
        public AudioMixer masterVolume;
        public Slider volumeSlider, sensitivitySlider;

        // Use this for initialization
        void OnEnable()
        {
            if(volumeSlider == null || sensitivitySlider == null)
            {
                Debug.LogError("Set up the Settings Manager!");
                Debug.Break();
            }
            //We adjust the sliders to match current volume and sensitivity
            float volume;
            masterVolume.GetFloat("Volume", out volume);
            volumeSlider.value = volume;
            sensitivitySlider.value = PlayerMove.sensitivity;
            if (debug)
            {
                Debug.Log("Volume " + volume);
                Debug.Log("Sensitivity " + PlayerMove.sensitivity);
            }
        }
        public void UpdateVolume()
        {
            //Set Float and Get Float are not working.
            float volume = volumeSlider.value;
            masterVolume.SetFloat("Volume", volume);
            volumeSlider.GetComponentInChildren<TextMeshProUGUI>().text = (80 +volume).ToString();

        }
        public void UpdateSensitivity()
        {
            float sensitivity = sensitivitySlider.value;
            PlayerMove.UpdateMoveDistance(sensitivity);
            sensitivitySlider.GetComponentInChildren<TextMeshProUGUI>().text = sensitivity.ToString();
        }
    }
}