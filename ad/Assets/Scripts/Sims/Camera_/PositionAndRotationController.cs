using UnityEngine;
using System.Collections;

public class PositionAndRotationController : MonoBehaviour {

	private Vector3 targetLocalPosition = new Vector3();
	private Vector3 startLocalPosition = new Vector3 ();
	private Vector3 defaultLocalPosition;

	private Quaternion targetLocalRotation = new Quaternion();
	private Quaternion startLocalRotation = new Quaternion();
	private Quaternion defaultLocalRotation;

	private float lerpTime = 0f;
	private float updateStep = 0.01f;

	// Use this for initialization
	void Start () {
		defaultLocalPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		defaultLocalRotation = new Quaternion (transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
	}

	public void AnimateToOringialStatus () {
		startLocalPosition.x = transform.localPosition.x;
		startLocalPosition.y = transform.localPosition.y;
		startLocalPosition.z = transform.localPosition.z;
		targetLocalPosition = defaultLocalPosition;

		startLocalRotation.x = transform.localRotation.x;
		startLocalRotation.y = transform.localRotation.y;
		startLocalRotation.z = transform.localRotation.z;
		startLocalRotation.w = transform.localRotation.w;

		targetLocalRotation = defaultLocalRotation;

		lerpTime = 0f;
	}

	public void RestStatus () {
		transform.localPosition = defaultLocalPosition;
		transform.localRotation = defaultLocalRotation;
	}

	// Update is called once per frame
	public void UpdateStatus () {
		if (targetLocalPosition != Vector3.zero && lerpTime <= 1.0f) {
			transform.localPosition = Vector3.Lerp (startLocalPosition, targetLocalPosition, lerpTime);
			transform.localRotation = Quaternion.Lerp (startLocalRotation, targetLocalRotation, lerpTime);
			lerpTime += updateStep;
		}
	}

}
