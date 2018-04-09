using UnityEngine;
using System.Collections;

public class MissileBase : MonoBehaviour {

	protected SimUnitBase fromUnit;
	protected SimUnitBase targetUnit;
	protected SimBehaviorDest dest;

	protected float launchedTime = 0f;
	protected float reachTime = 0;
	protected bool firing = false;
	protected bool hasShowFirstBlood = false;
	protected bool isShowingFirstBlood = false;
	protected float speed = 0f;
    protected bool isPuzzle = false;

    // Use this for initialization
    void Start () {
	
	}

	protected void CheckFirstBlood() {
		if (!hasShowFirstBlood && dest != null && dest.firstblood) {
            //			if ((reachTime - launchedTime) / SimCamera.firstBloodTimeScale <= SimCamera.firstBloodTime) {
            
            if (isPuzzle) {
                Debug.Log("+++++++++++++++++++++++++++++");
                isShowingFirstBlood = SimWorldManager.Instance.ShowFirstBlood(this.transform, fromUnit.FriendlyTeamName, "", dest.unit, dest.firstblood_message);
            } else {
                Debug.Log("______________________");
                isShowingFirstBlood = SimWorldManager.Instance.ShowFirstBlood(this.transform, fromUnit.FriendlyTeamName, targetUnit.FriendlyTeamName, "", dest.firstblood_message);
            }
            
			hasShowFirstBlood = true;
//			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
