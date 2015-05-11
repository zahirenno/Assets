using UnityEngine;
using TouchScript.Gestures;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

	public class GestureResult
	{
		public Gesture gesture = null;
		public GameObject hit = null;
	}

	public 	RoomHelper rh;
	public GameObject mainRoom = null;

	public StateStack ss1 = null;

	public CommandStack cSt = new CommandStack();
	
	public void Undo(){
		cSt.Undo ();
	}

	public void TouchEvent(object sender, EventArgs e){


		Gesture gesture = (Gesture)sender;

		GestureResult res = new GestureResult ();
		res.gesture = gesture;

		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (gesture.ScreenPosition.x, gesture.ScreenPosition.y, 0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			res.hit = hit.collider.gameObject;

		this.GestureChanged (res);

	}

	public void GestureChanged(GestureResult gesture){
		ss1.update (gesture);
	}

	// Use this for initialization
	void Start () {
		rh.LoadConfigurations ();
		reloadScene ();

		PressGesture press = gameObject.AddComponent<PressGesture> ();
		press.Pressed += TouchEvent;

		PanGesture pan = gameObject.AddComponent<PanGesture> ();
		pan.Panned += TouchEvent;
		//pan.MovementThreshold = 0;
		
		TapGesture tap = gameObject.AddComponent<TapGesture>();
		tap.Tapped += TouchEvent;
		tap.NumberOfTapsRequired = 2;
		tap.TimeLimit = 0.5f;

		ScaleGesture scale = gameObject.AddComponent<ScaleGesture> ();
		scale.Scaled += TouchEvent;
		//scale.MinPointsDistance = 0;

		gameObject.AddComponent<PressGesture>().Pressed += TouchEvent;
		
		LongPressGesture longPress = gameObject.AddComponent<LongPressGesture>();
		longPress.LongPressed += TouchEvent;
		longPress.TimeToPress = 0.2f;
		longPress.AddFriendlyGesture (pan);

		gameObject.AddComponent<ReleaseGesture>().Released += TouchEvent;

		FirstController.getGlobalFirstController ().dRegenButtonClicked.Add (onRegen);
		FirstController.getGlobalFirstController ().dOrderButtonClicked.Add (onOrderRoom);

	}

	private bool onRegen(Button sender){

		reloadScene ();

		return false;
	}

	bool stopCameraLayer = true;

	// Update is called once per frame
	void Update () {

		if (stopCameraLayer) {
			foreach (MeshCollider mc in FindObjectsOfType<MeshCollider> ()) {
				if (mc.gameObject.GetComponent<TouchScript.Hit.Untouchable> () == null)
					mc.gameObject.AddComponent<TouchScript.Hit.Untouchable> ();
			}
		} else {
			foreach (TouchScript.Hit.Untouchable mc in FindObjectsOfType<TouchScript.Hit.Untouchable>()){
				Destroy (mc);
			}
		}
	}


	public void reloadScene(){
		if (mainRoom != null) {
			GameObject.Destroy (mainRoom);
		}

		mainRoom = rh.Create ();
		mainRoom.transform.parent = this.gameObject.transform;
		ReflectionProbe reflectionProbe = mainRoom.gameObject.GetComponentInChildren<ReflectionProbe> ();
		if (reflectionProbe != null) {
			Debug.Log ("shshshdsgh");
			reflectionProbe.GetComponent<ReflectionProbe> ().RenderProbe ();
		}
	}

	private bool onOrderRoom(Button sender){
		//mainRoom.GetComponentInChildren<Room> ().generateOrder ();
		Debug.Log ("Order placed");
		return false;
	}
}
