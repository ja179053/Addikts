using UnityEngine;
using ManagerClasses;

public class PlayerTrigger : MonoBehaviour
{
    Ally2 ally;
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Hero")
        {
            ally = c.GetComponent<Ally2>();
        }
    }
    void Update()
    {
        if (ally != null)
        {
            if (Manager.GameState == ManagerStates.Exploration)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    ally.Interact();

                }
            }
        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Hero")
        {
            ally = null;
        }
    }
}
