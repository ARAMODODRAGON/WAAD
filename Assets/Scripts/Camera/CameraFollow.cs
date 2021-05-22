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
					break;
				}
		}
    }

	void MoveCamera()
	{
		Vector3 target = new Vector3();

		Vector3 smoothTarget = Vector3.SmoothDamp(transform.position, target, ref cameraVelocity, cameraTime);

		transform.position = smoothTarget;
	}
}
