
using UnityEngine;
using UnityEngine.AI;

public class DisablePhysics : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < -1000)
        {
            Respawn();
        }
	}
    int collisions = 0;
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Ground")
        {
            DisableMyPhysics(GetComponent<Rigidbody>(),true);
               NavMeshObstacle nmo = GetComponent<NavMeshObstacle>();
            if(nmo == null)
            gameObject.AddComponent<NavMeshObstacle>();
        }
        else
        {
            collisions++;

            if(collisions > 500)
            {
                Respawn();
            }
        }
    }
    void Respawn()
    {
        //Roughly 300 objects are respawned in this level
        //Debug.Log("respawned");
        EnviroSpawn.Spawn(this.gameObject);
        Destroy(this.gameObject);

    }
    public void DisableAllPhysics()
    {
        Rigidbody[] b = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody r in b)
        {
            DisableMyPhysics(r);
        }
    }
    public void DisableMyPhysics(Rigidbody r, bool permanent = false)
    {
        if (permanent)
        {
            Destroy(r);
        } else
        {
            r.isKinematic = true;
        }
    }
}
