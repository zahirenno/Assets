using UnityEngine;
using System.Collections;
using System;
using TouchScript.Gestures;

public class SimpleGameContoller : MonoBehaviour
{
	
	public StateStack ss1 = null;

	public virtual void TouchEvent(object sender, EventArgs e){
		Gesture gesture = (Gesture)sender;
		
		GameController.GestureResult res = new GameController.GestureResult ();
		res.gesture = gesture;
		
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (gesture.ScreenPosition.x, gesture.ScreenPosition.y, 0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit))
			res.hit = hit.collider.gameObject;
		
		this.GestureChanged (res);
		
	}

	public virtual void GestureChanged(GameController.GestureResult gesture){
		ss1.update (gesture);
	}

	// Use this for initialization
	protected virtual void Start ()
	{
		PressGesture press = gameObject.AddComponent<PressGesture> ();
		press.Pressed += TouchEvent;
		
		PanGesture pan = gameObject.AddComponent<PanGesture> ();
		pan.Panned += TouchEvent;
		
		TapGesture tap = gameObject.AddComponent<TapGesture>();
		tap.Tapped += TouchEvent;
		tap.NumberOfTapsRequired = 2;
		tap.TimeLimit = 0.5f;
		
		ScaleGesture scale = gameObject.AddComponent<ScaleGesture> ();
		scale.Scaled += TouchEvent;
		
		gameObject.AddComponent<PressGesture>().Pressed += TouchEvent;
		
		LongPressGesture longPress = gameObject.AddComponent<LongPressGesture>();
		longPress.LongPressed += TouchEvent;
		longPress.TimeToPress = 0.2f;
		longPress.AddFriendlyGesture (pan);
		
		gameObject.AddComponent<ReleaseGesture>().Released += TouchEvent;
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
	
	}
}

