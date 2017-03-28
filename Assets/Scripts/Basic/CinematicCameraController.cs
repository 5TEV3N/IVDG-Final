using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraController : MonoBehaviour
{
    //@Chronoblast
    //www.chronoblastgames.com

    [Header("Camera Waypoints")]
    public List<GameObject> cameraWaypoints; //List of Waypoints for the Camera to Follow;

    [Header("Camera Attributes")]
    public float cameraSpeed; //Speed at which the camera moves at;
    public float cameraRotationSpeed; //Sped at which the camera rotates at;

    [Header("Debug")]
    private bool canMove; //Can the Camera start moving?

    private int currentWaypoint = 0; //Camera's current waypoint;

    private float t; //Variable used to calculate distance between current position and desired position for Vector3.Lerp;

    private Vector3 waypointVector; //Where the camera is moving to;

    private Quaternion waypointQuaternion; //What rotation the camera is rotating to;

    private void Start() //Initialize Variables and Set up Route Logic;
    {
        currentWaypoint = 0;

        StartRoute();
    }

    private void Update() //Try and Lerp to the next position every frame;
    {
        if (canMove)
        {
            GoToWaypoint();
        }
    }

    void StartRoute() //Initial Logic;
    {
        waypointVector = cameraWaypoints[currentWaypoint].transform.position;
        waypointQuaternion = cameraWaypoints[currentWaypoint].transform.rotation;

        canMove = true;
    }

    void FindNextWaypoint() //Finds the Next waypoint in the waypoint list and resets the variable used for the lerp calculations;
    {
        if (currentWaypoint == cameraWaypoints.Count - 1)
        {
            FinishedRoute();
            return;
        }

        currentWaypoint++;
        t = 0;

        waypointVector = cameraWaypoints[currentWaypoint].transform.position;
        waypointQuaternion = cameraWaypoints[currentWaypoint].transform.rotation;

        canMove = true;
    }

    void GoToWaypoint() //The actual camera movement/rotation;
    {
        transform.position = Vector3.Lerp(transform.position, waypointVector, t);
        transform.rotation = Quaternion.Lerp(transform.rotation, waypointQuaternion, t);

        t += cameraSpeed * Time.deltaTime;

        if (transform.position == waypointVector)
        {
            canMove = false;
            FindNextWaypoint();
        }
    }

    void FinishedRoute() //Once the camera reaches the final point;
    {
        Debug.Log("Finished Route!");
    }
}
