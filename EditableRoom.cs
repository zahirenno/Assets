using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditableRoom : MovableObject {

	public float expandedWallHeight = 3.0f;
	public float collapsedWallHeight = 0.3f;
	public float height = 5.5f;
	public float width = 5.5f;
	
	private List<Wall> walls = new List<Wall>();
	private Dictionary<Wall, Vector3> wallLastRecordedPosition = new Dictionary<Wall, Vector3> ();
	private Wall masterWall = null;
	
	private GameObject floor;

	private void initializeGeometry(){

		float lposx = width / 2.0f;
		float lposz = height / 2.0f;

		walls[2].transform.localPosition = new Vector3(lposx, walls[0].transform.localPosition.y, walls[0].transform.localPosition.z);
		walls[3].transform.localPosition = new Vector3(-lposx, walls[1].transform.localPosition.y, walls[1].transform.localPosition.z);
		walls[0].transform.localPosition = new Vector3(walls[2].transform.localPosition.x, walls[2].transform.localPosition.y, lposz);
		walls[1].transform.localPosition = new Vector3(walls[3].transform.localPosition.x, walls[3].transform.localPosition.y, -lposz);
	}
	
	private void updateGeometry(){
		
		if (masterWall.name.Equals ("N") || masterWall.name.Equals ("S")) {
			float lposz = (walls [0].transform.localPosition.z + walls [1].transform.localPosition.z) / 2.0f;
			float length = Mathf.Abs(walls [0].transform.localPosition.z - walls [1].transform.localPosition.z);
			
			walls[2].transform.localPosition = new Vector3(walls[2].transform.localPosition.x, walls[2].transform.localPosition.y, lposz);
			walls[3].transform.localPosition = new Vector3(walls[3].transform.localPosition.x, walls[3].transform.localPosition.y, lposz);
			
			walls[2].transform.localScale = new Vector3(length, walls[2].transform.localScale.y, walls[2].transform.localScale.z);
			walls[3].transform.localScale = new Vector3(length, walls[3].transform.localScale.y, walls[3].transform.localScale.z);
			
			floor.transform.localPosition = new Vector3(floor.transform.localPosition.x, floor.transform.localPosition.y, lposz);
			floor.transform.localScale = new Vector3(floor.transform.localScale.x, floor.transform.localScale.y, length * 0.1f);
			
			height = length;
			
		} else {
			float lposx = (walls [2].transform.localPosition.x + walls [3].transform.localPosition.x) / 2.0f;
			float length = Mathf.Abs(walls [2].transform.localPosition.x - walls [3].transform.localPosition.x);
			
			walls[0].transform.localPosition = new Vector3(lposx, walls[0].transform.localPosition.y, walls[0].transform.localPosition.z);
			walls[1].transform.localPosition = new Vector3(lposx, walls[1].transform.localPosition.y, walls[1].transform.localPosition.z);
			
			walls[0].transform.localScale = new Vector3(length, walls[0].transform.localScale.y, walls[0].transform.localScale.z);
			walls[1].transform.localScale = new Vector3(length, walls[1].transform.localScale.y, walls[1].transform.localScale.z);
			
			floor.transform.localPosition = new Vector3(lposx, floor.transform.localPosition.y, floor.transform.localPosition.z);
			floor.transform.localScale = new Vector3(length * 0.1f, floor.transform.localScale.y, floor.transform.localScale.z);
			
			width = length;
		}
		
	}
	
	public Camera camera;
	
	// Use this for initialization
	void Start ()
	{
		if (this.camera == null)
			this.camera = Camera.main;
		
		walls.Add (transform.FindChild ("N").GetComponent<Wall> ());
		walls.Add (transform.FindChild ("S").GetComponent<Wall> ());
		walls.Add (transform.FindChild ("E").GetComponent<Wall> ());
		walls.Add (transform.FindChild ("W").GetComponent<Wall> ());
		
		floor = transform.FindChild ("Floor").gameObject;

		initializeGeometry ();

		foreach (Wall wall in walls) {
			masterWall = wall;
			updateGeometry ();
		}

		foreach (Wall wall in walls) {
			wallLastRecordedPosition[wall] = wall.transform.localPosition;
		}
	}
	
	void updateWallHeights(){
		foreach (Wall wall in walls) {
			if (this.camera != null){
				if (Vector3.Dot(wall.transform.forward, this.camera.transform.forward) > -0.3f)
					wall.transform.localScale = new Vector3(wall.transform.localScale.x, expandedWallHeight * wall.transform.parent.lossyScale.y, wall.transform.localScale.z);
				else
					wall.transform.localScale = new Vector3(wall.transform.localScale.x, collapsedWallHeight * wall.transform.parent.lossyScale.y, wall.transform.localScale.z);
			}
			wall.transform.localPosition = new Vector3 (wall.transform.localPosition.x, wall.transform.localScale.y / 2.0f, wall.transform.localPosition.z);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Find the wall that is being edited, and update the previous wall positions
		masterWall = null;
		foreach (Wall wall in walls) {
			if (!wall.transform.localPosition.Equals(wallLastRecordedPosition[wall])){
				masterWall = wall;
				break;
			}
		}
		
		if (masterWall != null)
			updateGeometry ();
		
		updateWallHeights ();
		
		foreach (Wall wall in walls) {
			wallLastRecordedPosition[wall] = wall.transform.localPosition;
		}
		
	}
}
