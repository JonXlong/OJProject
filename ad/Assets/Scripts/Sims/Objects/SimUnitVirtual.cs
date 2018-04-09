using UnityEngine;
using System.Collections;

public class SimUnitVirtual : SimUnitBase {
	
	public GameObject flame;

//	public Light[] lights;

	[HideInInspector]
	public SimPuzzleData puzzleData;

	private bool rotationUpdate = false;

	protected Vector3 defaultModelRotation;

	protected const float gatherEffectTime = 2f;

	override public void InitializeUnit() {
		defaultModelRotation = Vector3.zero;

		if (model != null) {
			defaultModelRotation = model.transform.localEulerAngles;
		}

		FloatingAnimation ();
		OpenTeamLight ();
//		SetUnitMateral ();

		UpdateDamangeStatus (unitData.damages);
        UpdateFlameByDamages();
    }

	private void FloatingAnimation() {
		Hashtable args = new Hashtable();

		args.Add("easeType", iTween.EaseType.linear);
		args.Add("speed", Random.Range(0f, 0.2f) + 0.1f);
		args.Add("loopType", "pingPong");
		args.Add("useRealTime", true);

		args.Add("isLocal", true);
		args.Add("onstarttarget", gameObject);
		args.Add("onupdatetarget", gameObject);
		args.Add("onupdateparams", true);

		args.Add("y", transform.localPosition.y + 0.5f);

		iTween.MoveTo(gameObject,args);
	}

	void Update () {
		if (rotationUpdate) {
			model.transform.Rotate (Vector3.up, 1.5f);
		}
	}

	private void OpenTeamLight () {
		string matPath = "Materials/VritualMat/" + unitData.color;
		SetUnitMateral (matPath);
	}

	private void CloseTeamLight () {
		string matPath = "Materials/VritualMat/Shadow";
		SetUnitMateral (matPath);

//		if (lights != null) {
////			Color teamColor = GameConfigs.Instance.GetColorByName(team.teamData.color);
//			foreach (Light lt in lights) {
//				lt.gameObject.SetActive (false);
//			}
//		}
	}

	private void SetUnitMateral(string matPath) {
		Material mat = Resources.Load<Material>(matPath);

		if (mat != null && model.GetComponent<Renderer> () != null) {
			Renderer renderer = model.GetComponent<Renderer> ();
			renderer.material = new Material (mat);
		}
	}

	override public void ApplyBehavior(SimUnitBase targetUnit, SimBehaviorDest dest) {
      //  Gather(dest, targetUnit.gameObject);
        switch (dest.behavior) {
		case GameConfigs.behavior_attack_weak:
		case GameConfigs.behavior_attack_moderate:
		case GameConfigs.behavior_attack_heavy:
			Attack (targetUnit, dest, 0f);
			UpdateScore (dest.score.src_addscore);
			break;
		case GameConfigs.behavior_attack_charge:
			StartCoroutine(ChargeReady (targetUnit, dest));
			UpdateScore (dest.score.src_addscore);
			break;
		case GameConfigs.behavior_enhance:
			ApplyEnhanceEffect ();
			UpdateDamages (dest);
			UpdateScore (dest.score.src_addscore);
			break;
		case GameConfigs.behavior_gather:
			// update gather result
			Gather(dest, targetUnit.gameObject);
			break;
		}
	}

	private IEnumerator ChargeReady (SimUnitBase targetUnit, SimBehaviorDest dest) {
		GameObject effect = (GameObject) Instantiate (GameManager.Instance.missileVirtualCharge_ready, gunTrans);
		effect.transform.position = gunPos;
		AddTempEffect (effect);

		Attack (targetUnit, dest, 1.9f);

		yield return new WaitForSeconds (2f);
		RemoveTempEffect (effect);
	}

	private void Attack (SimUnitBase targetUnit, SimBehaviorDest dest, float delayLaunchTime) {
		GameObject missilePrefab = GetMissilePrefab (dest.behavior);
		MissileProjectile missileProjectile = null;

		// attacks
		if (missilePrefab != null) {
			GameObject missileGO = (GameObject)Instantiate (missilePrefab, SimWorldManager.Instance.currentWorld.missiles);
			missileProjectile = missileGO.GetComponent<MissileProjectile> ();

			missileProjectile.Launch (this, targetUnit, dest, delayLaunchTime);
		}
	}

	private GameObject GetMissilePrefab(string behavior) {
		GameObject missilePrefab = null;

		switch (behavior) {
		case GameConfigs.behavior_attack_weak:
			missilePrefab = GameManager.Instance.missileVirtualWeak;
			break;
		case GameConfigs.behavior_attack_moderate:
			missilePrefab = GameManager.Instance.missileVirtualMid;
			break;
		case GameConfigs.behavior_attack_heavy:
			missilePrefab = GameManager.Instance.missileVirtualHeavy;
			break;
		case GameConfigs.behavior_attack_charge:
			missilePrefab = GameManager.Instance.missileVirtualCharge_attack;
			break;
		}

		return missilePrefab;
	}

	override public void UpdateDamages(SimBehaviorDest dest) {
		UpdateDamangeStatus (dest.damages);
        UpdateFlameByDamages ();
    }

    private void UpdateFlameByDamages () {
        bool flameStatus = true;
        if (damages.occupied || damages.down || damages.pwn)
        {
            flameStatus = false;
        }

        if (flame != null)
        {
            flame.gameObject.SetActive(flameStatus);
        }
    }

	// occupied status: pingpong rotation
	protected override void ApplyDamageOccupied (bool enable) {
//		Debug.Log ("ApplyDamageOccupied");
		if (enable) {
			rotationUpdate = true;
		} else {
			rotationUpdate = false;

			Hashtable rotArgs = new Hashtable ();
			rotArgs.Add("easeType", iTween.EaseType.linear);
			rotArgs.Add ("isLocal", true);
			rotArgs.Add ("time", 1f);
			rotArgs.Add ("x", defaultModelRotation.x);
			rotArgs.Add ("y", defaultModelRotation.y);
			rotArgs.Add ("z", defaultModelRotation.z);
			iTween.RotateTo (model.gameObject, rotArgs);

//			iTween.LookTo (model.gameObject, Vector3.zero, 1f);
//			iTween.RotateTo (model, defaultModelRotation, 1f);
		}
	}

	// down status: change color
	protected override void ApplyDamagePwn (bool enable) {
//		Debug.Log ("ApplyDamageDown");

//		if (model != null) {
//			
//			Color toColor = Color.white;
//			if (enable) {
//				toColor = new Color (0.3f, 0.3f, 0.3f, 1f);//Color.grey;
//			}
//
//			Renderer renderer = model.GetComponent<Renderer> ();
//			if (renderer != null) {
//				renderer.material.color = toColor;
//			}
//		}

		if (enable) {
			CloseTeamLight ();
		} else {
			OpenTeamLight ();
		}
	}

	// pwn status: add PWN effect
	protected override void ApplyDamageDown (bool enable) {
//		Debug.Log ("ApplyDamagePwn");
//		if (pwnGameObject == null) {
//			pwnGameObject = (GameObject)Instantiate (GameManager.Instance.virtualDamagePWN, transform);
//			pwnGameObject.transform.position = transform.position;
//		}
//		pwnGameObject.gameObject.SetActive (enable);

        if (enable) {
            Renderer renderer = model.GetComponent<Renderer>();
            if (renderer != null) {
                Color orginalColor = renderer.material.GetColor("_Color");
                renderer.material.SetColor("_Color", new Color(orginalColor.r, orginalColor.g, orginalColor.b, 0.2f));
            }
        } else {
            Renderer renderer = model.GetComponent<Renderer>();
            if (renderer != null) {
                Color orginalColor = renderer.material.GetColor("_Color");
                renderer.material.SetColor("_Color", new Color(orginalColor.r, orginalColor.g, orginalColor.b, 1f));
            }
        }

		//if (flame != null) {
		//	flame.gameObject.SetActive (!enable);
		//} else {
		//}
	}

	private void ApplyEnhanceEffect () {
		GameObject effect = (GameObject) Instantiate (GameManager.Instance.virtualEnhanceEffect, transform);
		effect.transform.position = transform.position;

		AddTempEffect (effect);
	}

	private void Gather(SimBehaviorDest dest, GameObject targetStone) {
		VritualGatherProjectile gatherEffect = CreateGatherEffect (targetStone);
        gatherEffect.Gathering ();


        StartCoroutine (ShowGatherResult(gatherEffect, dest));
	}

	private VritualGatherProjectile CreateGatherEffect (GameObject targetStone) {
		GameObject effect = (GameObject)Instantiate (GameManager.Instance.virtualGatherEffect, gunTrans);

		effect.transform.position = gunPos;
		//effect.transform.SetParent (transform, true);

//		Vector3 targetDir = Vector3.zero - transform.position;
//		float angle = 50f;//Vector3.Angle (transform.forward, targetDir);
//		effect.transform.localRotation = Quaternion.Euler (new Vector3(angle, 0f, 0f));

		effect.transform.LookAt (targetStone.transform.position);

		VritualGatherProjectile gatherEffect = effect.GetComponent<VritualGatherProjectile> ();
		gatherEffect.Play (targetStone.transform);

        AddTempEffect(effect);
//		Vector3 dotB = gunPos;
//		Vector3 dotA = Vector3.zero;
//
//		float angle = Mathf.Atan2 ((dotA.x - dotB.x), (dotA.z - dotB.z));
//		float xpos = GameConfigs.Instance.circle_1_radii * Mathf.Cos (angle);
//		float zpos = GameConfigs.Instance.circle_1_radii * Mathf.Sin (angle);
//
//		Vector3 targetPoint = new Vector3 (xpos, -5f, zpos);

        gatherEffect.gameObject.SetActive (true);

		return gatherEffect;
	}

    IEnumerator ShowGatherResult(VritualGatherProjectile gatherEffect, SimBehaviorDest dest) {
        yield return new WaitForSeconds(gatherEffectTime);
        
        bool isShowFirstBool = false;
        if (dest.firstblood) {
           isShowFirstBool = SimWorldManager.Instance.ShowFirstBlood(transform, FriendlyTeamName, "", dest.unit, dest.firstblood_message);
        }

        if (dest.gather_result) {
            gatherEffect.GatherSuccess();
        }
        else {
            gatherEffect.GatherFailed();
        }

        StartCoroutine(EndGather(gatherEffect, isShowFirstBool, dest));
    }

    IEnumerator EndGather(VritualGatherProjectile gatherEffect, bool isShowingFirstBlood, SimBehaviorDest dest) {
		yield return new WaitForSeconds(gatherEffectTime);

		if (isShowingFirstBlood) {
            //add  shake camera
            SimWorldManager.Instance.currentWorld.activeCamera.gameObject.GetComponent<ShakeCamera>().BeginShake();
//创建烟花
            //GameManager.Instance.CreateFrieWork();
            SimWorldManager.Instance.currentWorld.activeCamera.FollowMissileEnd ();
		}

		UpdateScore (dest.score.src_addscore);

		gatherEffect.RemoveBeam ();
	}

}