using UnityEngine;
using TouchScript.Gestures;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Xml;

public class GameController : MonoBehaviour {

	public class GestureResult
	{
		public Gesture gesture = null;
		public GameObject hit = null;
	}

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

		reloadScene ();

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

		FirstController.getGlobalFirstController ().dRegenButtonClicked.Add (onRegen);

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

	public void loadRoom(string xmlContent){
		Catalog catalog = Catalog.getCatalog ();

		XmlDocument doc = new XmlDocument ();
		
		//doc.LoadXml (xmlContent);
		doc.LoadXml (RoomConcretizer.pickConcreteFurnitures (xmlContent));
		
		XmlNode room = doc.SelectSingleNode ("Room").SelectSingleNode ("Furnitures");
		XmlNodeList furnitures = room.SelectNodes ("Furniture");
		
		GameObject mainRoom = Instantiate<GameObject> ((GameObject)Resources.Load ("Room"));
		//mainRoom.name = "Room 1";
		mainRoom.GetComponent<EditableRoom> ().camera = Camera.main;

		GameObject roomObj = new GameObject ("Room Container");
		roomObj.AddComponent<Room> ().editableRoom = mainRoom.GetComponent<EditableRoom>();
		roomObj.transform.SetParent(mainRoom.transform);
		
		Vector3 center = new Vector3 (0, 0, 0);
		float nb = 0.0f;
		
		foreach (XmlNode furnitureNode in furnitures) {
			string id = furnitureNode.SelectSingleNode("CatalogId").InnerText;
			
			XmlNode positionNode = furnitureNode.SelectSingleNode("Position");
			Vector3 position = new Vector3();
			position.x = (float)Convert.ToDouble(positionNode.Attributes["posX"].Value) * catalog.scale;
			position.y = (float)Convert.ToDouble(furnitureNode.Attributes["height"].Value) * catalog.scale / 2.0f;
			position.z = (float)Convert.ToDouble(positionNode.Attributes["posY"].Value) * catalog.scale;
			
			float angle = (float)Convert.ToDouble(furnitureNode.Attributes["rotation"].Value);
			
			if (id != null){
				String mid = id;
				
				GameObject fur = catalog.createFurniture(mid, position, angle, true);
				if (fur != null){
					fur.transform.SetParent(roomObj.transform);
					nb += 1.0f;
					center += fur.transform.position;
					
					roomObj.GetComponent<Room>().furnitures.Add(fur.GetComponent<Furniture>());
				}
			}
		}
		
		center /= -nb;
		//roomObj.transform.Translate (center.x, 0, center.z);

		mainRoom.transform.SetParent (this.transform);

		this.mainRoom = mainRoom;
	}



	public void reloadScene(){
		if (mainRoom != null) {
			GameObject.Destroy (mainRoom);
		}

		loadRoom (Resources.Load<TextAsset> ("output_all").text);

		ReflectionProbe reflectionProbe = mainRoom.gameObject.GetComponentInChildren<ReflectionProbe> ();
		if (reflectionProbe != null) {
			Debug.Log("shshshdsgh");
			reflectionProbe.GetComponent<ReflectionProbe> ().RenderProbe ();
		}
	}
}
