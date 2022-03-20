using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PetAvoidEdges : CreatureAction
{
	public OnSurfaceTracker onSurfaceTracker;
	public WalkBehaviour walkBehaviour;
	public Rigidbody rigidbody;
	public Vector3 scanOffset = new Vector3(0f, 2f, 0f);
	public Vector3 scanDirection = new Vector3(0f, -2f, 5f);
	public float scanDistance = 15f;
	public float scanInterval = 1f;
	public float walkInterval = 1f;
	public float avoidanceDuration = 2f;
	public float avoidanceDistance = 5f;
	public float moveVelocity = 10f;
	private Vector3 avoidancePosition;
	private float timeStartAvoidance;
	private float timeNextScan;
	private float timeNextWalk;

	public override float Evaluate(Creature creature)
	{
		if (Time.time < this.timeStartAvoidance + this.avoidanceDuration)
		{
			return base.GetEvaluatePriority();
		}
		if (!this.onSurfaceTracker.onSurface)
		{
			return 0f;
		}
		if (Time.time < this.timeNextScan)
		{
			return 0f;
		}
		this.timeNextScan = Time.time + this.scanInterval;
		Vector3 origin = base.transform.TransformPoint(this.scanOffset);
		Vector3 surfaceNormal = this.onSurfaceTracker.surfaceNormal;
		Vector3 normalized = Vector3.ProjectOnPlane(this.rigidbody.velocity, surfaceNormal).normalized;
		Vector3 direction = this.scanDirection.y * surfaceNormal + this.scanDirection.z * normalized;
		if (Physics.Raycast(origin, direction, this.scanDistance))
		{
			return 0f;
		}
		this.avoidancePosition = base.transform.position - normalized * this.avoidanceDistance;
		return base.GetEvaluatePriority();
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x000313F7 File Offset: 0x0002F5F7
	public override void StartPerform(Creature creature)
	{
		this.timeStartAvoidance = Time.time;
		this.walkBehaviour.WalkTo(this.avoidancePosition, this.moveVelocity);
	}



}
