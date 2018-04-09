using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScifiClockUI : MonoBehaviour {
	public bool enableMouseInteract = false;
	public LoadProgressImages progress;
	public Text text;
	[HideInInspector]
	public float moveTime = 2f;

	private Vector3 _rotation = new Vector3(8f, -8f, 0);
	private float _prevX;
	private float _prevY;

	protected void Update() {
		if (enableMouseInteract) {
			if (Mathf.Approximately(Input.mousePosition.x, _prevX) == false || Mathf.Approximately(Input.mousePosition.y, _prevY) == false) {

				var normalized = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
				normalized.x -= 0.5f;
				normalized.x *= 2f;

				normalized.y -= 0.5f;
				normalized.y *= 2f;

				var rot = _rotation;
				rot.x *= normalized.y;
				rot.y *= normalized.x;

				transform.rotation = Quaternion.Euler(rot);
			}

			_prevX = Input.mousePosition.x;
			_prevY = Input.mousePosition.y;
		}
	}

	public void UpdateTime(int cdTime) {
		if (cdTime <= 0) {
			MoveUpClock ();
			cdTime = 0;
		}
		progress.OnProgress (cdTime);
		text.text = cdTime.ToString();
	}

	private void MoveUpClock() {

		Hashtable args = new Hashtable ();
		args.Add("easeType", iTween.EaseType.linear);
//		args.Add ("speed", 50f);
		args.Add ("time", moveTime);
		args.Add ("oncomplete", "AnimationEnd");
		args.Add ("oncompletetarget", gameObject);
		args.Add ("y", 1200f);

		iTween.MoveTo (gameObject, args);
	}

	void AnimationEnd() {
		gameObject.SetActive (false);
	}

}