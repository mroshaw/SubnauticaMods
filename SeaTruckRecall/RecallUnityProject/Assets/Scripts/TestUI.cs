using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    [SerializeField] TMP_InputField forceText;

    [SerializeField] private WaypointNavigation navSystem;

    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform waypoint2;
    [SerializeField] private Transform waypoint3;
    [SerializeField] private Transform waypoint4;
    [SerializeField] private Transform waypoint5;

    public void StartNav()
    {
        List<Waypoint> waypoints = new List<Waypoint>();

        waypoints.Add(new Waypoint(waypoint1.position, waypoint1.rotation, true, "Waypoint 1"));
        waypoints.Add(new Waypoint(waypoint2.position, waypoint2.rotation, true, "Waypoint 2"));
        waypoints.Add(new Waypoint(waypoint3.position, waypoint3.rotation, true, "Waypoint 3"));
        waypoints.Add(new Waypoint(waypoint4.position, waypoint4.rotation, true, "Waypoint 4"));
        waypoints.Add(new Waypoint(waypoint5.position, waypoint5.rotation, true, "Waypoint 5"));

        navSystem.SetWayPoints(waypoints);
        navSystem.StartWaypointNavigation();
    }

    public void ApplyForce()
    {
        float forceToApply = float.Parse(forceText.text);
        Rigidbody rigidBody = navSystem.GetComponent<Rigidbody>();

        Vector3 forwardForce = rigidBody.transform.forward * forceToApply;

        rigidBody.AddForce(forwardForce);
    }

    public void ConfigureRigidBodies()
    {
        RigidbodyNavMovement rbNav = navSystem as RigidbodyNavMovement;
        rbNav.ConfigureRigidBodies();
    }

    public void RestoreRigidBodies()
    {
        RigidbodyNavMovement rbNav = navSystem as RigidbodyNavMovement;
        rbNav.RestoreRigidBodies();
    }
}
