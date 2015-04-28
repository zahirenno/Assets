using TouchScript.Gestures;

public interface IStateListener {

	void onStarting (State sender);
	void onStarted (State sender);
	void onEnding (State sender);
	void onExit (State sender, State nextState, bool shouldPop);
	GameController.GestureResult getLastGesture (State sender);

}
