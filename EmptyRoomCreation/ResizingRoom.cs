using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System;
using UnityEngine.UI;

public class ResizingRoom 
{
	private bool aWallIsSelected = false;

	// 1 for North, 2 for East 3 for South, 4 for west
	private int selectedWall;
	private string []toS= new string[4]{"N","E","S","W"};
 	private float hitY;

	public void HandleLongPress(object sender, GameObject room){
		LongPressGesture lpg = (LongPressGesture)sender;
		int k=CheckIfWallSelected(lpg.ScreenPosition.x,lpg.ScreenPosition.y);
		if (k != 0) {
			Debug.Log (k);
			aWallIsSelected=true;
			selectedWall=k;
			room.transform.FindChild(toS[selectedWall-1]).GetComponent<Highlightable>().highlighted=true;
		}
	}
	public void HandlePanning(object sender, GameObject room){
		PanGesture pg = (PanGesture)sender;
		if (aWallIsSelected) {
			Ray ray = Camera.main.ScreenPointToRay (new Vector3(pg.ScreenPosition.x, pg.ScreenPosition.y, 0));
			float t=(hitY-ray.origin.y)/ray.direction.y;
			float factor=1.0F;
			if(selectedWall==1){
				float z=ray.origin.z+ray.direction.z*t;
				z=(int)(Math.Round(z/0.025F))*0.025F;
				float oldZ=room.gameObject.transform.Find(toS[0]).transform.position.z;
				room.gameObject.transform.Find(toS[0]).transform.Translate(new Vector3(0,0,factor*(z-oldZ)));
				room.gameObject.transform.Find(toS[2]).transform.Translate(new Vector3(0,0,factor*(z-oldZ)));
			}else if (selectedWall==4){
				float x=ray.origin.x+ray.direction.x*t;
				x=(int)(Math.Round(x/0.025F))*0.025F;
				float oldX=room.gameObject.transform.Find(toS[3]).transform.position.x;
				room.gameObject.transform.Find(toS[1]).transform.Translate(new Vector3(0,0,factor*(oldX-x)));
				room.gameObject.transform.Find(toS[3]).transform.Translate(new Vector3(0,0,factor*(oldX-x)));
			}
		}
	}

	public void HandleRelease(object sender,GameObject room){
		if (aWallIsSelected) {
			aWallIsSelected=false;
			room.transform.FindChild (toS [selectedWall - 1]).GetComponent<Highlightable> ().highlighted = false;
		}
	}
	private int CheckIfWallSelected(float screenX, float screenY){
		Ray ray = Camera.main.ScreenPointToRay (new Vector3(screenX, screenY, 0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)){
			string name=hit.collider.gameObject.name;
			hitY=hit.transform.position.y;
			if(name.Equals("N"))
			   return 1;
			if(name.Equals("E"))
				return 2;
			if(name.Equals("S"))
				return 3;
			if(name.Equals("W"))
				return 4;
		}
		return 0;
	}
}

