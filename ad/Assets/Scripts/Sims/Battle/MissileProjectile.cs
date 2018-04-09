using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileProjectile : MissileBase {
	private const float shieldRadius = 0.5f;

	private Vector3 startPoint;
	private Vector3 endPoint;

    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject[] trailParticles;
	[HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

	private Vector3 launchVelocity;
	private GameObject m_projectileParticle;

	private string missileType;
	private bool hitShield = false;

	public const string missile_attack = "missile_attack";
	public const string missile_arbiter = "missile_arbiter";

	// Use this for initialization
	void Start () {
		m_projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation, transform) as GameObject;

		for (int i = 0; i < trailParticles.Length; i++) {
			trailParticles [i] = m_projectileParticle.transform.FindChild (trailParticles [i].name).gameObject;
		}
	}

	public void Launch(SimUnitBase _fromUnit, SimUnitBase _targetUnit, SimBehaviorDest _dest, float _delayLaunchTime) {
		speed = GameConfigs.Instance.vir_missile_speed;

		dest = _dest;
		fromUnit = _fromUnit;
		targetUnit = _targetUnit;

		startPoint = fromUnit.gunPos;
		endPoint = targetUnit.transform.position;

		launchedTime = 0;
		this.transform.position = startPoint;

		if (dest.isIntercepted) {
			hitShield = true;
            if (targetUnit.team != null) {
                endPoint = targetUnit.team.teamShield.transform.position;
            } else {
                hitShield = false;
            }
		} else {
			hitShield = false;
		}

		float distance = Vector3.Distance(startPoint, endPoint);
		reachTime = distance / speed;

		if (fromUnit is SimArbiterVirtual) {
			missileType = missile_arbiter;
		} else {
			missileType = missile_attack;
			launchVelocity = ProjectileHelper.ComputeVelocityToHitTargetAtTime (startPoint, endPoint, GameConfigs.Instance.virtual_gravity, reachTime);
		}

		if (_delayLaunchTime > 0) {
			CheckFirstBlood ();
			Invoke ("DelayLaunch", _delayLaunchTime);
		} else {
			firing = true;
		}
	}

	private void DelayLaunch() {
		firing = true;
	}

	// Update is called once per frame
	void Update () {
		
		if (firing) {
			
			float moveTime = Time.deltaTime;
			transform.LookAt (endPoint);
			CheckFirstBlood ();

			launchedTime += moveTime;
			if (missileType == missile_attack) {
				Vector3 pos = transform.position;
				ProjectileHelper.UpdateProjectile (ref pos, ref launchVelocity, GameConfigs.Instance.virtual_gravity, moveTime);
				transform.position = pos;
			} else if (missileType == missile_arbiter) {
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, endPoint, step);
			}

			if (hitShield && Vector3.Distance (endPoint, transform.position) <= shieldRadius) {
//				transform.Translate (Vector3.back * (shieldRadius - Vector3.Distance(endPoint, pos)), Space.Self);

				Forcefield targetShield = targetUnit.team.teamShield.GetComponent<Forcefield> ();

				Vector3 v = transform.position - targetShield.transform.position;
				v = v.normalized;
				transform.position = targetShield.transform.position + v * shieldRadius;

				float hitPower = 0.0f;
				float hitAlpha = 1.0f;
				switch (dest.behavior) {
				case GameConfigs.behavior_attack_weak:
					hitPower = 0.1f;
					break;
				case GameConfigs.behavior_attack_moderate:
					hitPower = 0.5f;
					break;
				case GameConfigs.behavior_attack_heavy:
					hitPower = 0.8f;
					break;
				case GameConfigs.behavior_attack_charge:
					hitPower = 1.5f;
					break;
				}

				targetShield.OnHit (transform.position, hitPower, hitAlpha);
				OnHitTarget ();
			}

			if (firing && launchedTime >= reachTime) {
				transform.position = endPoint;
				OnHitTarget ();
			}

		}
	}

	public void OnHitTarget () {
		if (isShowingFirstBlood) {
			SimWorldManager.Instance.currentWorld.activeCamera.FollowMissileEnd ();
		}

		impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal), transform.parent) as GameObject;
		// Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

		foreach (GameObject trail in trailParticles) {
			trail.transform.parent = transform.parent;
			Destroy(trail, 3f);
		}

		Destroy (m_projectileParticle, 3f);
		Destroy (impactParticle, 5f);
		Destroy (gameObject);

		firing = false;

		// update damages
		if (dest.team != GameConfigs.NPC_puzzle && dest.team != GameConfigs.NPC_Arbiter) {
			targetUnit.UpdateDamages (dest);
		}

		targetUnit.UpdateScore (dest.score.dest_addscore);
	}

}
