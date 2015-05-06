using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class StandbyState : State {

	// Use this for initialization
	void Start () {

	}
	
	protected override void onLongPressRunning(LongPressGesture gesture) {

		if (this.getHitObject() == null){
			nextState = createNextState<LookAroundState>();
			shouldPop = false;
			dismiss(nextState, shouldPop);
		} else if (this.getHitObject().GetComponent<MovableWall>() != null) {
			nextState = createNextState<MoveWallState>();
			shouldPop = false;
			dismiss(nextState, shouldPop);
		} else if (this.getHitObject().GetComponent<MovableObject>() != null) {
			nextState = createNextState<MoveObjectState>();
			shouldPop = false;
			dismiss(nextState, shouldPop);
		}
		
	}
	protected override void onTapRunning(TapGesture gesture) {
		if (this.getHitObject() != null){
			if  (this.getHitObject().GetComponent<MovableWall>() != null){
				return;
			} else if (this.getHitObject().GetComponent<MovableObject>() != null){
				if (gesture.State == Gesture.GestureState.Recognized){
					nextState = createNextState<ObjectFocusState>();
					shouldPop = false;
					dismiss(nextState, shouldPop);
				}
			}
		}
	}
	protected override void onPanRunning(PanGesture gesture) {
		
		nextState = createNextState<LookAroundState>();
		shouldPop = false;
		dismiss(nextState, shouldPop);
		
	}

	protected Camera camera;
	protected float cameraFOV;

	public override void Enter(){

		camera = Camera.main;
		cameraFOV = this.camera.fieldOfView;

		FirstController.getGlobalFirstController ().deleteButtonIsVisible (false);
		FirstController.getGlobalFirstController ().orderButton.gameObject.SetActive (true);
		FirstController.getGlobalFirstController ().navButtonIsVisible (true);

		base.Enter ();
	}

	protected override void onScaleRunning(ScaleGesture gesture){

		this.camera.fieldOfView = (1.0f / gesture.LocalDeltaScale) * this.camera.fieldOfView;
	}

}
