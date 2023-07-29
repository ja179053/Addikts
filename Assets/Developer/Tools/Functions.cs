using UnityEngine;

public static class Functions
{
    //Multiplies 2 vectors
    public static Color Invisible(Color c, Visibility v = Visibility.Opposite)
    {
        switch (v)
        {
            case Visibility.Opposite:
                c.a = (c.a == 0) ? 1 : 0;
                break;
            case Visibility.Show:
                c.a = 1;
                break;
            case Visibility.Hide:
                c.a = 0;
                break;
        }
        return c;
    }
    public static Vector3 MultiplyVector3(Vector3 a, Vector3 b)
    {
        a.x *= -b.x;
        a.y *= b.y;
        a.z *= b.z;
        return a;
    }
    public static Vector3 MultiplyForwardVector3(Vector3 v, Vector3 forward)
    {
        forward *= v.z;
        forward.y += v.y;
        return forward;
    }
    //Adds a random upwards force
    public static Vector3 RandomVector3YPlus(int maxRadius)
    {
        return new Vector3(RandomInt(maxRadius), Mathf.Abs(RandomInt(maxRadius)), RandomInt(maxRadius));
    }
    //Adds a random upwards force
    public static Vector3 RandomVector3(int maxRadius)
    {
        return new Vector3(RandomInt(maxRadius), (RandomInt(maxRadius)), RandomInt(maxRadius));
    }
    //Creates a random int
    public static int RandomInt(int limit)
    {
        return Random.Range(-limit, limit);
    }
    public static int Loop(int i, int max)
    {
        if (i >= max)
        {
            return 0;
        }
        return i;
    }
    public static bool InBounds(float number, float boundary)
    {
        return (number < boundary && number > -boundary);
    }
    public static bool InBounds(Vector2 vector, float boundary)
    {
        return InBounds(vector.x, boundary) && InBounds(vector.y, boundary);
    }
    public static bool InBounds(Vector3 vector, float boundary)
    {
        return InBounds(vector.x, boundary) && InBounds(vector.y, boundary) && InBounds(vector.z, boundary);
    }
}
