using UnityEngine;
using System.Collections;

public class RealLinkDot : MonoBehaviour {
	public RealLinkDot prev = null;

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, 0.5f);
	}

}
