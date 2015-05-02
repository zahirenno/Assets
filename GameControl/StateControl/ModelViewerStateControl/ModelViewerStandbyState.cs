using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ModelViewerStandbyState : StandbyState {

	// Use this for initialization
	void Start () {

	}
	
	protected override void onLongPressRunning(LongPressGesture gesture) {
		
	}
	protected override void onTapRunning(TapGesture gesture) {

	}
	protected override void onPanRunning(PanGesture gesture) {
		
		nextState = createNextState<LookAroundState>();
		shouldPop = false;
		dismiss(nextState, shouldPop);
		
	}

	public override void Enter(){

		camera = Camera.main;
		cameraFOV = this.camera.fieldOfView;

		lifeCycle = STATE.STARTING;
		listener.onStarting (this);
	}

	protected override void onScaleRunning(ScaleGesture gesture){

		this.camera.fieldOfView = (1.0f / gesture.LocalDeltaScale) * this.camera.fieldOfView;
	}

}
