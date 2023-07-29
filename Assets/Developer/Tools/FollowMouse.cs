using UnityEngine;
using Players;

//Rotates an object around it
public class FollowMouse : MonoBehaviour {
    public float yMin = 0, yMax = 360;
    public Vector2 speed, boundary;
    public void Rotate()
    {
        Vector3 rotation = new Vector3(transform.rotation.x,
            transform.rotation.y, transform.rotation.z);
        float y = Input.mousePosition.y;
        float x = Input.mousePosition.x;
        int invert = CameraAIv2.invertY ? -1 : 1;
        if (!Functions.InBounds(y + Screen.height / 2, boundary.y)){
            y *= speed.y * PlayerMove.sensitivity;
            y = Mathf.Clamp(y, yMin, yMax);
            if (invert == -1)
            {
                y = yMax - y;
            }
         //   Debug.Log(y);
        } else
        {
            y = rotation.x;
        }
        if(!Functions.InBounds(x + Screen.width / 2, boundary.x))
        {
            x *= speed.x * PlayerMove.sensitivity * invert;
        } else {
            x = rotation.y;
        }
        transform.rotation = Quaternion.Euler(new Vector3
            (y, x, 0));
     //   Debug.Log(transform.ToString() + x + " " + y);
    }
}
