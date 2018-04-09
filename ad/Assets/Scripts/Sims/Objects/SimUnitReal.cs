using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public class SimUnitReal : SimUnitBase {

	public RealLinkDot linkDot;

	public GameObject statusPWN;
	public GameObject statusOccupied;
	public GameObject statusDown;

	override public void InitializeUnit() {
		UpdateDamangeStatus (unitData.damages);

		if (unitData.type != GameConfigs.real_empty) {
			GameObject ringEffect = (GameObject) Instantiate (GameManager.Instance.realUnitRing, transform);
			ringEffect.transform.localPosition = Vector3.zero;

			MeshRenderer renderer = ringEffect.GetComponentInChildren<MeshRenderer> ();
			if (renderer != null) {
				renderer.material.SetColor ("_EmissionColor", GameConfigs.Instance.GetRealWorldColorByName(unitData.color));
			}
		}

	}

	override public void ApplyBehavior(SimUnitBase targetUnit, SimBehaviorDest dest) {
		switch (dest.behavior) {
		case GameConfigs.behavior_attack_weak:
		case GameConfigs.behavior_attack_moderate:
		case GameConfigs.behavior_attack_heavy:
		case GameConfigs.behavior_attack_charge:
			
			if (dest.isIntercepted) {
				SimUnitBase firewall = SimWorldManager.Instance.currentWorld.findUnitById (dest.team, dest.intercepted);
				if (firewall != null) {
					targetUnit = firewall;
				}
			}

			Attack (targetUnit as SimUnitReal, dest);
			break;
		case GameConfigs.behavior_enhance:
			ApplyEnhanceEffect ();
			UpdateDamages (dest);
			break;
		case GameConfigs.behavior_gather:
			// update gather result
			if (dest.gather_result) {
				GatherSuccess (targetUnit as SimUnitReal, dest);
			} else {
				GatherFailed (targetUnit as SimUnitReal, dest);
			}
			break;
		}

		UpdateScore (dest.score.src_addscore);
	}

	private void Attack (SimUnitReal targetUnit, SimBehaviorDest dest) {
//		GameObject missilePrefab = GameManager.Instance.realMissile;
//
//		// attacks
//		if (missilePrefab != null) {
//			GameObject missileGO = (GameObject)Instantiate (missilePrefab, SimWorldManager.Instance.currentWorld.transform);
//			RealMissileAttack missileProjectile = missileGO.GetComponent<RealMissileAttack> ();
//			missileProjectile.Launch (this, targetUnit, dest);
//			if (team != null) {
//				missileProjectile.SetColor (GameConfigs.Instance.GetColorByName(team.teamData.color));
//			} else {
//				missileProjectile.SetColor (Color.white);
//			}
//
//		}

		GameObject effect = (GameObject) Instantiate (GameManager.Instance.realMissile, SimWorldManager.Instance.currentWorld.missiles);
		WayPointMover wpMover = effect.GetComponent<WayPointMover>();

		Color teamColor = Color.white;
		if (team != null) {
			teamColor = GameConfigs.Instance.GetRealWorldColorByName (team.teamData.color);
		}

		Vector3[] fWaypoints = GetWayPointToArbiter ();
		Vector3[] tWaypoints = targetUnit.GetWayPointToArbiter ();
		System.Array.Reverse(tWaypoints);

		Vector3[] waypoints = new Vector3[fWaypoints.Length + tWaypoints.Length];
		fWaypoints.CopyTo(waypoints, 0);
		tWaypoints.CopyTo(waypoints, fWaypoints.Length);

		wpMover.Launch (this, targetUnit, dest, waypoints, teamColor, false);
	}

	public Vector3[] GetWayPointToArbiter() {
//		bool xDir = true;

		List<Vector3> wayDots = new List<Vector3> ();
//		wayDots.Add (linkDot);

		RealLinkDot curDot = linkDot;
		while (curDot != null) {
			
//			if (curDot.prev != null) {
//				if (xDir) {
//					if (curDot.transform.position.z != curDot.prev.transform.position.z) {
//						xDir = !xDir;
//						wayDots.Add (curDot);
//					}
//				} else {
//					if (curDot.transform.position.x != curDot.prev.transform.position.x) {
//						xDir = !xDir;
//						wayDots.Add (curDot);
//					}
//				}
//			} else {
//				// add arbiter dot
			wayDots.Add (new Vector3(curDot.transform.position.x, 0.2f, curDot.transform.position.z));
//			}

			curDot = curDot.prev;
		}

//		return System.Array.ConvertAll (wayDots.ToArray (), t => new Vector3(t.transform.position.x, 0.2f, t.transform.position.z));
		return wayDots.ToArray ();
	}

	override public void UpdateDamages(SimBehaviorDest dest) {
		UpdateDamangeStatus (dest.damages);

		if (dest.isIntercepted) {
			ShowShieldEffect ();
		}

		GameObject effectPrefab = null;
		switch (dest.behavior) {
		case GameConfigs.behavior_attack_weak:
			ShowAttackChargeShake ();
			break;
		case GameConfigs.behavior_attack_moderate:
			effectPrefab = GameManager.Instance.realAttackImpackMidEffect;
			ShowAttackChargeShake ();
			break;
		case GameConfigs.behavior_attack_heavy:
			effectPrefab = GameManager.Instance.realAttackHeavyImpackEffect;
			ShowAttackChargeShake ();
			break;
		case GameConfigs.behavior_attack_charge:
			ShowAttackChargeShake ();
			break;
		}

		if (effectPrefab != null) {
			GameObject effect = (GameObject)GameObject.Instantiate (effectPrefab, transform);
			effect.transform.position = transform.position;

			AddTempEffect (effect);
		}
	}

	private void ShowAttackChargeShake() {
		iTween.ShakePosition(model, iTween.Hash("x", 0.3f, "time", 1.0f));
	}

	// display shiled effect
	private void ShowShieldEffect () {
		GameObject shieldEffect = (GameObject)GameObject.Instantiate (GameManager.Instance.realShieldEffect, gunTrans);
		shieldEffect.transform.position = gunPos;

		AddTempEffect (shieldEffect);
	}

	// occupied status
	protected override void ApplyDamageOccupied (bool enable) {
		if (enable) {
			if (statusOccupied == null) {
				statusOccupied = (GameObject)GameObject.Instantiate (GameManager.Instance.realDamageOccupied, transform);
				statusOccupied.transform.position = transform.position;
			}
		} else {
			if (statusOccupied != null) {
				Destroy (statusOccupied);
			}
		}
	}

	// pwn status
	protected override void ApplyDamagePwn (bool enable) {
		if (enable) {
			if (statusPWN == null) {
				statusPWN = (GameObject)GameObject.Instantiate (GameManager.Instance.realDamagePWN, gunTrans);
				statusPWN.transform.position = gunPos;
			}
		} else {
			if (statusPWN != null) {
				Destroy (statusPWN);
			}
		}
	}

	// down status
	protected override void ApplyDamageDown (bool enable) {
		if (enable) {
			if (statusDown == null) {
				statusDown = (GameObject)GameObject.Instantiate (GameManager.Instance.realDamageDown, gunTrans);
				statusDown.transform.position = gunPos;
			}
		} else {
			if (statusDown != null) {
				Destroy (statusDown);
			}
		}
	}

	private void ApplyEnhanceEffect () {
		GameObject effect = (GameObject) Instantiate (GameManager.Instance.virtualEnhanceEffect, transform);
		effect.transform.position = transform.position;

		AddTempEffect (effect);
	}

	private void DoGather(SimUnitReal targetUnit, Color color, SimBehaviorDest dest) {
		GameObject effect = (GameObject) Instantiate (GameManager.Instance.realGatherEffect, SimWorldManager.Instance.currentWorld.missiles);
		WayPointMover wpMover = effect.GetComponent<WayPointMover>();

		Vector3[] waypoints = GetWayPointToArbiter ();
		System.Array.Reverse(waypoints);

        wpMover.Launch (this, targetUnit, dest, waypoints, color, true);
	}

	private void GatherSuccess(SimUnitReal targetUnit, SimBehaviorDest dest) {
		DoGather (targetUnit, Color.green, dest);
	}

	private void GatherFailed(SimUnitReal targetUnit, SimBehaviorDest dest) {
		DoGather (targetUnit, Color.red, dest);
	}

}