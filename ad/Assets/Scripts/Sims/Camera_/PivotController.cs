using UnityEngine;
//using System.Collections;

public class PivotController : MonoBehaviour {
	public Transform roamingPivot;

	public float maxAngleX;
	public float maxAngleZ;
	public float sinLoopSpeed;

	private SimWorldBase.CAMERATYPE camType = SimWorldBase.CAMERATYPE.normal;
	private Vector3 nomalRotationDegrees = new Vector3();
	private Vector3 roamingRotationDegrees = new Vector3();

	private float startTime = 0f;
	private float sinLoop = 0f;
	private float sinTarget = 0f;

	//// Use this for initialization
	//void Start () {
	//}

	public void InitPivotRotation(float rotX, float rotY) {
		roamingRotationDegrees.x = rotX;
		roamingRotationDegrees.y = rotY;
		roamingRotationDegrees.z = 0f;

		nomalRotationDegrees.x = 0f;
		nomalRotationDegrees.y = rotY;
		nomalRotationDegrees.z = 0f;
	}

	private Vector3 rotV3 = Vector3.zero;
	public void Rotate () {
		transform.Rotate (Vector3.up, RotationDegrees.y * Time.deltaTime, Space.Self);

		if (camType == SimWorldBase.CAMERATYPE.normal) {
			if (sinLoop != sinTarget) {
				sinLoop = Mathf.Lerp (sinLoop, sinTarget, startTime);
				startTime += sinLoopSpeed * Time.deltaTime * 0.5f;
			}
		} else if (camType == SimWorldBase.CAMERATYPE.roaming) {
			sinLoop = (sinLoop + (sinLoopSpeed * Time.deltaTime)) % (2f * Mathf.PI);
		}

		float angleX = Mathf.Sin(sinLoop);
		rotV3.x = maxAngleX * (angleX < 0 ? angleX * 0.5f : angleX);

		float angleZ = Mathf.Sin(sinLoop);
		rotV3.z = maxAngleZ * angleZ;

		roamingPivot.localEulerAngles = rotV3;
	}

	public Vector3 RotationDegrees {
		get {
			Vector3 rDegrees = nomalRotationDegrees;

			if (camType == SimWorldBase.CAMERATYPE.roaming) {
				rDegrees = roamingRotationDegrees;
			}

			return rDegrees;
		}
	}

	public void OnSetRotationType (SimWorldBase.CAMERATYPE type) {
		if (camType != type) {
			camType = type;

			sinTarget = 0f;
			float loopSpace = sinLoop / (2f * Mathf.PI);
			if (loopSpace <= 0.25f) {
				sinTarget = 0f;
			} else if (loopSpace >= 0.5f && loopSpace <= 0.75f) {
				sinTarget = Mathf.PI;
			} else if (loopSpace >= 0.25f && loopSpace <= 0.5f) {
				sinTarget = Mathf.PI;
			} else {
				sinTarget = Mathf.PI * 2f;
			}
			startTime = 0f;
		}
	}

	public void ResetStatus () {
		sinLoop = 0f;
	}

	//// Update is called once per frame
	//void Update () {
	//}
}
