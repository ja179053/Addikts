using UnityEngine;

public class Hover : MonoBehaviour
{
    public Vector3 offset;
    public float hoverSpeed = 1;
    public float maxSpeed = 1, acceleration = 1, distance = 1;
    public Transform target;
    public Vector3 baseOffset;
    CameraAI3 character;
    public bool lookAt, followPlayer;
    // Use this for initialization
    void Start()
    {
        character = GetComponent<CameraAI3>();
        if (character != null)
        {
            baseOffset = character.offset;
        }
        else if (target != null)
        {
            baseOffset = offset;
        }
        if (followPlayer)
        {
         //   StartCoroutine(FindPlayer());
        }
    }
  /*  IEnumerator FindPlayer()
    {
    //    yield return new WaitUntil(() =>FindObjectOfType<Player>() != null);
        yield return new WaitForSeconds(10);
        target = FindObjectOfType<Player>().transform;
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        //I like this-
        ///This is now scientifically correct
        ///frequency = 1/wavelength
        ///Distance works fine
        ///To make it move FASTER we have to multiply the time
        float f = Mathf.Sin(Time.time * hoverSpeed);
        Vector3 hoverPos = offset * f;
        if (character != null)
        {
            hoverPos.y = character.CheckGround();
        }
        if (target != null)
        {
            //   Debug.Log("following target " + name);
            //When a transform is attached
            Vector3 targetPos = target.position + baseOffset;
              hoverSpeed = Vector3.Distance(transform.position, targetPos);
              if (hoverSpeed > distance)
              {
                  hoverSpeed *= acceleration;
                  hoverSpeed = Mathf.Clamp(hoverSpeed, 0, maxSpeed);
                transform.position = Vector3.Lerp
                    (transform.position, targetPos, hoverSpeed * Time.fixedDeltaTime);
            }
            if (lookAt)
            {
                transform.LookAt(target.parent);
            }
        }
        else
        {
            //When nothing is attached
            //   Debug.Log("hovering " + target + name);
            hoverPos = transform.position + hoverPos;
            if (Vector3.Distance(hoverPos, transform.position) > distance)
            {
                transform.position = Vector4.Lerp(transform.position, hoverPos, hoverSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
