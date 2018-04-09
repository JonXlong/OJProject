using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreHUD : MonoBehaviour {
	public Text text;
	public RectTransform rect;

	[HideInInspector]
	public Vector3 OffsetV3 = Vector3.zero;
	[HideInInspector]
	public GameObject fromObject;

	void Start() {
	}

	void Update() {
		SimCamera cam = SimWorldManager.Instance.currentWorld.activeCamera;

		if (cam != null && fromObject != null) {
			Vector3 position = fromObject.transform.position + OffsetV3;
			this.transform.position = position;
			rect.LookAt (cam.transform, Vector3.up);
		}
	}

	public void PlayFloating (string t, Color color, float time) {
		text.text = t;
		text.color = color;

		Hashtable moveArgs = new Hashtable ();
		moveArgs.Add ("easeType", iTween.EaseType.linear);
		moveArgs.Add ("time", time);
		moveArgs.Add ("islocal", true);
		moveArgs.Add ("x", text.transform.localPosition.x);
		moveArgs.Add ("y", text.transform.localPosition.x + 3f);
		moveArgs.Add ("z", text.transform.localPosition.z);
		moveArgs.Add ("oncomplete", "AnimationComplete");
		moveArgs.Add ("oncompletetarget", gameObject);
		iTween.MoveTo (text.gameObject, moveArgs);

//		iTween.ColorTo(text.gameObject, new Color(color.r, color.g, color.b, 0f), time);
	}

	void AnimationComplete() {
		Destroy (gameObject);
	}

}