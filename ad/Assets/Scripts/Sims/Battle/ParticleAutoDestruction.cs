using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(ParticleSystem))]
public class ParticleAutoDestruction : MonoBehaviour {

	private ParticleSystem[] particleSystems;  

	void Start() {
		particleSystems = GetComponentsInChildren<ParticleSystem>();
	}

	void Update () {
		bool allStopped = true;

		foreach (ParticleSystem ps in particleSystems) {
			if (!ps.isStopped) {
				allStopped = false;
			}
		}

		if (allStopped) {
			GameObject.Destroy(gameObject);
		}
	}

}