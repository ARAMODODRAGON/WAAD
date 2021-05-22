using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStates
{
	Locked,
	Moving
}

public class CameraFollow : MonoBehaviour
{
	public CameraStates state;

	public PlayerController player;

	[SerializeField] float offset;

	// how close can the camera get to a set location before it stops
	[SerializeField] float tolerance;

	// used and modified by the smooth dampening function, just set it to 0 and let the function take care of it
	Vector3 cameraVelocity = Vector3.zero;

	// how long does it take for the camera to reach target
	[SerializeField] float cameraTime;

	public List<Transform> targets;

	int currentTarget = 0;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		switch (state)
		{
			case CameraStates.Locked:
				{
					break;
				}
			case CameraStates.Moving:
				{
					MoveCamera();
					break;
				}
		}
    }

	void MoveCamera()
	{
		Vector3 target = targets[currentTarget].position;

		Vector3 smoothTarget = Vector3.SmoothDamp(transform.position, target, ref cameraVelocity, cameraTime);

		transform.position = smoothTarget;

		// stop the camera once it reaches it's target
		if (Vector3.Distance(transform.position, target) <= tolerance)
		{
			state = CameraStates.Locked;
			currentTarget++;
		}
	}
}
