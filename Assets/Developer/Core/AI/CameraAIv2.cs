using UnityEngine;
using Players;
using ManagerClasses;
using System.Collections;
using TMPro;

public class CameraAIv2 : AI
{
    public LayerMask nonFocus, trees, ground;
    public FollowMouse targetPosHidden;
    public static Camera mainCamera;
    static int targetZoom, zoomSpeed = 2, zoomIn = 35, zoomOut = 50;
    public bool shared, canFollow;
    public AI player;
    public Transform moveTarget;
    static float speedFactor = 1;
    public static bool autoRotate = true, invertY = false;
    public static CameraSetup setup = CameraSetup.One;
    // public Vector3 offset;

    protected override void CharacterSettings()
    {
        hP = 1000000;
        //   player = MultiplayerManager.players[0];
        //  player = Player.players[0];
        if (MultiplayerManager.players != null && MultiplayerManager.players.Count == 1 && (moveTarget == null || moveTarget.position == Vector3.zero))
        {
            GameObject g = GameObject.Find("PlayerCameraTarget");
            if (g != null)
            {
                moveTarget = g.transform;
            }
            //   moveTarget = player.transform;
            //   moveTarget.position = player.transform.position;
        }
        else
        {
            if (moveTarget == null)
            {
                offset = new Vector3(0, 2, -4.75f);
            }
            else
            {
                moveTarget.localPosition = offset;
                offset = Vector3.zero;
            }
        }
        Camera c = GetComponent<Camera>();

        CameraAI3.mainCamera = mainCamera = currentCamera = c;
        CameraAI3[] cams = FindObjectsOfType<CameraAI3>();
        allCameras = new Camera[cams.Length + 1];
        allCameras[0] = mainCamera;
        for (int i = 0; i < cams.Length; i++)
        {
            allCameras[i + 1] = cams[i].GetComponent<Camera>();
            allCameras[i + 1].enabled = false;
        }
        maxMoveDistance = 0;
        targetZoom = zoomOut;
        //   LookAt();
        // moveAngle = new Vector3();
    }
    Transform lookAt, runTarget;
    float distance;
    public float height = 8;
    int ID;
    // Update is called once per frame
    public static Camera currentCamera;
    static Camera[] allCameras;
    public static void Activate(int index, int time = 0)
    {
        if (currentCamera != null)
        {
            if (allCameras != null)
            {
                if (index < allCameras.Length)
                {
                    currentCamera.GetComponent<AI>().StartCoroutine(DramaticShot(allCameras[index], time));
                }
            }
        }
    }
    static IEnumerator DramaticShot(Camera c, int time = 0)
    {
        Camera d = currentCamera;
        currentCamera.enabled = false;
        currentCamera = c;
        currentCamera.enabled = true;
        yield return new WaitForSeconds(time);
        if (time != 0)
        {
            currentCamera.enabled = false;
            currentCamera = d;
            currentCamera.enabled = true;
        }

    }
    void LookAt()
    {
        if (Player.targeter)
        {
            Vector3 v;
            if (Player.targeter.Target == null)
            {
                //Set look at ti v's position
             //   Debug.Log("looking at " + player.nameString);
                /*  if (targetPosHidden != null)
                  {
                      lookAt = targetPosHidden.transform;
                  } else
                  {
                      lookAt = player.transform;
                  }*/
                lookAt = player.transform;
                v = lookAt.position;
                //   v = moveTarget.position + moveTarget.forward;
            }
            else
            {
                lookAt = Player.targeter.Target.transform;
                v = ((lookAt.position - player.transform.position) / 1.75f) + player.transform.position;
            }
            //      Debug.Log("looking at " + lookAt);
            v = Vector3.Slerp(transform.position, v + lookAt.transform.forward, Time.deltaTime * 0.1f);
            //    transform.LookAt(v);
            lookAtRotation = Quaternion.LookRotation(v - transform.position);
        }
    }
    Quaternion lookAtRotation;
    float lastTimeScale = -1;
    protected override void Update()
    {
    }
    bool fpsMode;
    new void FixedUpdate()
    {
        if (ID > 0)
        {
            if (canFollow)
            {
                if (player != null && moveTarget != null)
                {
                    //Where the player is going
                    Vector3 newTarget = moveTarget == null ? new Vector3(player.dir.x + player.dir.z, player.transform.position.y, player.dir.y + player.dir.w) : moveTarget.transform.position;
                    if (autoRotate)
                    {
                        fpsMode = (Manager.GameState == ManagerStates.Battle) || (EventManager.HasQuest("Move") != null);
                        //Only moves when the player moves.
                        if (player.dir.x != 0 && player.dir.y != 0)
                        {
                            //Follows in relation to the offset
                            Vector3 newOffset = fpsMode ? offset + (moveTarget.forward * 2) : Functions.MultiplyForwardVector3(offset, moveTarget.transform.forward);
                            newTarget = moveTarget.transform.position + newOffset;
                        }

                        if (!fpsMode)
                        {
                            newTarget.y = CheckGround() * 0.8f;
                        }
                        LookAt();
                    }
                    else
                    {
                        //Follows a set position in relation to the target
                        newTarget += offset;
                        //    newTarget = new Vector3(newTarget.x, CheckGround() * 1.5f, newTarget.z);
                        targetPosHidden.Rotate();
                        LookAt();
                    }
                    //Y positions above ground
                    //    Debug.Log(moveTarget.y);
                    //    distance = Vector3.Distance(transform.position, moveTarget.transform.position + offset);
                    // Debug.Log(distance);
                    //     newTarget = (distance < maxMoveDistance) ? transform.position : moveTarget.transform.position + offset;newTarget = (distance < maxMoveDistance) ? transform.position : moveTarget.transform.position + offset;
                    transform.position = Vector3.Lerp(transform.position, newTarget, ((float)speed / speedFactor) * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, rotateSpeed * Time.deltaTime);
                    Zoom(targetZoom);
                    //   CheckObstructed(nonFocus);
                }
            }
        }
    }
    public IEnumerator SetID(int i)
    {
        yield return new WaitUntil(() => MultiplayerManager.players != null && i <= MultiplayerManager.players.Count);
        ID = i;
        if (player == null)
        {
        //    Debug.Log(string.Format("player {0}/{1} is null.", ID, MultiplayerManager.players.Count));
            player = MultiplayerManager.players[(int)ID - 1];
        }
        if (player != null)
        {
            moveTarget = player.transform;
            if (setup == CameraSetup.SplitScreen)
            {
                //Needs option to make landscape/portrait
                GetComponent<Camera>().rect = new Rect(0.5f * (ID - 1), 0, 0.5f, 1f);
            }
            player.gameObject.SetActive(true);
            Hover hover = GameObject.Find("MinimapCamera").GetComponent<Hover>();
            hover.target = transform;
            hover.baseOffset = hover.offset;
            runTarget = GameObject.Find("RunTarget").transform;
            GameObject ga = GameObject.Find("CamStartPos");
            if (ga != null)
            {
                canFollow = false;
                transform.position = ga.transform.position;
                //Bouncer code to organise the crowd
                foreach (TextMeshPro c in GetComponentsInChildren<TextMeshPro>())
                {
                    int j = 0;
                    if (int.TryParse(c.name[0].ToString(), out j)){
                        if (j < (int)QuestEnum.Base)
                        {
                            c.gameObject.SetActive(false);
                        }
                    } else
                    {
                        Debug.LogError("You fool! The quest needs a serious name that starts with a number!");
                    }
                }
            }
            else
            {
                canFollow = true;
                transform.position = moveTarget.position + (Vector3.up * 3);
            }
        }
    }
    protected bool obstructed, obstructedTrees, groundSeen;
    float inverse = 1f;
    public float mouseBorders = 0, xLimit = 0, yLimit = 0, rotateSpeed = 1; 
    float CheckGround()
    {
        RaycastHit rhit;
        Vector3 newPosition = transform.position;
        newPosition.y = moveTarget.position.y + 2;
        groundSeen = Physics.Raycast(newPosition, -transform.up, out rhit, height, ground);
        if (groundSeen)
        {
            float result = height + rhit.point.y;
            Debug.Log(result);
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
    public void Run()
    {
        CameraAIv2[] cameras = FindObjectsOfType<CameraAIv2>();
        foreach (CameraAIv2 camera in cameras)
        {
            runTarget.gameObject.SetActive(true);
            camera.moveTarget = runTarget;
        }
        runTarget.GetComponent<NavMeshMove>().SetSpeed(0.01f);
        //   moveTarget = GameObject.Find("RunTarget").transform;
        //   Debug.Log(mainCamera + " running");
    }
}
public enum CameraSetup
{
    Shared,
    One,
    SplitScreen
}
