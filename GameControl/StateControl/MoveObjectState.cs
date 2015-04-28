using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TouchScript.Gestures;

public class MoveObjectState : State {
	
	// Use this for initialization
	
	
	private Transform focusedObject;
	private Command b;
	
	public override void Enter(){
		//creating the command with old state
		
		b = new MoveFurnitureCommand (listener.getLastGesture (this).hit, 
		                              controller.GetComponentInChildren<Room> ());
		focusedObject = listener.getLastGesture (this).hit.transform;
		focusedObject.gameObject.GetComponent<Highlightable> ().highlighted = true;
		base.Enter ();
	}
	
	
	private Vector3 firstPosition;
	private Vector3 deltaPos = new Vector3(0,0,0);
	
	protected override void UpdateStarting(){
		if (Input.GetMouseButton (0)) {
			firstPosition = Input.mousePosition;
			start();
		}
	}
	
	protected override void UpdateRunning(){
		if (Input.GetMouseButton (0)) {
			
			if (deltaPos.magnitude < 70.0f){
				deltaPos = Input.mousePosition - firstPosition;
			}else{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition - deltaPos);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit))
					focusedObject.gameObject.GetComponent<MovableObject> ().move (ray, hit);
			}
		}
		
	}
	
	protected override void die (State nextState, bool shouldPop)
	{
		base.die (nextState, shouldPop);
		controller.cSt.Execute (b);
	}
	protected override void onReleaseRunning(ReleaseGesture gesture){
		nextState = null;
		shouldPop = true;
		
		focusedObject.gameObject.GetComponent<MovableObject> ().endMove ();
		focusedObject.gameObject.GetComponent<Highlightable> ().highlighted = false;
		
		dismiss(nextState, shouldPop);
	}
}
