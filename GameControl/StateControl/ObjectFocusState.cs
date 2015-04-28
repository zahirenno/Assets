using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using UnityEngine.UI;

public class ObjectFocusState : State, ZoomCamera.IZoomCameraListener{
	
	// Use this for initialization
	
	private ZoomCamera zoomCamera;
	private Transform focusedObject;
	private Button deleteButton;
	private GameObject undoButton;
	private string focusedObjectName;
	private CommandStack tempcSt;
	private Command b;
	
	public void onZoomRevertEnd(ZoomCamera sender){
		zoomCamera.enabled = false;
		this.die (nextState, shouldPop);
	}
	
	protected override void UpdateDying(){
	}
	
	public bool onSelectedFurniture(ViewController sender, FurnitureEntry selectedEntry){
		Furniture furniture = Catalog.getCatalog().createFurniture (selectedEntry.id,
		                                                            new Vector3 (0, 0, 0),0).GetComponent<Furniture> ();
		tempcSt.Execute (new SwapFurnitureCommand (
			focusedObject.gameObject, furniture.gameObject,
			controller.GetComponentInChildren<Room> ()));
		
		this.gameObject.GetComponentInChildren<Room> ().swapFurniture (focusedObject.GetComponent<Furniture> (),
		                                                               furniture);
		focusedObject = furniture.gameObject.transform;
		furniture.gameObject.name = focusedObjectName;
		return false;
	}
	
	public void HandleUndoFSClick(){
		tempcSt.Undo ();
		Room r = controller.GetComponentInChildren<Room> ();
		foreach(Furniture f in r.furnitures){
			if(f.gameObject.name.Equals(focusedObjectName))
				focusedObject=f.transform;
		}
	}
	public override void dismiss (State nextState, bool shouldPop)
	{
		FirstController.getGlobalFirstController ().dDeleteButtonClicked.Remove (onDelete);
		FirstController.getGlobalFirstController ().dFurniturePicked.Remove (onSelectedFurniture);
		
		base.dismiss (nextState, shouldPop);
	}
	
	public override void Enter(){
		//prepare command for undo
		FirstController.getGlobalFirstController ().deleteButtonIsVisible (true);
		
		
		FirstController.getGlobalFirstController ().undoButtonFS.gameObject.SetActive (true);
		FirstController.getGlobalFirstController ().undoButtonFS.
			onClick.AddListener (() => HandleUndoFSClick ());
		
		zoomCamera = Camera.main.GetComponent<ZoomCamera> ();
		zoomCamera.enabled = true;
		zoomCamera.camera = Camera.main;
		zoomCamera.listener = this;
		
		focusedObject = listener.getLastGesture (this).hit.transform;
		focusedObjectName = focusedObject.gameObject.name;
		zoomCamera.focusOnObject (focusedObject);
		
		if (focusedObject == null) {
			Debug.LogWarning ("focused is null!!");
		} 
		
		//hide the undo button
		undoButton = GameObject.Find ("UndoButton");
		undoButton.SetActive (false);
		tempcSt = new CommandStack ();
		
		//assume that the furniture is going to be rotated
		FirstController.getGlobalFirstController ().dDeleteButtonClicked.Add (onDelete);
		FirstController.getGlobalFirstController ().dFurniturePicked.Add (onSelectedFurniture);
		FirstController.getGlobalFirstController ().regenButton.gameObject.SetActive (false);
		FirstController.getGlobalFirstController ().navButtonIsVisible (false);
		base.Enter ();
	}
	
	private bool onDelete(Button button){
		if (focusedObject == null) {
			Debug.LogWarning ("focused is null!!");
		} else {
			b = new DeleteFurnitureCommand (focusedObject.gameObject, controller.GetComponentInChildren<Room> ());
			tempcSt.Execute (b);
			this.gameObject.GetComponentInChildren<Room> ().removeFurniture (focusedObject.GetComponent<Furniture> ());
		}
		
		nextState = null;
		shouldPop = true;
		zoomCamera.revert ();
		
		
		dismiss(nextState, shouldPop);
		
		return false;
	}
	
	bool rotatedDirty = false;
	protected override void onPanRunning(PanGesture gesture){
		rotatedDirty = true;
		focusedObject.Rotate(0, -(gesture.ScreenPosition.x - gesture.PreviousScreenPosition.x) * .5f, 0, Space.World);
		GameObject.Find("MessageBox").GetComponent<Text>().text = focusedObject.eulerAngles.y.ToString("0") + "\u00B0";
	}
	
	protected override void onTapRunning(TapGesture gesture){
		nextState = null;
		shouldPop = true;
		zoomCamera.revert ();
		Debug.Log ("Tap");
		dismiss(nextState, shouldPop);
	}
	protected override void die (State nextState, bool shouldPop)
	{
		//copy tempcSt to controller.cSt
		controller.cSt.pushStack (tempcSt);
		FirstController.getGlobalFirstController ().undoButtonFS.gameObject.SetActive (false);
		undoButton.SetActive (true);
		base.die (nextState, shouldPop);
	}
	protected override void onReleaseRunning (ReleaseGesture gesture)
	{
		if(rotatedDirty)
			tempcSt.Execute (b);
	}
	protected override void onPressRunning (PressGesture gesture)
	{
		rotatedDirty = false;
		b = new MoveFurnitureCommand (focusedObject.gameObject, 
		                              controller.gameObject.GetComponentInChildren<Room> ());
	}
}
