using UnityEngine;
using Players;

public class Targeter : MonoBehaviour {
    static Targeter singleton;
    public Vector3 offset;
    AI target, player;
    void Awake()
    {
        if(singleton != null)
        {
            Destroy(singleton.gameObject);
        }
        singleton = this;
        Player.targeter = this;
    }
    public AI Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
            if (value == null)
            {
             //   target.myText.Color(false);
                singleton.GetComponentInChildren<Renderer>().enabled = false;
            }
            else
            {
                GetComponentInChildren<Renderer>().enabled = true;
                transform.position = target.transform.position + offset;
                if(player == null)
                {
                    player = FindObjectOfType<Player>();
                }
                target.myText.Color(true);
                player.Aim(target);
            }
         //   Debug.Log(value);
        }
    }
    public void MoveTo(Vector3 v)
    {
            singleton.transform.position = v + offset;
    }
    public static void Teleport(Vector3 v)
    {
        singleton.transform.position = v;
    }
}
