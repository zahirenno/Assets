using UnityEngine;
using System.Collections;

public class TurnTableCamera : MonoBehaviour {
	public bool acceptInput = true;

	public Vector3 center = new Vector3(0,0,0);
	public float ray = 4.0f;
	public float friction = 0f;
	public float sensitivity = 360.0f;
	public float height = 1.6f;
	private float angular_speed = 0;

	private float angle = 0;


	// Use this for initialization
	void Start () {
	
	}

	private enum STATE {READY, MOVING, READY_MOVE, phase3};
	private STATE state = STATE.READY;

	private Vector2 delta = new Vector2(0,0);
	private bool firm = false;
	public void setDelta(Vector2 delta){
		this.delta += delta;
		firm = true;
	}

	// Update is called once per frame
	void Update () {
		angular_speed -= angular_speed * friction * Time.deltaTime;

		if (acceptInput) {

			if (firm){
				angular_speed = -delta.x * sensitivity;
				this.transform.Translate (0, delta.y / 10.0f, 0);

				delta = new Vector2(0,0);
				firm = false;
			}

			/*if (true || Application.platform != RuntimePlatform.Android) {
				if (Input.GetMouseButton (0)) {
					angular_speed = -Input.GetAxis ("Mouse X") * sensitivity;
					this.transform.Translate (0, Input.GetAxis ("Mouse Y") / 10.0f, 0);
				}
			} else {
				if (Input.touchCount == 1) {
					Touch touch = Input.GetTouch (0);

					switch (touch.phase) {
					case TouchPhase.Began:
						state = STATE.READY_MOVE;
						break;
					case TouchPhase.Stationary:
						if (state != STATE.MOVING)
							state = STATE.READY;
						break;
					case TouchPhase.Moved:
						if (state == STATE.READY_MOVE) {
							state = STATE.MOVING;
						}
						if (state == STATE.MOVING) {
							angular_speed = -Input.GetAxis ("Mouse X") * sensitivity;
							this.transform.Translate (0, Input.GetAxis ("Mouse Y") / 10.0f, 0);
						}
						break;
					case TouchPhase.Ended:
						state = STATE.READY;
						break;
					}

				}
			}*/

		}
		/*this.transform.RotateAround (-this.transform.position,
		                             new Vector3 (0, 1, 0),
		                             angular_speed * Time.deltaTime);*/

		float angularOffset = angular_speed * Time.deltaTime;
		float angularOffsetR = angularOffset * 3.14f / 180.0f;
		angle += angularOffsetR;

		this.transform.position = new Vector3(center.x + ray * Mathf.Cos(angle),
		                                      this.transform.position.y,
		                                      center.z + ray * Mathf.Sin(angle));

		this.transform.LookAt(new Vector3(0,height,0));
	}
}
