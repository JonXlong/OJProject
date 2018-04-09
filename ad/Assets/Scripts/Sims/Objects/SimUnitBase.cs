using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SimUnitBase : MonoBehaviour {
	
	[HideInInspector]
	public SimUnitData unitData;
	[HideInInspector]
	public List<SimUnitBase> children = null;
	[HideInInspector]
	public SimTeamBase team;
	[HideInInspector]
	public SimDamagesData damages = new SimDamagesData();

	protected List<GameObject> tempEffects = new List<GameObject> ();

	public Transform gunTrans;
	public GameObject model;

	//public Collider unitCollider {
	//	get {
	//		Collider c;
	//		if (model != null) {
	//			c = model.GetComponent<Collider> ();
	//		} else {
	//			c = GetComponent<Collider> ();
	//		}
	//		return c;
	//	}
	//}

	public void UpdateScore(int score) {
		ShowScoreHUD (score);

		if (team != null) {
			team.score += score;
		}

	}

	protected void ShowScoreHUD (int score) {
		if (score != 0) {
			string scoreStr = score.ToString ();

			if (score > 0) {
				scoreStr = "+" + score.ToString ();
			}

			Color color = Color.green;
			if (score < 0) {
				color = Color.red;
			}

			GameObject scoreHudGo = (GameObject) Instantiate (GameManager.Instance.scoreHUD, GameObject.Find ("UI/Canvas3D").transform);
			scoreHudGo.transform.position = transform.position;

			ScoreHUD scoreHUD = scoreHudGo.GetComponent<ScoreHUD>();
			scoreHUD.fromObject = gameObject;
			scoreHUD.PlayFloating (scoreStr, color, 1f);
		}

	}

	/** Already has team data **/
	abstract public void InitializeUnit ();

	protected void UpdateDamangeStatus(SimDamagesData newDamages) {
		if (newDamages.occupied != damages.occupied) {
			ApplyDamageOccupied (newDamages.occupied);
			damages.occupied = newDamages.occupied;
		}

		if (newDamages.down != damages.down) {
			ApplyDamageDown (newDamages.down);
			damages.down = newDamages.down;
		}

		if (newDamages.pwn != damages.pwn) {
			ApplyDamagePwn (newDamages.pwn);
			damages.pwn = newDamages.pwn;
		}
	}

	virtual public Vector3 gunPos {
		get {
			return gunTrans == null ? transform.position : gunTrans.transform.position;
		}
	}

	private static SimDamagesData noStatus = new SimDamagesData();
	public void ResetStatus() {
		UpdateDamangeStatus (noStatus);
	}

	public void RefreshDamageStatus() {
		UpdateDamangeStatus (unitData.damages);
	}

    abstract protected void ApplyDamageOccupied (bool enable);
	abstract protected void ApplyDamageDown (bool enable);
	abstract protected void ApplyDamagePwn (bool enable);

	abstract public void ApplyBehavior (SimUnitBase targetUnit, SimBehaviorDest behaviorDest);
	abstract public void UpdateDamages (SimBehaviorDest dest);

	protected void AddTempEffect (GameObject go) {
		tempEffects.Add (go);
	}

	protected void RemoveTempEffect (GameObject go) {
		tempEffects.Remove (go);
		Destroy (go);
	}

	public void RemoveAllTempEffects () {
		foreach (GameObject go in tempEffects) {
			Destroy (go);
		}
		tempEffects = new List<GameObject> ();
	}

    virtual public string FriendlyTeamName
    {
        get
        {
            return team.teamData.displayTeamName;
        }
    }

}