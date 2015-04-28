using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.UI;

public class MoveWallState : State {

	// Use this for initialization
	void Start () {
		
	}
	
	private Transform focusedObject;
	
	public override void Enter(){
		focusedObject = listener.getLastGesture (this).hit.transform;
		focusedObject.gameObject.GetComponent<Highlightable> ().highlighted = true;

		base.Enter ();
	}

	protected override void UpdateStarting(){
			start();
	}
	
	protected override void UpdateRunning(){
		if (Input.GetMouseButton (0)) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit))
					focusedObject.gameObject.GetComponent<MovableWall> ().move (ray, hit);

			EditableRoom room = this.gameObject.GetComponentInChildren<EditableRoom>();
			Text t = GameObject.Find("MessageBox").GetComponent<Text>();
			t.text = (room.width * 100.0f).ToString("0") + "X" + (room.height * 100.0f).ToString("0");

		}
		
	}
	
	protected override void onReleaseRunning(ReleaseGesture gesture){
		nextState = null;
		shouldPop = true;

		focusedObject.gameObject.GetComponent<Highlightable> ().highlighted = false;

		dismiss(nextState, shouldPop);
	}
}
