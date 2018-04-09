using UnityEngine;
using System.Collections;

public class ActivateDelay : MonoBehaviour {

	public GameObject delayGameObject;
	public float delayTime;

	// Use this for initialization
	void Start () {
		
	}

	IEnumerator DelayActive () {
		yield return new WaitForSeconds (3f);
		delayGameObject.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
