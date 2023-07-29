using UnityEngine;

public class EnviroSpawn : MonoBehaviour
{
    TreeSpawner[] treeSpawners;
    static Vector3 myPos;
    public GameObject cloud, grassPrefab;
    public GameObject[] spawnables;
    public int grassChancePercent;
    public int maxRadius = 100, noToSpawn = 100, avoidanceFactor = 1, cloudDensity = 10, cloudSpacing = 2, levelWidth = 100, levelHeight = 100, spacing = 1, borderWidth = 1, treeDensity = 2;
    static int MaxRadius, AvoidanceFactor;
    // The objects are hitting each other too hard
    //We gotta make them dodge the other objects its the only way
    void Start()
    {
        treeSpawners = FindObjectsOfType<TreeSpawner>();
        //  SpawnClouds();
           PhysicsObjects();
        foreach(TreeSpawner ts in treeSpawners)
        {
            ts.Borders();
        }
        myPos = transform.position;
        MaxRadius = maxRadius;
        AvoidanceFactor = avoidanceFactor;
        SpawnGrass();
    }
    void SpawnGrass()
    {
        Terrain terrain = FindObjectOfType<Terrain>();
        for (int i = 0; i < terrain.terrainData.size.x; i++)
        {

            for (int j = 0; j < terrain.terrainData.size.x; j++)
            {
                if(Random.Range(0,100) < grassChancePercent)
                {
                    //Grass isnt spawning in exactly the right place
                    GameObject.Instantiate(grassPrefab, new Vector3(i,terrain.terrainData.GetHeight(i,j),j), Quaternion.identity);
                }
            }
        }
    }
    public static void Spawn(GameObject g)
    {

        if (g != null)
        {
            Rigidbody r = g.GetComponent<Rigidbody>();
            if (r == null)
            {
                r = g.AddComponent<Rigidbody>();
            }
            Vector3 v = Functions.RandomVector3YPlus(MaxRadius);
            Instantiate(g, myPos + (-v * AvoidanceFactor), Quaternion.identity);
            r.AddForce(v);
        }
    }
    void PhysicsObjects()
    {
        foreach (GameObject g in spawnables)
        {
            for (int i = 0; i < noToSpawn; i++)
            {
                Spawn(g);
            }
        }
    }
    
    void SpawnClouds()
    {
        int cloudRadius = maxRadius * (100 / cloudDensity);
        for (int i = 0; i < cloudRadius; i++)
        {
            for (int j = 0; j < cloudRadius; j++)
            {
                //The clouds have to appear relative to the spawner's position
                //i.e. original position - x = finalPosition
                Instantiate(cloud, new Vector3((i * cloudSpacing), transform.position.y, (j * cloudSpacing)), Quaternion.identity);
                i += Mathf.Abs(Functions.RandomInt(maxRadius / cloudDensity));
                j += Mathf.Abs(Functions.RandomInt(maxRadius / cloudDensity));
            }

        }
    }
}
