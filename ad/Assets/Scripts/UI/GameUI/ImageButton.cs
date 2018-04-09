using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.UI;
//using System.Collections;

public class ImageButton : MonoBehaviour {
	
	private Image image;

	public Sprite OnState;
	public Sprite OffState;

	private bool stateOn = true;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

	public void OnClickSwitchWorld () {
		SetState (!stateOn);
		GameManager.Instance.CommandSwitchWorld (stateOn);
	}

	public void OnClickSwitchSound () {
		SetState (!stateOn);
		GameManager.Instance.CommandSwitchSound (stateOn);
	}

	public void OnClickSwitchCamera () {
		SetState (!stateOn);
		GameManager.Instance.CommandSwitchCamera (stateOn);
	}

	public void OnClickSwitchUI () {
		SetState (!stateOn);

		GameManager.Instance.CommandSwitchUI (stateOn);
	}
    //Add 
    public void StopCamerButtonInit()
    {
        
        image.sprite = OnState;stateOn = true;
        SimWorldManager.Instance.currentWorld.StartRotation();

    }
    public void OnClickStopCamer()
    {
        SetState(!stateOn);
        switch (stateOn )
        {
            case true:
                SimWorldManager.Instance.currentWorld.StartRotation();
                break;
            case false:
                SimWorldManager.Instance.currentWorld.StopRotation();
                break;
            default:
                break;
        }
      
        // GameManager.Instance.CommandSwitchUI(stateOn);
    }
    public void OnClickHideCamerButton()
    {
        Debug.LogError("StopCamera");
        GameManager.Instance.StopMoveCamera(false);
        SimWorldManager.Instance.currentWorld.StartRotation();
    }
	public void SetState(bool isOn) {
		if (isOn) {
			image.sprite = OnState;
		} else {
			image.sprite = OffState;
		}
		stateOn = isOn;
	}

}
