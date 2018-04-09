using UnityEngine;
using System.Collections;

public class RotateThisObject : MonoBehaviour {
	public float speed = 40f;
	
	void Update () {

        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
       
	}
}
