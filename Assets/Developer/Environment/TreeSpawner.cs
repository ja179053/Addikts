using UnityEngine;
//FIX ME
public class TreeSpawner : MonoBehaviour
{
    public GameObject tree;
    public int spacing = 1, borderWidth = 1, treeDensity = 2;
    public bool forest;
    public void Borders()
    {
        Terrain terrain = FindObjectOfType<Terrain>();
        int posXMin = (int)terrain.transform.position.x;
        int posXMax = posXMin + (int)terrain.terrainData.size.x;
        int posZMin = (int)terrain.transform.position.z;
        int posZMax = posXMin + (int)terrain.terrainData.size.z;
        //Creates as long as x border position is verified
        //Creates as long as z border position is verified
        ///Only creates when x or z are within the border
        ///Spacing is how far apart the trees are from each other
        ///(Breaks 2 borders when not set to 1)
        for (int i = posXMin; i <= posXMax; i++)
        {
            for (int j = posZMin; j <= posZMax; j++)
            {
                bool x = (i <= posXMin + borderWidth || i >= posXMax - borderWidth);
                bool z = (j <= posZMin + borderWidth || j >= posZMax - borderWidth);
                if (i % spacing < 2 && j % spacing < 2)
                {
                    if (forest || x || z)
                    {
                        //  Debug.Log(i % spacing + " " + j%spacing);
                        Vector3 newRotation = new Vector3(Functions.RandomInt(15), Functions.RandomInt(360), Functions.RandomInt(15));
                        Instantiate(tree, new Vector3(i, terrain.transform.position.y, j), Quaternion.Euler(newRotation));
                    }
                }

            }

        }
    }
}
