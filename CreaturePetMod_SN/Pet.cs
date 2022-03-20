using UnityEngine;

class Pet : Creature
{
	public OnSurfaceTracker onSurfaceTracker;

	public bool IsOnSurface()
	{
		return this.onSurfaceTracker.onSurface;
	}


	public Vector3 GetSurfaceNormal()
	{
		return this.onSurfaceTracker.surfaceNormal;
	}
}
