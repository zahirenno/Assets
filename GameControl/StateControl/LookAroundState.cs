using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TouchScript.Gestures;

public class LookAroundState : State {

	// Use this for initialization
	void Start () {
	
	}

	private TurnTableCamera turnTableCamera;

	protected virtual bool onFurnitureDragged(ViewController sender, FurnitureEntry entry, Vector2 screenPosition){
		Debug.Log ("drag reported from standby");
		GameObject f = Catalog.getCatalog ().createFurniture (entry.id, new Vector3 (0, 0, 0), 0);
		if (f != null) {
			controller.GetComponentInChildren<Room> ().addFurniture (f.GetComponent<Furniture> ());
			f.GetComponent<MovableObject> ().place (screenPosition);
			nextState = createNextState<AddObjectState> ();
			((AddObjectState)nextState).focusedObject = f.transform;
			shouldPop = false;
			dismiss (nextState, shouldPop);
			die (nextState, shouldPop);
		}
		return false;
	}

	public override void Enter(){
		if (FirstController.getGlobalFirstController () != null) {
			FirstController.getGlobalFirstController ().dFurnitureDragged.Add (onFurnitureDragged);
		}
		base.Enter ();
	}
	
	public override void dismiss (State nextState, bool shouldPop)
	{
		if (FirstController.getGlobalFirstController () != null) {
			FirstController.getGlobalFirstController ().dFurnitureDragged.Remove (onFurnitureDragged);
		}
		base.dismiss (nextState, shouldPop);
	}

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
		
		/*if (gesture.ActiveTouches.Count > 0)
			if (EventSystem.current.IsPointerOverGameObject ())
				return;*/

		turnTableCamera.setDelta ((gesture.ScreenPosition - gesture.PreviousScreenPosition) / 10.0f);
	}

	protected override void onReleaseRunning(ReleaseGesture gesture) {
		nextState = null;
		shouldPop = true;
		dismiss(nextState, shouldPop);
	}
	
	protected override void onPressDying(PressGesture gesture) {
		nextState = null;
		shouldPop = true;
		this.die (nextState, shouldPop);
	}

	protected override void UpdateDying(){

	}

}
