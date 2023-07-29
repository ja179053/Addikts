namespace ManagerClasses.Music {
    using UnityEngine;

    public class MusicManager : MonoBehaviour
    {
        public AudioSource music, sfx;
        //Sound tracks
        public AudioClip battle, gameOver, chill;
        //Battle/enemy death sound effects
        public AudioClip victory, battleStart;
        //Character sound effects
        public AudioClip item, hit, drop, levelUp, heal;
        // Use this for initialization
        void Start() {
            music = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        public void Play(ManagerStates state) {
            switch (state)
            {
                //UPGRADE ONCE YOU'VE FINISHED VOLACNO MUSIC
                case ManagerStates.Battle:
                    Playing(battle, true);
                    break;
                case ManagerStates.Exploration:
                    Playing(chill, true);
                    break;
                case ManagerStates.GameOver:
                    Playing(gameOver, false);
                    break;
            }
        }
        void Playing(AudioClip ac, bool loop)
        {
            if (ac != null)
            {
                music.clip = ac;
                music.loop = loop;
                if (!music.isPlaying)
                {
                    music.Play();
                }
            }
        }
        public void PlaySound(ManagerStates newState)
        {
            if (Manager.GameState != newState)
            {
                switch (newState)
                {
                    case ManagerStates.Battle:
                        sfx.PlayOneShot(battleStart);
                        break;
                    case ManagerStates.Exploration:
                        sfx.PlayOneShot(victory);
                        break;
                }
            }
        }
        public void PlaySound(string s)
        {
            AudioClip ac = null;
            switch (s)
            {
            }
            if (ac != null)
            {
                sfx.PlayOneShot(ac);
            }
        }
}
}