using UnityEngine;
using UnityEditor;
using System.Reflection;

public class CheatCodes : MonoBehaviour {
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                LevelManager.Menu();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                /*
                //Automatically clears the log
                var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
                var type = assembly.GetType("UnityEditor.LogEntries");
                var method = type.GetMethod("Clear");
                method.Invoke(new object(), null);
                */
                if (LevelManager.LevelNumber == 0)
                {
                    MultiplayerManager manager = FindObjectOfType<MultiplayerManager>();
                    StartCoroutine(manager.SetReady(1));
                }
                else { 
                    LevelManager.NextLevel();
                }
            }
        }
	}
}
