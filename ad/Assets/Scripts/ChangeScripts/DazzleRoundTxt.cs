using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DazzleRoundTxt :MonoBehaviour {

    // Use this for initialization
    public GameObject  DazzleObj;
    public  bool IsRound = false;
    public  Vector3 OffsetV3;
    public Text RoundTxtLeft;
    public Text RoundTxtRight;
    public  RectTransform DazzleRect;
    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {
        if (!IsRound)
        {
            SimCamera cam = SimWorldManager.Instance.currentWorld.activeCamera;
            if (cam)
            {
                Vector3 position = DazzleObj.transform.position + OffsetV3;
                this.transform.position = position;
                transform.LookAt(cam.transform, Vector3.up);
            }
           
        }
    }
}
