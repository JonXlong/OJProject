using UnityEngine;
//using System.Collections;
using System;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using UnityEngine.Experimental.Director;
//using UnityEngine.Internal;
//using UnityEngine.Scripting;

public class EarthTransportScript : MonoBehaviour {

	public Animator animatorCam;
	public Animator animatorImg;
	private bool toEarth;
    public GameObject waitSceneObj;
    private Action completeAnimFun;

	void Start () {
	}

	public void Play (bool _toEarth, Action _completeAnimFun) {
		toEarth = _toEarth;
		gameObject.SetActive (true);
		completeAnimFun = _completeAnimFun;

		if (toEarth) {
			animatorImg.Play ("EarthImageAnim", 0, 0f);
			animatorImg.SetFloat("SingSpeed", 1f);

			animatorCam.Play ("ToEarth", 0, 0f);
			animatorCam.SetFloat("SingSpeed", 1f);
		} else {
			animatorImg.Play ("EarthImageAnim", 0, 1f);
			animatorImg.SetFloat("SingSpeed", -1f);

			animatorCam.Play ("ToEarth", 0, 1f);
			animatorCam.SetFloat("SingSpeed", -1f);
            Invoke("SetBack",1.0f);
		}

	}
    private void SetBack()
    {
        isBack = true;
    }
    private bool isBack = false;
	void Update () {
		AnimatorStateInfo info = animatorCam.GetCurrentAnimatorStateInfo(0);

		if (toEarth) {
			if (info.normalizedTime >= 1.0f) {
				if (animatorCam) {

					animatorCam.StopPlayback ();
				}
				gameObject.SetActive (false);
				if (completeAnimFun != null) {
                    if (GameManager.Instance.isWait)
                    {
                    waitSceneObj.SetActive(true);


                    }
                    completeAnimFun();
                    // change hide		completeAnimFun ();
                    //if (GameManager.Instance.gameObject.GetComponent<AudioListener>())
                    //{
                    //    GameManager.Instance.gameObject.GetComponent<AudioListener>().enabled = false;
                    //}
                }
			}
		} else {
            if (isBack )
            {
                if (info.normalizedTime <= 0.05f)
                {
                    if (animatorCam)
                    {

                        animatorCam.StopPlayback();
                    }
                    gameObject.SetActive(false);
                    if (completeAnimFun != null)
                    {
                        if (GameManager.Instance.isWait)
                        {
                            waitSceneObj.SetActive(true);

                        }
                        completeAnimFun();
                       
                        isBack = false;
                    }
                }
            }
			
		}


	}

}
