using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System;

public class InputZoomBehaviour
{
	public void HandleInput(object sender, Camera camera){
		ScaleGesture gesture = (ScaleGesture) sender;
		Vector3 expPos=camera.gameObject.transform.localPosition * (1.0F / gesture.LocalDeltaScale);
		if (expPos.magnitude < 50 && expPos.magnitude > 1) {
			camera.gameObject.transform.localPosition = expPos;
			camera.gameObject.transform.LookAt (new Vector3(-1,0,1));
		}
	}
}


