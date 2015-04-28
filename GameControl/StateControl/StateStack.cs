using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

public class StateStack : MonoBehaviour, IStateListener {

	// Use this for initialization
	void Start () {
		rootState.listener = this;
		stack.Push(rootState);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected Stack<State> stack = new Stack<State>();
	
	public State rootState = null;

	public void update(GameController.GestureResult gesture){
		stack.Peek ().update (gesture);
		lastGesture = gesture;
	}
	
	public void onStarting (State sender) {
		sender.enabled = true;
	}
	public void onStarted (State sender) {
		
	}
	public void onEnding (State sender) {
		
	}
	public void onExit (State sender, State nextState, bool shouldPop) {
		sender.enabled = false;
		if (shouldPop) {
			Component.Destroy(stack.Pop ());
		}
		if (nextState != null)
			stack.Push(nextState);
		stack.Peek ().Enter ();
	}
	private GameController.GestureResult lastGesture = null;
	public GameController.GestureResult getLastGesture(State sender){
		return lastGesture;
	}

}
