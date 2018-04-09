
using UnityEngine;
using System.Collections;

public class VritualGatherProjectile : MonoBehaviour {
	
	public float beamScale;         // Default beam scale to be kept over distance
	public bool AnimateUV;          // UV Animation
	public float UVTime = 1f;            // UV Animation speed

	public Transform rayImpact;     // Impact transform
	public Transform rayMuzzle;     // Muzzle flash transform

    private Transform targetTrans;

    private LineRenderer lineRenderer;      // Line rendered component
	//private float beamLength;               // Current beam length
	private float initialBeamOffset;        // Initial UV offset 

	private Vector3 startPoint;
	private Vector3 endPoint;

	private Color defaultColor;

	void Awake() {
		// Get line renderer component
		lineRenderer = GetComponent<LineRenderer>();
		defaultColor = lineRenderer.material.GetColor ("_TintColor");

		// Randomize uv offset
		initialBeamOffset = Random.Range(0f, 5f);
	}

	public void Play(Transform _targetTrans) {
        targetTrans = _targetTrans;
        Raycast ();
	}

	// Hit point calculation
	public void Raycast() {

       // beamLength = Vector3.Distance(transform.position, targetTrans.position);
        startPoint = transform.position;
        endPoint = targetTrans.position;

        lineRenderer.SetPosition(0, startPoint);
		lineRenderer.SetPosition(1, endPoint);

		// Adjust impact effect position
		if (rayImpact) {
			rayImpact.position = endPoint;
		}

		// Adjust muzzle position
		if (rayMuzzle) {
			rayMuzzle.position = transform.position;
		}

		// Set beam scaling according to its length
		lineRenderer.material.SetTextureScale("_MainTex", new Vector2 (beamScale, 1f));
	}

    private float blendEffectValue = 0.6f;
    public void Gathering() {
        // white
        Color c = new Color(blendEffectValue, blendEffectValue, blendEffectValue, 1f);
        lineRenderer.material.SetColor("_TintColor", c);
    }

    public void GatherSuccess() {
		// green
		Color c = new Color (0f, blendEffectValue, 0f, 1f);
		lineRenderer.material.SetColor ("_TintColor", c);
	}

	public void GatherFailed() {
		// red
		Color c = new Color (blendEffectValue, 0f, 0f, 1f);
		lineRenderer.material.SetColor ("_TintColor", c);
	}

	public void RemoveBeam () {
		lineRenderer.material.SetColor ("_TintColor", defaultColor);
		Destroy (gameObject);
	}

	void Update() {
		// Animate texture UV
		if (AnimateUV) {
			lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * UVTime + initialBeamOffset, 0f));
		}

		Raycast ();
	}
}
