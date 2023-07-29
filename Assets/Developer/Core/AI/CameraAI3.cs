 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using AIAdvanced;
using Players;

public class CameraAI3 : AIAdvance
{
    public LayerMask nonFocus, trees, ground;
    public FollowMouse targetPosHidden;
    public static Camera mainCamera;
    static int targetZoom, zoomSpeed = 2, zoomIn = 35, zoomOut = 50;
    AI player;
    Hover hover;
    public Transform moveTarget;
    public static bool autoRotate = true, invertY = false;

    protected override void CharacterSettings()
    {
        hP = 1000000;
        hover = GetComponent<Hover>();
        if(hover != null)
        {
            offset = hover.offset;
        }
    //    player = MultiplayerManager.players[0];
      //  player = Player.players[0];
        // moveAngle = new Vector3();
    }
    bool setup = false;
    Transform lookAt;
    float distance;
    public float height = 8;
    public void Setup()
    {
        if (moveTarget == null || moveTarget.position == Vector3.zero)
        {
            moveTarget = player.transform;
            moveTarget.position = player.transform.position;
        }
        else
        {
            moveTarget.localPosition = offset;
            offset = Vector3.zero;
        }
        Camera c = GetComponent<Camera>();
        if (mainCamera == null && name == "Director")
        {
            mainCamera = c;
        }
        /*
        else if (mainCamera != c)
        {
            Debug.Log("TWO MAIN CAMERAS!");
            Destroy(this.gameObject);
        }*/
        maxMoveDistance = 0;
        targetZoom = zoomOut;
        setup = true;
    }
    public static bool moveEventPresent;
    // Update is called once per frame
    void LookAt()
    {
        if (Player.targeter)
        {
            Vector3 v;
            if (Player.targeter.Target == null)
            {
                lookAt = player.transform;
                v = lookAt.position;
             //   Debug.Log(name + " lookking at player");
            }
            else
            {
                lookAt = Player.targeter.Target.transform;
                v = ((lookAt.position - player.transform.position) / 2) + player.transform.position;
            }
            v = Vector3.Lerp(transform.position, v + lookAt.transform.forward, Time.deltaTime * 0.01f);
            transform.LookAt(v);
        }
    }
    protected override void Update()
    {
        //OPTIMISE YOUR CODE
    }
    new void FixedUpdate()
    {
        if (player == null)
        {
         //   player = MultiplayerManager.players[0];
            //  player = Player.players[0];
        }
        else
        {
            if (!setup)
            {
                Setup();
            }
            Vector3 newTarget = transform.position;
            //Where the player is going
            //  newTarget = moveTarget == null ? new Vector3(player.dir.x + player.dir.z, player.transform.position.y, player.dir.y + player.dir.w) : moveTarget.transform.position;
            if (autoRotate)
            {
                if ((player.dir.x != 0 && player.dir.y != 0) || moveEventPresent)
                {
                    //  float f = Vector3.Distance(transform.position, player.transform.position);
                    //  Debug.Log(f);
                    //    Debug.Log(player.transform.forward);
                    //Follows in relation to the offset
                    //    newTarget = -Functions.MultiplyVector3(player.transform.forward, offset) + player.transform.position;
                    //  Debug.Log(string.Format("{0} {1} adjusting to {2}"
                    //      , player.dir.x, player.dir.y, moveTarget));
                    newTarget = new Vector3(newTarget.x, CheckGround() * 0.75f, newTarget.z);
                }
            }
            else
            {
                //    newTarget = new Vector3(newTarget.x, CheckGround() * 1.5f, newTarget.z);
                targetPosHidden.Rotate();
            }
            LookAt();
            //Y positions above ground
            //    Debug.Log(moveTarget.y);
            //    distance = Vector3.Distance(transform.position, moveTarget.transform.position + offset);
            // Debug.Log(distance);
            Zoom(targetZoom);
            //   CheckObstructed(nonFocus);
        }
    }
    protected bool obstructed, obstructedTrees, groundSeen;
    float inverse = 1f;
    public float mouseBorders = 0, xLimit = 0, yLimit = 0, rotateSpeed = 1;
    public float CheckGround()
    {
        RaycastHit rhit;
        groundSeen = Physics.Raycast(transform.position, -transform.up, out rhit, height, ground);
        if (groundSeen)
        {
            float result = height - (transform.position.y - rhit.point.y);
            return result;
        }
        else
        {
            return height;
        }
    }
    void CheckObstructed(LayerMask layer)
    {
        RaycastHit rhit;
        bool wasObstructed = obstructed;
        obstructed = Physics.Raycast(transform.position, transform.forward, out rhit, Vector3.Distance(transform.position, lookAt.position), layer);
        obstructedTrees = Physics.Raycast(transform.position, -transform.forward, out rhit, 1, trees);
        if (obstructed || wasObstructed)
        {
            //Move out the way
            /*      if (distance > (newTarget.z * inverse))
                  {
                      inverse *= -1;
                  } else
                  {
                      offset.x += inverse;
                   //   inverse *= -1;
                  }*/
            if (rhit.collider)
            {
                Debug.Log(string.Format("{0} in the way.", rhit.collider.name));
            }
        }
        if (obstructedTrees)
        {
            offset.z += 0.2f;
        }
    }

    public static void ZoomTo(bool zoom)
    {
        targetZoom = (zoom) ? zoomIn : zoomOut;
    }
    protected void Zoom(int targetZoom)
    {
        if (targetZoom != mainCamera.fieldOfView)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetZoom, zoomSpeed * Time.deltaTime);
        }
    }
    public void ToggleAutoRotate()
    {
        autoRotate = !autoRotate;
    }
    public void ToggleInvertY()
    {
        invertY = !invertY;
    }
}
