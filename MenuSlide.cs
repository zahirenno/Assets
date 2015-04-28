using UnityEngine;
using System.Collections;

public class MenuSlide : MonoBehaviour {
	private RectTransform rectTransform;
	private bool prevVisible = true;
	private enum STATE {hiding, showing, resting};
	private STATE state = STATE.resting;
	private float speed = 0;
	private float width = 0;

	public bool visible = false;
	public int animationTime = 200;

	// Use this for initialization
	void Start () {
		rectTransform = (RectTransform)this.transform;

		Vector3[] g = new Vector3[4];
		rectTransform.GetWorldCorners(g);
		float leftx = g[0].x;
		float rightx = g[2].x;
		width = Mathf.Abs (leftx - rightx);
	}

	private void updateResting(){
		if (visible == prevVisible)
			return;

		prevVisible = visible;
		if (!visible)
			state = STATE.hiding;
		else
			state = STATE.showing;
	}

	private void updateHiding(){
		rectTransform.Translate (new Vector3 (-speed * Time.deltaTime * 1000.0f, 0, 0));
		if (rectTransform.position.x < -width / 2.0f) {
			state = STATE.resting;
			rectTransform.position = new Vector3(-width / 2.0f, rectTransform.position.y, rectTransform.position.z);
		}
	}

	private void updateShowing(){
		rectTransform.Translate (new Vector3 (speed * Time.deltaTime * 1000.0f, 0, 0));
		if (rectTransform.position.x > width / 2.0f) {
			state = STATE.resting;
			rectTransform.position = new Vector3(width / 2.0f, rectTransform.position.y, rectTransform.position.z);
		}
	}

	// Update is called once per frame
	void Update () {
		speed = width / (float)animationTime;

		switch (state) {
		case STATE.hiding:
			updateHiding();
			break;
		case STATE.resting:
			updateResting();
			break;
		case STATE.showing:
			updateShowing();
			break;
		}
	}
}
