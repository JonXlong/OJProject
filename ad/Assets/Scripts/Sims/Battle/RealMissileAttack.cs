using UnityEngine;
using System.Collections;

public class RealMissileAttack : MonoBehaviour {
	[HideInInspector]
	public SimUnitBase fromUnit;
	[HideInInspector]
	public SimUnitBase targetUnit;

	private Vector3 startPoint;
	private Vector3 endPoint;

	private bool launched = false;
	private SimBehaviorDest dest;

	public GameObject head;
	public GameObject tail;

	private LineRenderer lineRenderer;      // Line rendered component

	void Awake() {
		// Get line renderer component
		lineRenderer = GetComponent<LineRenderer>();
	}

	// Use this for initialization
	void Start () {
	
	}

	public void Launch (SimUnitBase _fromUnit, SimUnitBase _targetUnit, SimBehaviorDest _dest) {
		dest = _dest;
		fromUnit = _fromUnit;
		targetUnit = _targetUnit;

		startPoint = fromUnit.gunPos;
		endPoint = targetUnit.transform.position + new Vector3 (0, 0.5f, 0);

		this.transform.position = new Vector3(startPoint.x, 0.6f, startPoint.z);

		float speed = GameConfigs.Instance.real_missile_speed;
		float distance = Vector3.Distance(startPoint, endPoint);
		float reachTime = distance / speed;

		Hashtable args = new Hashtable ();
		args.Add("easeType", iTween.EaseType.linear);
//		args.Add("space", Space.World);
		args.Add ("time", reachTime);
		args.Add ("oncomplete", "OnHitTarget");
//		args.Add ("oncompletetarget", gameObject);
		args.Add ("x", endPoint.x);
		args.Add ("y", 0.6f);
		args.Add ("z", endPoint.z);

		iTween.MoveTo (gameObject, args);

		// Set beam scaling according to its length
		lineRenderer.material.SetTextureScale("_MainTex", new Vector2 (1f, 1f));

		launched = true;
	}

	public void SetColor(Color c) {
		if (tail != null) {
			Renderer renderer = tail.GetComponent<Renderer> ();
			if (renderer != null) {
				renderer.material.SetColor ("_TintColor", c);
			}

			renderer = GetComponent<Renderer> ();
			if (renderer != null) {
				renderer.material.SetColor ("_TintColor", c);
			}
		}
	}

	// refresh predict line
	void Update () {
		if (launched) {
			DrwaPredictLine ();
		}
	}

	private void DrwaPredictLine () {
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, endPoint);
		lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(-Time.time, 0f));
	}

	private void OnHitTarget() {
		launched = false;

		targetUnit.UpdateDamages (dest);
		targetUnit.UpdateScore (dest.score.dest_addscore);

		if (head != null) {
			Destroy (head);
		}

		Destroy (gameObject, 1f);
	}
}
