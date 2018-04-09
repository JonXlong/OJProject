using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimTeamVirtual : SimTeamBase {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public override void CreateUnits () {
		unitsDict = new Dictionary<string, SimUnitBase> ();
		simUnits = new List<SimUnitBase> ();
		CreateSubUnits (teamData.units, simUnits, transform);
	}

	private void CreateSubUnits(List<SimUnitData> units, List<SimUnitBase> savedObjects, Transform parent) {
		foreach (SimUnitData unitData in units) {

			GameObject prefab = SimWorldManager.Instance.getSimObjectPrefabByName (unitData.virtual_world.model);
			SimUnitBase simUnit = ((GameObject)Instantiate (prefab, parent, true)).GetComponent<SimUnitVirtual>();
			unitsDict[unitData.id] = simUnit;

			simUnit.gameObject.transform.SetParent (parent);
			simUnit.gameObject.name = unitData.id;

			simUnit.unitData = unitData;

			if (unitData.virtual_world.model != GameConfigs.virtual_empty) {
				savedObjects.Add (simUnit);
			}

			if (unitData.virtual_world.show_children && unitData.units != null && unitData.units.Count > 0) {
				CreateSubUnits (unitData.units, savedObjects, parent);
			}

		}
	}

	public override void InitializeTeam () {
		// create shield
		teamShield = ((GameObject) Instantiate (GameManager.Instance.virtualShield, this.transform)).GetComponent<Forcefield>();
		teamShield.transform.localPosition = Vector3.zero;
		teamShield.team = this;

		score = teamData.score;
	}

	public override void CreateHUD () {
		if (textHud == null) {
			// create team hud
			textHud = gameObject.AddComponent<DUIHUD> ();
            // add blood team hud 
			textHud.TextPrefab = GameManager.Instance.virtualBloodTeamHUD;
            // Debug.Log(teamData.logo);
            //Debug.Log("this is vir"+ teamData.team);
			textHud.Init (teamData.team, teamData.displayTeamName, teamData.logo, new Vector3(0f, 2f, 0f), false,true   );
		}
	}

	public override void RemoveHUD () {
		if (textHud != null) {
            GameManager.Instance.bloodTeamHudList.Remove(teamData.team);
			GameObject.Destroy (textHud);
		}
	}

	public override void ArrangeUnits() {
		
		int unitNum = simUnits.Count;

		if (unitNum > 0) {
			SimUnitBase[] unitAry = simUnits.ToArray();
			SimUnitBase simUnit;

			bool hasLeader = unitAry[0].unitData.type == GameConfigs.vir_att_shipleader;
			float leaderOffsetZ = 0f;

			if (hasLeader) {
				simUnit = unitAry [0];
				simUnit.transform.localPosition = Vector3.zero;

				leaderOffsetZ = GameConfigs.Instance.vir_leaderOffsetZ;
				unitAry = simUnits.GetRange (1, simUnits.Count - 1).ToArray();
				unitNum--;
			}

//			int targetWave = unitNum > 1 ? 1 : 0;
//			int num = GameConfigs.vir_uint_start_num;
//			while (unitNum > num) {
//				num += GameConfigs.vir_uint_inc_step * targetWave;
//				targetWave++;
//			}
//			targetWave = targetWave / GameConfigs.vir_unit_arrange_depth;

			int curDepth = 0;
			int waveCount = 0;
			int curWaveMaxNum = 0;
			int unitIndex = 0;

//			Debug.logger.Log ("units : " + unitNum + " , waves : " + waves);
			while (unitIndex < unitNum) {
				
				curWaveMaxNum = GameConfigs.Instance.vir_uint_start_num + GameConfigs.Instance.vir_uint_inc_step * waveCount;

				// init wave values
				int waveLen = unitNum - unitIndex < curWaveMaxNum ? unitNum - unitIndex : curWaveMaxNum;

				float placeX = GameConfigs.Instance.vir_unit_grid_x;
				float placeY = GameConfigs.Instance.vir_unit_waveY_offset + GameConfigs.Instance.vir_unit_grid_y * curDepth;
				float placeZ = leaderOffsetZ - waveCount * GameConfigs.Instance.vir_unit_grid_z;
				float zOffset = GameConfigs.Instance.vir_unit_waveZ;
				int upOffset = 0;
				int times = 1;

				// place objects in the same wave
				for (int w = 0; w < waveLen; w++) {
					
					// This is odd number wave
					if (w == 0 && curWaveMaxNum % 2 != 0 && waveLen % 2 != 0) {
						// place unit in the middle when the number is odd
						simUnit = unitAry [unitIndex];
						simUnit.transform.localPosition = new Vector3 (0, placeY, placeZ - GameConfigs.Instance.vir_unit_waveZ);
					} else {
						if (upOffset == 2) {
							zOffset = -zOffset;
							upOffset = 0;
							times++;
						}

						simUnit = unitAry [unitIndex];

						if (w % 2 == 0) {
							simUnit.transform.localPosition = new Vector3 (placeX * times, placeY, placeZ + zOffset);
						} else {
							simUnit.transform.localPosition = new Vector3 (-placeX * times, placeY, placeZ + zOffset);
						}

						upOffset++;
					}

					unitIndex++;
				}

				// check use depth
				if (unitNum <= GameConfigs.Instance.vir_unit_usedepth_num) {
					curDepth = 0;
					waveCount++;
				} else {
					curDepth++;
					if (curDepth == GameConfigs.Instance.vir_unit_arrange_depth) {
						curDepth = 0;
						waveCount++;
					}
				}

			}
		}

		foreach (SimUnitBase u in simUnits) {
			u.team = this;
			u.InitializeUnit ();
		}

	}
}