using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

public class State : MonoBehaviour {

	public GameController controller ;	
	private GameObject hitObject = null;
	protected GameObject getHitObject(){
		return hitObject;
	}

	public virtual void Enter(){
		lifeCycle = STATE.STARTING;
		listener.onStarting (this);
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	public virtual void Update () {
		switch (lifeCycle) {
		case STATE.STARTING:
			UpdateStarting();
			break;
		case STATE.RUNNING:
			UpdateRunning();
			break;
		case STATE.DYING:
			UpdateDying();
			break;
		}
	}

	public enum STATE {STARTING, RUNNING, DYING, DEAD};
	public STATE lifeCycle = STATE.STARTING;
	public bool shouldPop = false;
	public State nextState = null;
	
	public IStateListener listener = null;
	
	public virtual void dismiss (State nextState, bool shouldPop){
		if (lifeCycle == STATE.DEAD)
			return;
		
		lifeCycle = STATE.DYING;
		listener.onEnding(this);
	}
	
	protected virtual void die(State nextState, bool shouldPop){
		if (lifeCycle != STATE.DYING)
			return;
		
		lifeCycle = STATE.DEAD;
		listener.onExit(this, nextState, shouldPop);
	}
	
	protected virtual void start(){
		if (lifeCycle != STATE.STARTING)
			return;
		
		lifeCycle = STATE.RUNNING;
		listener.onStarted(this);
	}
	

	public void update(GameController.GestureResult gesture){
		hitObject = gesture.hit;
		onGesture (gesture.gesture);
	}
	
	protected T createNextState<T> () where T : State {
		T t = gameObject.GetComponent<T>();
		if (t == null)
			t = gameObject.AddComponent<T>();
		t.listener = listener;
		t.controller = gameObject.GetComponent<GameController> ();
		return t;
	}

	protected virtual void UpdateStarting(){
		this.start();
	}
	protected virtual void UpdateRunning(){
	}
	protected virtual void UpdateDying(){
		this.die(nextState, shouldPop);
	}

	protected virtual void onGesture(Gesture gesture){
		switch (lifeCycle) {
		case STATE.STARTING:
			onGestureStarting(gesture);
			break;
		case STATE.RUNNING:
			onGestureRunning(gesture);
			break;
		case STATE.DYING:
			onGestureDying(gesture);
			break;
		}

		if (gesture is LongPressGesture)
			onLongPress ((LongPressGesture)gesture);
		else if (gesture is PressGesture)
			onPress ((PressGesture)gesture);
		else if (gesture is TapGesture)
			onTap ((TapGesture)gesture);
		else if (gesture is ReleaseGesture)
			onRelease ((ReleaseGesture)gesture);
		else if (gesture is PanGesture)
			onPan ((PanGesture)gesture);
		else if (gesture is ScaleGesture)
			onScale ((ScaleGesture)gesture);
	}

	protected virtual void onGestureStarting (Gesture gesture){
	}
	protected virtual void onGestureRunning(Gesture gesture){
	}
	protected virtual void onGestureDying(Gesture gesture){
	}
	
	protected virtual void onLongPress(LongPressGesture gesture){
		switch (lifeCycle) {
		case STATE.STARTING:
			onLongPressStarting(gesture);
			break;
		case STATE.RUNNING:
			onLongPressRunning(gesture);
			break;
		case STATE.DYING:
			onLongPressDying(gesture);
			break;
		}
	}
	protected virtual void onLongPressStarting (LongPressGesture gesture){
	}
	protected virtual void onLongPressRunning(LongPressGesture gesture){
	}
	protected virtual void onLongPressDying(LongPressGesture gesture){
	}

	protected virtual void onTap(TapGesture gesture){
		switch (lifeCycle) {
		case STATE.STARTING:
			onTapStarting(gesture);
			break;
		case STATE.RUNNING:
			onTapRunning(gesture);
			break;
		case STATE.DYING:
			onTapDying(gesture);
			break;
		}
	}
	protected virtual void onTapStarting (TapGesture gesture){
	}
	protected virtual void onTapRunning(TapGesture gesture){
	}
	protected virtual void onTapDying(TapGesture gesture){
	}

	protected virtual void onPress(PressGesture gesture){
		switch (lifeCycle) {
		case STATE.STARTING:
			onPressStarting(gesture);
			break;
		case STATE.RUNNING:
			onPressRunning(gesture);
			break;
		case STATE.DYING:
			onPressDying(gesture);
			break;
		}
	}
	protected virtual void onPressStarting (PressGesture gesture){
	}
	protected virtual void onPressRunning(PressGesture gesture){
	}
	protected virtual void onPressDying(PressGesture gesture){
	}

	protected virtual void onRelease(ReleaseGesture gesture){
		switch (lifeCycle) {
		case STATE.STARTING:
			onReleaseStarting(gesture);
			break;
		case STATE.RUNNING:
			onReleaseRunning(gesture);
			break;
		case STATE.DYING:
			onReleaseDying(gesture);
			break;
		}
	}
	protected virtual void onReleaseStarting (ReleaseGesture gesture){
	}
	protected virtual void onReleaseRunning(ReleaseGesture gesture){
	}
	protected virtual void onReleaseDying(ReleaseGesture gesture){
	}

	protected virtual void onScale(ScaleGesture gesture){
		switch (lifeCycle) {
		case STATE.STARTING:
			onScaleStarting(gesture);
			break;
		case STATE.RUNNING:
			onScaleRunning(gesture);
			break;
		case STATE.DYING:
			onScaleDying(gesture);
			break;
		}
	}
	protected virtual void onScaleStarting (ScaleGesture gesture){
	}
	protected virtual void onScaleRunning(ScaleGesture gesture){
	}
	protected virtual void onScaleDying(ScaleGesture gesture){
	}

	protected virtual void onPan(PanGesture gesture){
		switch (lifeCycle) {
		case STATE.STARTING:
			onPanStarting(gesture);
			break;
		case STATE.RUNNING:
			onPanRunning(gesture);
			break;
		case STATE.DYING:
			onPanDying(gesture);
			break;
		}
	}
	protected virtual void onPanStarting (PanGesture gesture){
	}
	protected virtual void onPanRunning(PanGesture gesture){
	}
	protected virtual void onPanDying(PanGesture gesture){
	}
}
