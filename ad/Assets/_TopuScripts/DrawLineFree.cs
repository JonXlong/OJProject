using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineFree : MonoBehaviour {

	// Use this for initialization
	private LineRenderer  lineR;
	public  int posCount;

	void Start () {
		
	}
	public void SetLinePos(int posCout,Vector3[]  v3)
	{
		lineR = transform.GetComponent<LineRenderer> ();
		lineR.positionCount = posCout;

		lineR.SetPositions (v3 );
	}

	// Update is called once per frame
	void Update () {
		
	}
}
