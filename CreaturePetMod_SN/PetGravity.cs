using UnityEngine;

class PetGravity : MonoBehaviour
{
	public Rigidbody petRigidBody;
	public OnSurfaceTracker onSurfaceTracker;
	public Pet pet;

	public void FixedUpdate()
	{
		petRigidBody.useGravity = false;
		bool aboveZero = base.transform.position.y >= 0f;
		bool isOnSurface = pet.IsOnSurface();

		if (!isOnSurface)
		{
			float d = aboveZero ? 9.81f : 2.7f;
			this.petRigidBody.AddForce(-Vector3.up * Time.deltaTime * d, ForceMode.VelocityChange);
		}
		else
		{
			float d2 = 10f;
			Vector3 surfaceNormal = pet.GetSurfaceNormal();
			this.petRigidBody.AddForce(-surfaceNormal * d2);
		}
		float num = isOnSurface ? 1.6f : 0.03f;
		if (!aboveZero)
		{
			num += 0.3f;
		}
		this.petRigidBody.drag = num;

	}

}
