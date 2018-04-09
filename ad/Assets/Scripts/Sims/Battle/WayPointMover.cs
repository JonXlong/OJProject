using UnityEngine;
using System.Collections;

public class WayPointMover : MissileBase {

	private int index = 0;
	private Vector3[] wayPoints;
	private Vector3 nextPosition;

    private GameObject boxGameObject;
    private TrailRenderer trailRenderer;
    private float timeFromPuzzleDBToArbiter = 1.0f;

	// Use this for initialization
	void Awake () {
		trailRenderer = GetComponentInChildren<TrailRenderer> ();
	}

	public void Launch(SimUnitBase _fromUnit, SimUnitBase _targetUnit, SimBehaviorDest _dest, Vector3[] wp, Color color, bool _isPuzzle) {
        isPuzzle = _isPuzzle;

        speed = GameConfigs.Instance.real_missile_speed;
        
		float totalDistance = 0f;
		for (int i = 0; i < wp.Length - 1; i++) {
			totalDistance += Vector3.Distance(wp[i], wp[i+1]);
		}

		reachTime = totalDistance / speed;

		fromUnit = _fromUnit;
		targetUnit = _targetUnit;
		dest = _dest;

		wayPoints = wp;

		index = 1;

		Transform tail = transform.FindChild ("Tail");
		if (tail != null) {
			if (tail.GetComponent<Renderer>() != null) {
				tail.GetComponent<Renderer>().material.SetColor ("_TintColor", color);
			}
		}

		Transform head = transform.FindChild ("Head");
		if (head != null) {

			if (color == Color.green) {
				boxGameObject = head.FindChild ("BoxGreen").gameObject;
			} else if (color == Color.red) {
				boxGameObject = head.FindChild ("BoxRed").gameObject;
			}

			if (boxGameObject != null) {
				boxGameObject.SetActive (true);
				MeshRenderer renderer = boxGameObject.GetComponent<MeshRenderer> ();
				if (renderer != null) {
					string logoURL = SimWorldManager.Instance.FindTeamLogoById (dest.firstblood_team);
					if (logoURL.Length > 0) {
						StartCoroutine(LoadLogo(logoURL, renderer));
					}
				}
			}
		}
        
		if (wayPoints.Length > 0) {
			nextPosition = wayPoints [1];
		}

        if (isPuzzle && targetUnit != null) {
            transform.position = targetUnit.transform.position;
            reachTime += timeFromPuzzleDBToArbiter;
            StartCoroutine(PuzzleMoveToArbiter());
        } else {
            transform.position = wayPoints[0];
            firing = true;
        }
	}

    IEnumerator PuzzleMoveToArbiter() {
        yield return new WaitForSeconds(timeFromPuzzleDBToArbiter);
        transform.position = wayPoints[0];
        firing = true;
    }

    IEnumerator LoadLogo(string path, MeshRenderer renderer) {
		while (true) {
			Texture2D sourceTexture = UIManager.Instance.GetLogoTexture2D(path);

			if (sourceTexture != null) {
				renderer.material.SetTexture ("_MainTex", sourceTexture);
				break;
			}

			yield return 0;
		}
	}

	// Update is called once per frame
	void Update () {
		if (firing) {

			CheckFirstBlood ();

			transform.position = Vector3.MoveTowards (transform.position, nextPosition, Time.deltaTime * speed);
			if (Vector3.Distance(transform.position, nextPosition) < 0.01f) {
//			if (transform.position.x == nextPosition.x && transform.position.z == nextPosition.z) {
				index++;
				if (index < wayPoints.Length) {
					nextPosition = wayPoints [index];
				} else {
					firing = false;
					OnHitTarget ();
				}
			}

			launchedTime += Time.deltaTime;
		} else if (isPuzzle) {
            transform.position = Vector3.Lerp(transform.position, wayPoints[0], launchedTime);
            launchedTime += Time.deltaTime;
        }
	}

	public void OnHitTarget () {
		if (isShowingFirstBlood) {
			SimWorldManager.Instance.currentWorld.activeCamera.FollowMissileEnd ();
		}

		if (!isPuzzle && targetUnit != null) {
			targetUnit.UpdateDamages (dest);
			targetUnit.UpdateScore (dest.score.dest_addscore);
		}

		if (trailRenderer != null) {
			Destroy (gameObject, trailRenderer.time);
		} else {
			Destroy (gameObject);
		}

	}

}
