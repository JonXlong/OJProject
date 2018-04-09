using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimWorldManager : MonoBehaviour {
	public static SimWorldManager Instance;

	public SimRealWorld simRealWorld;
  
	public SimVirtualWorld simVirtualWorld;

	[HideInInspector]
	public SimWorldBase currentWorld;

	private bool worldCreated = false;
	private Dictionary<string, SimTeamData> teamsDict;
	private Dictionary<string, SimPuzzleData> puzzlesDict;
   public Dictionary<string, Transform> puzzlesDicTrans;
	private bool m_isZooming = false;

	public void Awake () {
		Instance = this;
	}

	public void CreateWorld(JSONObject teamsObj) {
		if (!worldCreated) {
      //      Debug.LogError(teamsObj);
			List<SimTeamData> teamsData = new List<SimTeamData> ();
			List<JSONObject> teamsJson = teamsObj.GetField ("teams").list;
			teamsDict = new Dictionary<string, SimTeamData> ();
			List<string> logoUrls = new List<string> ();
			foreach (JSONObject teamJson in teamsJson) {
				SimTeamData teamData = SimTeamData.CreateFromJSON (teamJson);
				teamsData.Add (teamData);

				teamsDict[teamData.team] = teamData;

				if (!logoUrls.Contains(teamData.logo)) {
                   
					logoUrls.Add (teamData.logo);
				}
			}

		simRealWorld.CreateWorld (teamsData);
      
       
			simVirtualWorld.CreateWorld (teamsData);

			puzzlesDict = new Dictionary<string, SimPuzzleData> ();
            puzzlesDicTrans = new Dictionary<string, Transform>();
			if (teamsObj.GetField ("puzzles")) {
				List<JSONObject> puzzleObjs = teamsObj.GetField ("puzzles").list;
                
				foreach (JSONObject puzzleJson in puzzleObjs) {
					SimPuzzleData puzzleData = SimPuzzleData.CreateFromJSON (puzzleJson);
                   
					puzzlesDict[puzzleData.name] = puzzleData;
                    //foreach (SimPuzzleData item in puzzlesDict.Values)
                    //{
                    //    Debug.LogError(item.id);
                    //}
				}

             simRealWorld.CreatePuzzles(puzzleObjs);
                simVirtualWorld.CreatePuzzles (puzzleObjs);
			}

			simVirtualWorld.Hide ();
			simRealWorld.Hide ();

			UIManager.Instance.PreloadLogos (logoUrls);
			UIManager.Instance.gameUI.InitGameUI (teamsData);

			worldCreated = true;
		}
     //   Debug.LogError(puzzlesDict.Count);
	}

	public void ActiveVirtualWorld() {
		simVirtualWorld.Show ();
		simRealWorld.Hide ();
		currentWorld = simVirtualWorld;
	}
    /// <summary>
    /// 2017.05.08
    /// </summary>
    public void ActiveVirtualDazzle( string strLeft,string strRight)
    {
        simVirtualWorld.CreatDazzleLight(strLeft ,strRight);
    }
	public void ActiveRealWorld() {
		simVirtualWorld.Hide ();
		simRealWorld.Show ();
		currentWorld = simRealWorld;
	}

	public void ApplyBehavior(SimBehaviorData behavior) {
		UIManager.Instance.gameUI.DisplayBehaviorMessage(behavior);

		currentWorld.ApplyBehavior (behavior);

		//foreach (SimBehaviorDest dest in behavior.dests) {
		//	switch (dest.behavior) {
		//	case GameConfigs.behavior_enhance:
		//	case GameConfigs.behavior_gather:
		//		UpdateUnitStatus (behavior.scr_team, behavior.scr_unit, dest.damages);
		//		break;
		//	default:
		//		UpdateUnitStatus (dest.team, dest.unit, dest.damages);
		//		break;
		//	}
		//}
	}

	private void UpdateUnitStatus (string teamID, string unitId, SimDamagesData damageData) {
		if (teamsDict.ContainsKey(teamID)) {
			SimTeamData teamData = teamsDict [teamID];
			if (teamData.unitsDict.ContainsKey (unitId)) {
				SimUnitData unitData = teamData.unitsDict[unitId];
				unitData.damages.down = damageData.down;
				unitData.damages.occupied = damageData.occupied;
				unitData.damages.pwn = damageData.pwn;
			}
		}
	}

	public string FindTeamLogoById (string teamID) {
		string logoUrl = "";
		if (teamsDict.ContainsKey (teamID)) {
			SimTeamData teamData = teamsDict [teamID];
			logoUrl = teamData.logo;
		}
		return logoUrl;
	}

	//public string FindNameByID(string teamID) {
	//	string name = "";

	//	if (teamID == GameConfigs.NPC_Arbiter) {
	//		name = "仲裁者";
	//	} else if (teamID == GameConfigs.NPC_puzzle) {
	//		name = "水晶";
	//	} else {
			
	//		if (teamsDict.ContainsKey (teamID)) {
	//			SimTeamData teamData = teamsDict [teamID];
	//			name = teamData.displayTeamName;
	//		} else {
	//			name = teamID;
	//		}

	//	}

	//	return name;
	//}

	public string FindPuzzleNameByID(string puzzleID) {
		string name = "";
		if (puzzlesDict.ContainsKey (puzzleID)) {
			SimPuzzleData puzzleData = puzzlesDict [puzzleID];
			name = puzzleData.name;
		}
		return name;
	}

	public GameObject getSimObjectPrefabByName(string prefabName) {
		bool isAvaliableData = false;
		GameObject resultGameObject = null;

		switch (prefabName) {
		case GameConfigs.real_att_laptop:
		case GameConfigs.real_att_desktop:
		case GameConfigs.real_link_firewall:
		case GameConfigs.real_link_router:
		case GameConfigs.real_link_switch:
		case GameConfigs.real_res_server:
		case GameConfigs.vir_att_shipgroup:
		case GameConfigs.vir_att_shipleader:
		case GameConfigs.vir_res_asset:
		case GameConfigs.real_arbiter:
		case GameConfigs.virtual_arbiter:
		case GameConfigs.real_empty:
		case GameConfigs.virtual_empty:
         case GameConfigs.virtual_dazzle:
			isAvaliableData = true;
			break;
		default :
			isAvaliableData = false;
			break;
		}

		if (isAvaliableData) {
			resultGameObject = Resources.Load<GameObject> (prefabName);
		} else {
			Debug.LogError ("Unknow SimObject type :" + prefabName);
		}

		return resultGameObject;
	}

	public void TransportToEarth () {
		m_isZooming = true;
		currentWorld.activeCamera.TransportToEarth ();
	}

	public void TransportToSpace () {
		m_isZooming = true;
		currentWorld.activeCamera.TransportToSpace ();
	}

	public bool IsReadyToTransport {
		get {
			return !m_isZooming && currentWorld != null && currentWorld.isActiveAndEnabled && currentWorld.activeCamera != null;
		}
	}
    Transform targetTran;
	public bool ShowFirstBlood (Transform followObject, string teamA, string teamB, string puzzle, string title) {
		if (!m_isZooming && currentWorld != null && currentWorld.isActiveAndEnabled && currentWorld.activeCamera != null) {
			m_isZooming = true;

			if (puzzle.Length == 0) {
                targetTran = null;
				currentWorld.activeCamera.FirstBlood (followObject, title + "\n" + teamA + "->" + teamB, false,targetTran);
               
            } else {
                Debug.Log("Title firstblood");
               
               targetTran= puzzlesDicTrans[puzzle] ; 
                
                 currentWorld.activeCamera.FirstBlood (followObject, title + "\n" + teamA + "->" + FindPuzzleNameByID (puzzle), true,targetTran);
			}

			return true;
		}
		return false;
	}

	public bool isZooming {
		get {
			return m_isZooming;
		}
	}

	public void EndZooming () {
		m_isZooming = false;
	}

	public void DebugAttack() {
		simVirtualWorld.DebugAttack ();
	}

}