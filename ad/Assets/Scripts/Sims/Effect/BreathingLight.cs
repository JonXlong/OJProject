using UnityEngine;
using System.Collections;

public class BreathingLight : MonoBehaviour {

	private Light lightComp;

	public float startValue;
	public float endValue;
	public float speed = 0.5f;

	private float curValue;
	private float dir = 1f;

	// Use this for initialization
	void Start () {
		lightComp = GetComponent<Light>();

		curValue = Random.Range (startValue, endValue);

//		Hashtable args = new Hashtable ();
//		args.Add("easeType", iTween.EaseType.linear);
//		args.Add("loopType", "pingPong");
//		args.Add("from", startValue);
//		args.Add("to", endValue);
//		args.Add("speed", 0.5f);
//		args.Add("onupdate", "UpdateIntensity");
//		iTween.ValueTo (gameObject, args);
	}

	// Update is called once per frame
	void Update () {
		curValue += dir * speed * Time.deltaTime;

		if (curValue >= endValue) {
			curValue = endValue;
			dir *= -1f;
		} else if (curValue <= startValue) {
			curValue = startValue;
			dir *= -1f;
		}

		lightComp.intensity = curValue;
	}
}
