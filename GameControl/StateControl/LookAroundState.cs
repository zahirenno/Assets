using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class LookAroundState : State {

	// Use this for initialization
	void Start () {
	
	}

	private TurnTableCamera turnTableCamera;

	protected override void start(){
		turnTableCamera = Camera.main.GetComponent<TurnTableCamera> ();
		turnTableCamera.enabled = true;
		base.start ();
	}

	protected override void die(State nextState, bool shouldPop){
		turnTableCamera.enabled = false;
		base.die (nextState, shouldPop);
	}

	protected override void onPanRunning(PanGesture gesture){
		turnTableCamera.setDelta ((gesture.ScreenPosition - gesture.PreviousScreenPosition) / 10.0f);
	}

	protected override void onReleaseRunning(ReleaseGesture gesture) {
		nextState = null;
		shouldPop = true;
		dismiss(nextState, shouldPop);
	}
	
	protected override void onPressDying(PressGesture gesture) {
		this.die (nextState, shouldPop);
	}

	protected override void UpdateDying(){

	}

}
