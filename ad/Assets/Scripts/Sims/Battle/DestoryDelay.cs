using UnityEngine;
using System.Collections;

public class DestoryDelay : MonoBehaviour {
	public float delayTime = 1f;

	// Use this for initialization
	void Start () {
		Destroy (this, delayTime);
	}
}
