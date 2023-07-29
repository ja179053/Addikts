namespace Instruction
{
    using UnityEngine;

    public class SpeechAfterTime : EventAfterTime
    {
        public Color color;
        public AudioClip speech;
        public string speechText;
        protected override void EventCompleted()
        {
            base.EventCompleted();
            GameObject.Find("Subtitles").GetComponent<Messenger>().AddMessage(speechText, 10, speech, color);
         //   Character.subtitles.AddMessage(speechText, 10, speech);
        }
    }
}