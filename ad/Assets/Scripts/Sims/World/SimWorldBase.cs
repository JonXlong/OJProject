using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public abstract class SimWorldBase : MonoBehaviour {
	[HideInInspector]
	public enum CAMERATYPE {
		normal,
		roaming
	}

	[HideInInspector]
	public enum WORLDATYPE {
		virtual_world,
		real_world
	}

	[HideInInspector]
	public SimCamera activeCamera;
	[HideInInspector]
	public SimUnitBase arbiter;

    /// <summary>
    /// 2017.05.08 
    /// To Change Add dazzle light on Arbiter to Show RoundNumber
    /// </summary>
    public GameObject DazzleObject;
	[HideInInspector]
	public List<SimTeamBase> simTeams = new List<SimTeamBase> ();

	public Transform missiles;
	public PivotController pivotController;

	public WORLDATYPE type;

	private bool rotationStart = false;

	public abstract void CreateWorld (List<SimTeamData> teamsData);

	private void CreateTeamsHUD () {
		foreach (SimTeamBase team in simTeams) {
			team.CreateHUD ();
		}
	}

	private void RemoveTeamsHUD () {
		foreach (SimTeamBase team in simTeams) {
			team.RemoveHUD ();
		}
	}

	public void ApplyBehavior (SimBehaviorData behaviorData) {
       
		SimUnitBase fromUnit = findUnitById(behaviorData.scr_team, behaviorData.scr_unit);

		if (fromUnit != null) {
			foreach (SimBehaviorDest dest in behaviorData.dests) {
				SimUnitBase toUnit = findUnitById (dest.team, dest.unit);
				if (toUnit != null) {
					fromUnit.ApplyBehavior (toUnit, dest);
				} else {
//					Debug.Log ("Unit :" + dest.unit + " in team :" + dest.team + " not found!");
				}
			}
		} else {
//			Debug.Log ("Unit :" + behaviorData.scr_unit + " in team :" + behaviorData.scr_team + " not found!");
		}
	}

	public SimUnitBase findUnitById (string teamId, string uid) {
		SimUnitBase rUnit = null;

		if (teamId == GameConfigs.NPC_Arbiter) {
			rUnit = arbiter;
		} else if (teamId == GameConfigs.NPC_puzzle) {
			rUnit = FindPuzzle (uid);
		} else {
			SimTeamBase rTeam = null;

			foreach (SimTeamBase team in simTeams) {
				if (team.teamData.team == teamId) {
					rTeam = team;
					break;
				}
			}

			if (rTeam != null) {
				rUnit = rTeam.findUnitById (uid);
			}
		}

		return rUnit;
	}

	public void ResetAllUnits () {
		if (simTeams != null) {
			foreach (SimTeamBase team in simTeams) {
				team.ResetAllUnits ();
			}
		}
	}

	private void RefreshUnitsDamageStatus () {
		if (simTeams != null) {
			foreach (SimTeamBase team in simTeams) {
				team.RefreshUnitsDamageStatus ();
			}
		}
	}

	public abstract SimUnitBase FindPuzzle (string uid);
	protected virtual void OnShowWorld () {
	}
	protected virtual void OnHideWorld () {
		foreach (SimTeamBase team in simTeams) {
			team.RemoveAllUnitsEffects ();
		}
	}

	protected abstract void CreateArbiter ();
    /// <summary>
    /// 2017.05.08  
    /// To Change  ：Add dazzle light  on Arbiter to show RoundNumber.
    /// </summary>
    public  abstract void CreatDazzleLight(string strLeft,string strRight);

    protected abstract void ArrangeTeams ();

	public void StopRotation() {
		rotationStart = false;
	}

	public void StartRotation() {
		rotationStart = true;
	}

	// Update is called once per frame
	void Update () {
		if (rotationStart && activeCamera && !activeCamera.isFreeLookCameraControlling) {
			pivotController.Rotate ();
		}
	}

	public void Show () {
		gameObject.SetActive (true);
      
		activeCamera.gameObject.SetActive (true);

		pivotController.InitPivotRotation (GameConfigs.Instance.virtual_cam_rotationX, GameConfigs.Instance.virtual_cam_rotationY);

		StartRotation ();
		RefreshUnitsDamageStatus ();

		CreateTeamsHUD ();
		OnShowWorld ();
	}

	private void ResetCamera() {
		pivotController.ResetStatus ();
		if (activeCamera != null) {
			activeCamera.ResetStatus ();
			activeCamera.ResetFreeLookCamera ();
		}
	}

	public void SetCameraType(CAMERATYPE type) {
		pivotController.OnSetRotationType (type);
		if (activeCamera != null) {
			activeCamera.ResetCameraStatus ();
			activeCamera.ResetFreeLookCamera ();
		}
	}

	public void Hide () {
		RemoveTeamsHUD ();
		OnHideWorld ();
		gameObject.SetActive (false);
	}

}