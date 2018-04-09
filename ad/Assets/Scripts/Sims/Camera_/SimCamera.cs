using UnityEngine;
using System.Collections;

public class SimCamera : MonoBehaviour {

	[HideInInspector]
	public enum CAMERA_WORLD {
		WorldReal,
		WorldVirtual
	}

	public CAMERA_WORLD type = CAMERA_WORLD.WorldVirtual;

	public const float zoomEarthTime = 2f;

	public const float firstBloodTime = 3f;
	public const float firstBloodOutTime = 1f;
	public const float firstBloodTimeScale = 0.6f;
	public const float firstBloodStayTime = 1f;

	public float zoomToEarth;
	public float zoomToUnit;
	public GameObject earth;
	public EarthTransportScript transportAnimation;

	public float FB_distanceUp = 1f;
	public float FB_distanceAway = 10f;

	private PositionAndRotationController prController;
	private FreeLookCamera freeLookCamera;
	private Camera cam;

	private Transform followObject;

	private Animator trasportAnim;

	private float pausedZoom;
	private Vector3 pausedAngles = new Vector3();
	private Vector3 pausedPosition = new Vector3();

	void Awake () {
		freeLookCamera = GetComponent<FreeLookCamera> ();
		prController = GetComponent<PositionAndRotationController> ();
		cam = GetComponent<Camera> ();
		pausedZoom = cam.fieldOfView;
	}

	void Start () {
	}

	public void ResetCameraStatus () {
		prController.AnimateToOringialStatus ();
	}

	public void ResetStatus () {
		prController.RestStatus ();
	}

	private void stackCameraData() {
		pausedZoom = cam.fieldOfView;
		pausedAngles.Set (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
		pausedPosition.Set (transform.position.x, transform.position.y, transform.position.z);
	}

	private void restoreCameraData() {
		cam.fieldOfView = pausedZoom;
		transform.localRotation = Quaternion.Euler (pausedAngles);
		transform.position = pausedPosition;
	}

	public void TransportToEarth () {
		stackCameraData ();

		if (earth != null) {
			earth.gameObject.SetActive (true);
		}

		iTween.LookTo (gameObject, earth.transform.Find("LookAtPoint").position, zoomEarthTime);

		Hashtable zoomArgs = iTween.Hash("easeType", iTween.EaseType.linear, "from", cam.fieldOfView, "to", zoomToEarth, "time", 1f, "onupdate", "OnZoomInEarthUpdate", "oncomplete", "OnZoomInEarthEnd", "delay", zoomEarthTime);
		iTween.ValueTo (gameObject, zoomArgs);
	}

	public void TransportToSpace () {
		stackCameraData ();
		transportAnimation.Play (false, ToSpaceAnimationComplete);
	}

	private void OnZoomInEarthUpdate(float zoomVal) {
		cam.transform.Rotate (Vector3.forward, 2f);
		cam.fieldOfView = zoomVal;
//		cam.transform.localRotation = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, cam.transform.localRotation.eulerAngles.y, cam.transform.localRotation.eulerAngles.z + 10f);
	}

	private void UpdateZoom(float zoomVal) {
		cam.fieldOfView = zoomVal;
	}

	public void OnZoomInEarthEnd() {
	//	gameObject.SetActive (false);
        //if (GameManager.Instance.isWait==false  )
        //{
        //    restoreCameraData();
        //  //  transportAnimation.Play(true, ToEarthAnimationComplete);
        //}
        //if (GameManager.Instance.isWait )
        //{
        //    transportAnimation.Play(true, ToEarthAnimationComplete);
        //}
        gameObject.SetActive(false);

        restoreCameraData();

        transportAnimation.Play(true, ToEarthAnimationComplete);
    }

	private void ToEarthAnimationComplete() {
		SimWorldManager.Instance.EndZooming ();
		GameManager.Instance.SetGameMode (GameMode.GM_Real);
	}

	private void ToSpaceAnimationComplete() {
		SimWorldManager.Instance.EndZooming ();
		GameManager.Instance.SetGameMode (GameMode.GM_Vritual);
	}

	private Vector3[] path;
    private bool isFirstBloodPuzzle;
    private Transform targetTrans;
	public void FirstBlood (Transform _followObject, string text, bool isPuzzle ,Transform  targetT) {
        isFirstBloodPuzzle = isPuzzle;
        targetTrans = targetT;
        UIManager.Instance.gameUI.HideUIContainer ();

		followObject = _followObject;

		SimWorldManager.Instance.currentWorld.StopRotation ();
		stackCameraData ();

		// Enter bullet time
		Time.timeScale = firstBloodTimeScale;

//		path = new Vector3[2];
//		path [0] = transform.position;
//		path [1] = unit.transform.position + ((transform.position - unit.transform.position).normalized * 10f);
//
//		Hashtable args = new Hashtable ();
//		args.Add("easeType", iTween.EaseType.linear);
//		args.Add ("path", path);
//		args.Add ("looktarget", unit.transform.position);
//		args.Add ("oncomplete", "MoveToUnitEnd");
//		args.Add ("time", firstBloodTime);
//		args.Add ("ignoretimescale", true);
//
//		iTween.MoveTo (gameObject, args);

		UIManager.Instance.gameUI.ShowTitleTxt (text, false);

//		iTween.LookTo (gameObject, unit.transform.position, 1f);
//		Hashtable zoomArgs = iTween.Hash("from", cam.fieldOfView, "to", zoomToUnit, "time", zoomUnitTime * Time.timeScale, "onupdate", "UpdateZoom", "oncomplete", "OnZoomInUnitEnd");
//		iTween.ValueTo (gameObject, zoomArgs);
	}

    public Transform currentFollowObject {
		get {
			return followObject;
		}
	}
    float newDistanceUp = 20f;
	void Update () {
		if (followObject != null) {
			float smooth = 15f;

			if (type == CAMERA_WORLD.WorldReal) {
				smooth = 1f;
			}

			Vector3 disPos = Vector3.zero;
            Vector3 newPos = Vector3.zero;

            if (type == CAMERA_WORLD.WorldVirtual) {
                if (isFirstBloodPuzzle) {
                    float _dis = Vector3.Distance(followObject.position, targetTrans.position);
                    //TODO 对飞船和目标水晶的距离做判断 40为界限  大于后修改Smooth  以及disPos (up right)
                  //  newDistanceUp = _dis > 30 ? 50f : 26f;
                    newDistanceUp = _dis > 50 ? 60f : newDistanceUp = _dis > 30 ? 50f : 26f;
                    //if (_dis > 30&& _dis<50)
                    //{
                    //    newDistanceUp = 50f;
                    //    smooth = 20f;
                    //}
                    //if (_dis>50)
                    //{
                    //    newDistanceUp = 60f;
                    //    smooth = 30f;
                    //}
                    //else {
                    //    newDistanceUp = 26f;
                    //}
                    Debug.Log(_dis);
                  newPos  = new Vector3((followObject.position.x + targetTrans.position.x)/2, (followObject.position.y + targetTrans.position.y)/2, (followObject.position.z + targetTrans.position.z)/2);
                  //  disPos = followObject.position + Vector3.up * FB_distanceUp * 14f + followObject.forward * FB_distanceAway * 4f + followObject.right * FB_distanceAway;
                    disPos = newPos + Vector3.up * newDistanceUp + Vector3.right * newDistanceUp;
                    Debug.Log("show first blood" + disPos);
                 //   transform.LookAt(newPos);
                }
                else {
                    disPos = followObject.position + Vector3.up * FB_distanceUp - followObject.forward * FB_distanceAway;
                }
			} else {
				disPos = followObject.position + Vector3.up * FB_distanceUp;
			}

			transform.position = Vector3.Lerp (transform.position, disPos, Time.deltaTime * smooth);
          transform.LookAt( newPos != Vector3.zero ? newPos :followObject.position);
         //  transform.LookAt (followObject.position);
           
		} else {
			prController.UpdateStatus ();
		}
	}

	public void FollowMissileEnd() {
		// common speed
		Time.timeScale = 1f;
		followObject = null;

		Hashtable movArgs = new Hashtable ();
		movArgs.Add("easeType", iTween.EaseType.linear);
		movArgs.Add ("time", firstBloodOutTime);
		movArgs.Add ("x", pausedPosition.x);
		movArgs.Add ("y", pausedPosition.y);
		movArgs.Add ("z", pausedPosition.z);
		movArgs.Add ("delay", firstBloodStayTime);
		movArgs.Add ("onupdate", "UpdateLookAtZero");
		movArgs.Add ("oncomplete", "OnFirstBloodEnd");
		iTween.MoveTo (gameObject, movArgs);

//		Hashtable rotArgs = new Hashtable ();
//		rotArgs.Add("easeType", iTween.EaseType.linear);
//		rotArgs.Add ("isLocal", true);
//		rotArgs.Add ("time", firstBloodOutTime);
//		rotArgs.Add ("x", pausedAngles.x);
//		rotArgs.Add ("y", pausedAngles.y);
//		rotArgs.Add ("z", pausedAngles.z);
//		rotArgs.Add ("delay", firstBloodStayTime);
//		rotArgs.Add ("oncomplete", "OnFirstBloodEnd");
//		iTween.RotateTo (gameObject, rotArgs);
	}

	private void UpdateLookAtZero() {
		transform.LookAt (Vector3.zero);
	}

	private void OnFirstBloodEnd() {
        Debug.Log("OnFirstBloodEnd");
        UIManager.Instance.gameUI.HideFirstBloodTitle ();
		UIManager.Instance.gameUI.ShowUIContainer ();

		SimWorldManager.Instance.EndZooming ();
		SimWorldManager.Instance.currentWorld.StartRotation ();
	//	restoreCameraData ();
	}

	public bool isFreeLookCameraControlling {
		get {
			return freeLookCamera.isUnderControl;
		}
	}

	public void ResetFreeLookCamera () {
		freeLookCamera.ResetController ();
	}

//	private void OnZoomInUnitEnd() {
//		Hashtable rotArgs = new Hashtable ();
//		rotArgs.Add("easeType", iTween.EaseType.linear);
//		rotArgs.Add ("isLocal", true);
//		rotArgs.Add ("time", zoomUnitTime * Time.timeScale);
//		rotArgs.Add ("x", pausedAngles.x);
//		rotArgs.Add ("y", pausedAngles.y);
//		rotArgs.Add ("z", pausedAngles.z);
//		rotArgs.Add ("delay", 1f * Time.timeScale);
//		iTween.RotateTo (gameObject, rotArgs);
//
//		Hashtable zoomArgs = new Hashtable ();
//		zoomArgs.Add("easeType", iTween.EaseType.linear);
//		zoomArgs.Add("from", cam.fieldOfView);
//		zoomArgs.Add("to", pausedZoom);
//		zoomArgs.Add("time", zoomUnitTime * Time.timeScale);
//		zoomArgs.Add("onupdate", "UpdateZoom");
//		zoomArgs.Add("oncomplete", "OnZoomOutUnitEnd");
//		zoomArgs.Add("delay", 1f * Time.timeScale);
//		iTween.ValueTo (gameObject, zoomArgs);
//	}

}
