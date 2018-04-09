using UnityEngine;
using System.Collections;

public class CountDownGameMode : GameMode {
	
	public int countDownTime = 1;
	public const float cameraMoveTime = 3f;

	public override void OnEnterMod () {
		UIManager.Instance.gameUI.HideUIContainer ();
		RenderSettings.skybox = GameManager.Instance.virutalSkyBox;

		UIManager.Instance.scifiClockUI.gameObject.SetActive (true);
		UIManager.Instance.scifiClockUI.moveTime = cameraMoveTime;

		SimWorldManager.Instance.ActiveVirtualWorld ();

		StartCoroutine (CountDownTimer());
	}

	IEnumerator CountDownTimer() {
		while (countDownTime >= 0) {
			UIManager.Instance.scifiClockUI.UpdateTime (countDownTime);

			if (countDownTime == 0) {
				moveCameraDown ();
				break;
			}

			countDownTime--;
			yield return new WaitForSeconds(1.0f);
		}
	}

	private void moveCameraDown() {
//		Vector3 pos = UIManager.Instance.uiCamera.transform.position;

		Hashtable args = new Hashtable ();
		args.Add("easeType", iTween.EaseType.linear);
		args.Add("space", Space.World);
		args.Add ("time", 1.5f);
		args.Add ("oncomplete", "onCameraMoveDownEnd");
		args.Add ("oncompletetarget", gameObject);
		args.Add ("y", 10f);

		iTween.MoveTo (UIManager.Instance.uiCamera.gameObject, args);
	}

	void onCameraMoveDownEnd() {
		// uicamera move to active camera position
		SimCamera targetCamera = SimWorldManager.Instance.currentWorld.activeCamera;

		Hashtable moveArgs = new Hashtable ();
		moveArgs.Add("easeType", iTween.EaseType.linear);
		moveArgs.Add ("time", 2f);
		moveArgs.Add ("x", targetCamera.transform.position.x);
		moveArgs.Add ("y", targetCamera.transform.position.y);
		moveArgs.Add ("z", targetCamera.transform.position.z);
		iTween.MoveTo (UIManager.Instance.uiCamera.gameObject, moveArgs);

		// uicamera rotation as active camera rotation
		Hashtable rotArgs = new Hashtable ();
		rotArgs.Add("easeType", iTween.EaseType.linear);
		rotArgs.Add ("isLocal", true);
		rotArgs.Add ("time", 2f);
		rotArgs.Add ("oncomplete", "onMoveCameraComplete");
		rotArgs.Add ("oncompletetarget", gameObject);
		rotArgs.Add ("x", targetCamera.transform.localEulerAngles.x);
		rotArgs.Add ("y", targetCamera.transform.localEulerAngles.y);
		rotArgs.Add ("z", targetCamera.transform.localEulerAngles.z);
		iTween.RotateTo (UIManager.Instance.uiCamera.gameObject, rotArgs);
	}

	void onMoveCameraComplete() {
		GameManager.Instance.SetGameMode (GameMode.GM_Vritual);
	}

	public override void OnExitMod () {
		Destroy (this);
	}

}
