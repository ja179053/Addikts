using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerClasses;
using Players;
//FIX ME
public class CameraAI : CameraAIv2 {
    static Player player;
    static Camera mainCamera;
    static int targetZoom, zoomSpeed = 2, zoomIn = 35, zoomOut = 50;
    public LayerMask FocusTargets, environment;
	
	// Update is called once per frame
	protected override void Update ()
    {
        switch (Manager.GameState)
        {
            case ManagerStates.Battle:
                CheckToMove(FocusTargets);
                break;
            case ManagerStates.Shopping:
                break;
            case ManagerStates.Exploration:
                //A hacky way to fix the camera going too far away
                if(moveAngle != Vector3.zero)
                {
                    moveAngle = Vector3.zero;
                }
             //   CheckToMove(environment);
                break;

        }
        Vector3 forward = player.transform.forward;
    //    forward.x = moveAngle;
        forward = Functions.MultiplyVector3(forward, offset);
     //   forward.x += moveAngle.x;
        forward += player.transform.position + offset;
        //forward = new Vector3((int)forward.x, (int)forward.y,(int) forward.z);
        
        float s = speed;
        //(int)Mathf.Round(speed - Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, forward) > 1)
        {
            transform.position = Vector3.Lerp(transform.position, forward, s * Time.deltaTime);
        }
        transform.LookAt(forward);
        Zoom(targetZoom);
	}
    Vector3 moveAngle;
    int swerve = -1;
    void CheckToMove(LayerMask layer)
    {
        bool wasObstructed = obstructed;
        RaycastHit rhit;
        if (Physics.Raycast(transform.position, -transform.up, out rhit, 5))
        {
         //   moveAngle.y = 5 - rhit.distance;
        }
        obstructed = Physics.Raycast(transform.position, transform.forward, out rhit, targetZoom, layer);
        bool behind = Physics.Raycast(transform.position, -transform.forward, 1, environment);
        if (behind)
        {
         //   moveAngle.z++;
        }else
        if (obstructed)
        {
         //   moveAngle += (transform.right * swerve * speed * 2f);
            Debug.Log(string.Format("{0} {1}", rhit.collider.name, rhit.distance));
        } else if (swerve > moveAngle.x && wasObstructed)
        {
            //Swerves more when something is in the way
            //When the thing is finally out the way, it moves the opposite way
         //  swerve *= -1;
        }
    }
}
