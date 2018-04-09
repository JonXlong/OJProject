using UnityEngine;
using UnityEngine.EventSystems;
//using System.Collections;

public class FreeLookCamera : MonoBehaviour {

	public float zoomLimitMin = -50f;
	public float zoomLimitMax = 50f;
	private float currentZoom = 0f;

	private float m_RotationSped = 80f;
	private float m_ScrollSpeed = 200f;

	private KeyCode mouseRotationKey = KeyCode.Mouse0;
	private KeyCode rotateRightKey = KeyCode.D;
	private KeyCode rotateLeftKey = KeyCode.A;
	private KeyCode rotateUpKey = KeyCode.E;
	private KeyCode rotateDownKey = KeyCode.Q;
	private KeyCode resetKey = KeyCode.R;

	private KeyCode zoomInKey = KeyCode.W;
	private KeyCode zoomOutKey = KeyCode.S;

	private float lastOpertationTime = -1f;
	private float recoverOprationTime = 20f;
	private Vector3 lastMousePosition;
	private bool isMouseDown = false;

	void Awake () {
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    //
	void Update () {
		if (!SimWorldManager.Instance.isZooming) {
			
			if (!EventSystem.current.IsPointerOverGameObject () && Input.GetKeyDown(mouseRotationKey)) {
				isMouseDown = true;
               
              
            } else if (Input.GetKeyUp(mouseRotationKey)) {
				isMouseDown = false;
                //Debug.Log("Mouse Up");
			}
       

            Rotation ();
			Zoom ();

			if (Input.GetKey (resetKey)) {
				ResetController ();
			}
		}

		lastMousePosition = Input.mousePosition;
	}

	float zoomStep = 0f;
	private void Zoom() {
		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			zoomStep = Input.GetAxis("Mouse ScrollWheel") * m_ScrollSpeed * 0.5f;

			if (currentZoom + zoomStep > zoomLimitMax) {
				zoomStep = zoomLimitMax - currentZoom;
			} else if (currentZoom + zoomStep < zoomLimitMin) {
				zoomStep = zoomLimitMin - currentZoom;
			}

			currentZoom += zoomStep;
			lastOpertationTime = Time.time;

			transform.Translate(Vector3.forward * zoomStep, Space.Self);
		}

		if (ZoomDirection != 0) {
			zoomStep = ZoomDirection * Time.deltaTime * m_ScrollSpeed * 0.1f;

			if (currentZoom + zoomStep > zoomLimitMax) {
				zoomStep = zoomLimitMax - currentZoom;
			} else if (currentZoom + zoomStep < zoomLimitMin) {
				zoomStep = zoomLimitMin - currentZoom;
			}

			currentZoom += zoomStep;
			lastOpertationTime = Time.time;

			transform.Translate(Vector3.forward * zoomStep, Space.Self);
		}
	}
    private bool isStopCamera = false  ;
	private void Rotation() {

		if (isMouseDown && Input.GetKey(mouseRotationKey)) {
			if (Vector3.Distance(lastMousePosition, Input.mousePosition) > 0.1) {
				lastOpertationTime = Time.time;
                //Add stopMoveCamerButton
               
                    GameManager.Instance.StopMoveCamera(true);
                    Debug.logger.Log("Stop Camera");
               
            }
          
            transform.Rotate(Vector3.down, Input.GetAxis("Mouse X") * m_RotationSped * Time.deltaTime, Space.World);
			transform.Rotate(Vector3.left, -Input.GetAxis("Mouse Y") * m_RotationSped * Time.deltaTime, Space.Self);
		}

		if (RotationYDirection != 0) { 
			lastOpertationTime = Time.time;
			transform.Rotate(Vector3.up, RotationYDirection * m_RotationSped * Time.deltaTime, Space.World);
		}

		if (RotationXDirection != 0) {
			lastOpertationTime = Time.time;
			transform.Rotate(Vector3.right, RotationXDirection * m_RotationSped * Time.deltaTime, Space.Self);
		}

	}

	private int ZoomDirection {
		get {
			bool zoomIn = Input.GetKey(zoomInKey);
			bool zoomOut = Input.GetKey(zoomOutKey);
			if (zoomIn && zoomOut)
				return 0;
			else if(zoomIn && !zoomOut)
				return 1;
			else if(!zoomIn && zoomOut)
				return -1;
			else 
				return 0;
		}
	}

	private int RotationYDirection {
		get {
			bool rotateRight = Input.GetKey(rotateRightKey);
			bool rotateLeft = Input.GetKey(rotateLeftKey);
			if(rotateLeft && rotateRight)
				return 0;
			else if(rotateLeft && !rotateRight)
				return -1;
			else if(!rotateLeft && rotateRight)
				return 1;
			else 
				return 0;
		}
	}

	private int RotationXDirection {
		get {
			bool rotateUp = Input.GetKey (rotateUpKey);
			bool rotateDown = Input.GetKey (rotateDownKey);
			if (rotateUp && rotateDown)
				return 0;
			else if (rotateUp && !rotateDown)
				return -1;
			else if (!rotateUp && rotateDown)
				return 1;
			else
				return 0;
		}
	}

	public void ResetController () {
		currentZoom = 0f;
		lastOpertationTime = -1f;

		PositionAndRotationController controller = GetComponent<PositionAndRotationController> ();
		if (controller != null) {
			controller.RestStatus ();
		}
	}

	public bool isUnderControl {
		get {
			bool result = false;
			if ((lastOpertationTime > 0) && ((Time.time - lastOpertationTime) <= recoverOprationTime)) {
				result = true;
			}
			return result;
		}
	}

}
