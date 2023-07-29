using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitlesOnKey : MonoBehaviour {
    static Messenger subtitles;
    public DialogueCutscene scene;
    public string line;
    public AudioClip sound;
    public Character speaker;
    public SubtitlesOnKey nextLine;
    public string inputKey;
    // Update is called once per frame
    float timeElapsed;
	void Update () {
		if(timeElapsed < speakTime)
        {
            timeElapsed += Time.time;
        }
        else if (Input.GetKeyDown(inputKey))
        {
            NextAction();
        }
	}
    float speakTime = 0;
    public void Action()
    {
        if(subtitles == null)
        {
            subtitles = GameObject.Find("Subtitles").GetComponent<Messenger>();
        }
        speakTime = (sound == null) ? 10 : sound.length;
        subtitles.AddMessage(
            line, speakTime, sound, speaker.textColour);
    }
    void NextAction()
    {
        if(nextLine != null)
        {
            nextLine.Action();
        } else
        {
            EventManager.Trigger("DialogueCutscene" + scene.idNumber);
        }
        Destroy(this);
    }
}
