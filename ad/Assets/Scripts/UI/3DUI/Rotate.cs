using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	public Vector3 rotationAxis;

	protected void Update()
	{
		transform.Rotate(rotationAxis * Time.deltaTime);
	}
}