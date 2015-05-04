using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System;

public class InputHandler : MonoBehaviour
{

	InputZoomBehaviour zoom = new InputZoomBehaviour();
	ResizingRoom roomToResize = new ResizingRoom();

	public Camera camera;
	public GameObject room;

	// Use this for initialization

	void Start ()
	{
		ScaleGesture sg = gameObject.AddComponent<ScaleGesture> ();
		sg.Scaled += HandleZoom;

		PanGesture pg = gameObject.AddComponent<PanGesture> ();
		pg.MovementThreshold = 0.25F;
		pg.Panned += HandlePanned;

		LongPressGesture lpg = gameObject.AddComponent<LongPressGesture> ();
		lpg.LongPressed += HandleLongPressed;
		lpg.AddFriendlyGesture (pg);

		ReleaseGesture rg = gameObject.AddComponent<ReleaseGesture> ();
		rg.Released += HandleReleased;
	}

	void HandleReleased (object sender, EventArgs e)
	{
		roomToResize.HandleRelease (sender, room);
	}

	public void HandlePanned (object sender, EventArgs e)
	{
		roomToResize.HandlePanning (sender, room);
	}
	

	public void HandleZoom(object sender, EventArgs e){
		zoom.HandleInput (sender, camera);
	}

	public void HandleLongPressed(object sender,EventArgs e){
		roomToResize.HandleLongPress (sender, room);
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}

