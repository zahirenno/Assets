using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchyCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.camera = this.GetComponent<Camera> ();
		originalProjection = this.camera.projectionMatrix;
		Debug.Log (originalProjection);
		disableCameraTurn ();
	}

	public float rotationSensitivity = 0.5f;

	private MovableObject lastObject = null;
	private RaycastHit hit;
	private Camera camera;

	private enum STATE {READY, GET_HOLD, READY_MOVE, phase3};
	private STATE state = STATE.READY;


	private enum TOUCH_STATE {STANDBY, MODE_CAMERA, MODE_OBJECT, ON_OBJECT, OBJECT_FOCUS};
	private TOUCH_STATE touchState = TOUCH_STATE.STANDBY;

	public class OpenTouch {
		public static float sensitivity = 1.0f;

		public Vector2 position;
		public TouchPhase phase;
		public float deltaTime;
		public Vector2 deltaPosition;
		public int fingerId;
		public float timeOfCapture;
		public float tapCount;

		public bool is_stationary(OpenTouch other){
			return fingerId == other.fingerId && 
				(phase.Equals(other.phase) || phase.Equals(TouchPhase.Stationary)) && 
					(position - other.position).magnitude <= sensitivity;
		}

		public bool is_stationary_ignore_state(OpenTouch other){
			return fingerId == other.fingerId &&
					(position - other.position).magnitude <= sensitivity;
		}

		public OpenTouch (Touch touch){
			position = touch.position;
			phase = touch.phase;
			deltaTime = touch.deltaTime;
			deltaPosition = touch.deltaPosition;
			fingerId = touch.fingerId;
			tapCount = touch.tapCount;
		}

		public OpenTouch() {}
	}

	public float tapDelay = 0.1f;
	public float longPressDelay = 0.2f;

	private OpenTouch lastTouch = null;
	private OpenTouch lastTouchUp = null;
	void createTouchEvent(){
		if (false && Application.platform == RuntimePlatform.Android) {
			if (Input.touchCount <= 0) 
				return;
			OpenTouch touch = new OpenTouch(Input.GetTouch (0));
		
			switch (touch.phase) {
			case TouchPhase.Began:
				this.onStart (touch);
				break;
			case TouchPhase.Stationary:
				if (touch.deltaTime > longPressDelay) {
					this.onLongPress (touch);
				}
				break;
			case TouchPhase.Moved:
				this.onMove (touch);
				break;
			case TouchPhase.Ended:
				this.onEnd (touch);
				break;
			case TouchPhase.Canceled:
				this.onEnd (touch);
				break;
			}
		} else {
			if (Input.GetMouseButtonUp(0)){

				OpenTouch touch = new OpenTouch();
				touch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				touch.fingerId = 1;
				touch.timeOfCapture = Time.time;
				touch.phase = TouchPhase.Ended;
				touch.deltaTime = 0;
				touch.deltaPosition = new Vector2(0,0);
				touch.tapCount = 1;
				if (lastTouch != null){
					touch.deltaPosition = touch.position - lastTouch.position;
					touch.deltaTime = 0;
					if (!touch.is_stationary(lastTouch)){
						touch.deltaTime = lastTouch.deltaTime + touch.timeOfCapture - lastTouch.timeOfCapture;
					}
				}

				if (lastTouchUp != null){
					float delay = touch.timeOfCapture - lastTouchUp.timeOfCapture;
					if (delay < tapDelay && delay > 0.03f){
						touch.tapCount = lastTouchUp.tapCount + 1;
					}
				}

				lastTouchUp = touch;
				lastTouch = touch;

				if (touch.tapCount == 2)
					this.onDoubleTap(touch);
				this.onEnd(touch);
			}else if (Input.GetMouseButtonDown(0)){

				OpenTouch touch = new OpenTouch();
				touch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				touch.fingerId = 1;
				touch.timeOfCapture = Time.time;
				touch.phase = TouchPhase.Began;
				touch.deltaTime = 0;
				touch.deltaPosition = new Vector2(0,0);

				lastTouch = touch;

				this.onStart(touch);
			}else if (Input.GetMouseButton(0)){

				OpenTouch touch = new OpenTouch();
				touch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				touch.fingerId = 1;
				touch.timeOfCapture = Time.time;
				touch.phase = TouchPhase.Moved;
				touch.deltaTime = 0;
				touch.deltaPosition = new Vector2(0,0);
				if (lastTouch != null){
					touch.deltaPosition = touch.position - lastTouch.position;
					touch.deltaTime = 0;
					if (touch.is_stationary_ignore_state(lastTouch)){
						touch.deltaTime = lastTouch.deltaTime + touch.timeOfCapture - lastTouch.timeOfCapture;
						touch.phase = TouchPhase.Stationary;
					}
				}
				lastTouch = touch;

				if (touch.deltaTime > longPressDelay){
					this.onLongPress(touch);
				}
				else if (!touch.phase.Equals(TouchPhase.Stationary)){
					this.onMove(touch);
				}
			}
		}
	}

	void onSingleTap(OpenTouch touch){
		Debug.Log ("singlne tap");
	}

	void onStart(OpenTouch touch){

		switch (touchState) {
		case TOUCH_STATE.OBJECT_FOCUS:
			break;
		default:
			RayInfo rinfo = new RayInfo ();
			if (rinfo.raycast (this.camera, touch)) {
				touchState = TOUCH_STATE.ON_OBJECT;
			} else {
				touchState = TOUCH_STATE.STANDBY;
			}
			break;
		}
	}

	void onDoubleTap(OpenTouch touch){
		Debug.Log ("double tap");

		Debug.Log (touchState);

		switch (touchState) {
		case TOUCH_STATE.ON_OBJECT:
		case TOUCH_STATE.STANDBY:
			RayInfo rinfo = new RayInfo ();
			if (rinfo.raycast (this.camera, touch)) {
				MovableObject mo = rinfo.hit.transform.gameObject.GetComponent<MovableObject>();
				if (mo != null && !mo.GetType().Equals(typeof(MovableWall))){
					touchState = TOUCH_STATE.OBJECT_FOCUS;
					lastObject = rinfo.hit.transform.GetComponent<MovableObject>();
					this.focusOnObject(rinfo.hit.transform);
				}
			} 
			break;

		case TOUCH_STATE.OBJECT_FOCUS:
			this.unfocus();
			break;
		}


	}

	void onLongPress(OpenTouch touch){

		switch (touchState) {
		case TOUCH_STATE.ON_OBJECT:
			RayInfo rinfo = new RayInfo();
			if (rinfo.raycast (this.camera, touch)){
				touchState = TOUCH_STATE.MODE_OBJECT;
				lastObject = rinfo.hit.transform.gameObject.GetComponent<MovableObject> ();
				lastObject.GetComponent<Highlightable>().highlighted = true;
			}

			if (lastObject == null){
				touchState = TOUCH_STATE.MODE_CAMERA;
				enableCameraTurn();
			}
			break;
		case TOUCH_STATE.STANDBY:
			touchState = TOUCH_STATE.MODE_CAMERA;
			enableCameraTurn();
			break;
		default:
			break;
		}
	}


	void onMove(OpenTouch touch){

		switch (touchState) {
		case TOUCH_STATE.MODE_OBJECT:
			RayInfo rinfo = new RayInfo();
			if (lastObject != null){
				if (rinfo.raycast (this.camera, touch)) {
					lastObject.move (rinfo.ray, rinfo.hit);
				}else{
					/*lastObject.endMove ();
					lastObject.GetComponent<Highlightable>().highlighted = false;
					touchState = TOUCH_STATE.STANDBY;*/
				}
			}
			break;
		case TOUCH_STATE.MODE_CAMERA:

			break;
		case TOUCH_STATE.ON_OBJECT:
			touchState = TOUCH_STATE.MODE_CAMERA;
			enableCameraTurn();
			break;
		case TOUCH_STATE.STANDBY:
			touchState = TOUCH_STATE.MODE_CAMERA;
			enableCameraTurn();
			break;
		case TOUCH_STATE.OBJECT_FOCUS:
			if (lastObject != null){
				lastObject.transform.Rotate(0, -touch.deltaPosition.x * rotationSensitivity, 0, Space.World);
				GameObject.Find("MessageBox").GetComponent<Text>().text = lastObject.transform.eulerAngles.y.ToString("0") + "\u00B0";
			}
			break;
		default:
			break;
		}
	}

	void onEnd(OpenTouch touch){

		switch (touchState) {
		case TOUCH_STATE.MODE_OBJECT:
			if (lastObject != null){
				lastObject.endMove ();
				lastObject.GetComponent<Highlightable>().highlighted = false;
			}
			lastObject = null;

			touchState = TOUCH_STATE.STANDBY;
			disableCameraTurn ();

			break;
		case TOUCH_STATE.OBJECT_FOCUS:
			break;
		default:
			touchState = TOUCH_STATE.STANDBY;
			disableCameraTurn ();
			break;
		}


	}

	void disableCameraTurn(){
		this.camera.GetComponent<TurnTableCamera> ().acceptInput = false;
	}
	void enableCameraTurn(){
		this.camera.GetComponent<TurnTableCamera> ().acceptInput = true;
	}

	public void onZoomRevertEnd(){
		this.camera.GetComponent<ZoomCamera> ().enabled = false;
		this.camera.GetComponent<TurnTableCamera> ().enabled = true;
		touchState = TOUCH_STATE.STANDBY;
	}

	private Transform zoomTarget;
	public float animationTime = 0.3f;

	void focusOnObject(Transform target){
		zoomTarget = target;
		this.camera.GetComponent<TurnTableCamera> ().enabled = false;

		this.camera.GetComponent<ZoomCamera> ().enabled = true;
		this.camera.GetComponent<ZoomCamera> ().focusOnObject (zoomTarget);
	}

	void unfocus(){
		lastObject = null;
		this.camera.GetComponent<ZoomCamera> ().revert ();
	}

	public class RayInfo{
		public RaycastHit hit;
		public Ray ray;

		public bool raycast(Camera camera, OpenTouch touch){
			ray = camera.ScreenPointToRay (touch.position);
			return Physics.Raycast (ray, out hit);
		}
	}


	void FixedUpdate(){
		this.createTouchEvent ();
	}


	private MovableObject prevFocused = null;
	public void selectFurnitureFromCatalog(string id){
		switch (touchState) {
		case TOUCH_STATE.OBJECT_FOCUS:

			if (prevFocused == null)
				prevFocused = lastObject;
			
			lastObject.enabled = false;
			
			Catalog cat = GameObject.Find ("Catalog Provider").GetComponent<Catalog> ();
			MovableObject furniture = cat.createFurniture (id, prevFocused.transform.position, prevFocused.transform.eulerAngles.y).GetComponent<MovableObject>();
			if (furniture == null) {
				return;
			}
			
			lastObject = furniture;

			break;
		}

	}

	public void cancelFurnitureFromCatalog(){
		switch (touchState){
		case TOUCH_STATE.OBJECT_FOCUS:
			if (prevFocused != null) {
				GameObject.Destroy (lastObject.gameObject);
				lastObject = prevFocused;
				prevFocused = null;
			}
			break;
		}
	}

	Matrix4x4 originalProjection;

	// Update is called once per frame
	void Update () {
		/*Matrix4x4 p = originalProjection;
		p.m00 += Mathf.Sin(Time.time * 1.2F) * 0.1F;
		p.m11 += Mathf.Sin(Time.time * 1.5F) * 0.1F;
		camera.projectionMatrix = p;*/
	}
	
}
