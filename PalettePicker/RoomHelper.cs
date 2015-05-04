using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class RoomHelper
{
	private RoomType type;
	private Room room;
	public List<string> configurations=new List<string>();
	
	public RoomHelper(Room room, RoomType type){
		this.room = room;
		this.type = type;
	}

	public void Rearrange(Room room){
		//if we don't have any configuration yet, we will communicate with the server in order to get
		//possible configurations
		//for now we will put a static communication with a file
		if (configurations.Count == 0) {
			LoadConfigurations ();
			if (configurations.Count == 0) {
				return;
			}
		}
		reloadRoom (room);
	}

	public void Recolor(Room room){
		RoomConcretizer.refurnish (room);
	}

	public GameObject Create(){
		GameObject k = CreateEmpty();
		GameObject roomContainer = k.transform.FindChild ("Room Container").gameObject;
		reloadRoom (roomContainer.GetComponent<Room>());
		return k;
	}

	public GameObject CreateEmpty(){
		GameObject mainRoom = GameObject.Instantiate<GameObject> ((GameObject)Resources.Load ("Room"));
		mainRoom.GetComponent<EditableRoom> ().camera = Camera.main;



		
		GameObject roomObj = new GameObject ("Room Container");
		roomObj.AddComponent<Room> ().editableRoom = mainRoom.GetComponent<EditableRoom>();
		roomObj.transform.SetParent(mainRoom.transform);

		mainRoom.GetComponent<EditableRoom> ().width = 3.70F;
		mainRoom.GetComponent<EditableRoom> ().height = 4.35F;
		//set positions of walls in room
		//TODO let the code set the dimensions of the prototype room 
		Transform N = mainRoom.transform.Find ("N");
		Transform S = mainRoom.transform.Find ("S");
		Transform W = mainRoom.transform.Find ("W");
		Transform E = mainRoom.transform.Find ("E");

		N.position = new Vector3(N.position.x, N.position.y, 2.04F );
		S.position = new Vector3(S.position.x, S.position.y, -2.04F );
		W.position = new Vector3(-1.76F, W.position.y, W.position.z);
		E.position = new Vector3(1.76F, E.position.y, E.position.z);

		return mainRoom;
	}

	public static GameObject Clone(GameObject room){
		throw new NotImplementedException ();
	}

	public void LoadConfigurations(){
		string message = Resources.Load<TextAsset> ("ReplyFurnish").text;
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (message);
		XmlNode rooms = doc.SelectSingleNode ("/message/parameters/Rooms");
		XmlNodeList roomsNodeList = rooms.SelectNodes ("Room");
		foreach (XmlNode roomNode in roomsNodeList) {
			string x = "<Room>";
			x += roomNode.InnerXml;
			x += "</Room>";
			configurations.Add (x);
		}

		for (int i=0; i<configurations.Count; ++i) {
			Debug.Log(configurations[i]);
		}
	}

	public void reloadRoom(Room roomT){
		//TODO Remove offsets for positions dirty way
		//empty the room
		while (roomT.furnitures.Count>0)
			roomT.removeFurniture (roomT.furnitures [0]);


		string xmlConfiguration = configurations[RandomHelper.nextInt(configurations.Count)];

		Catalog catalog = Catalog.getCatalog ();
		
		XmlDocument doc = new XmlDocument ();

		doc.LoadXml (RoomConcretizer.pickConcreteFurnitures (xmlConfiguration));
		
		XmlNode room = doc.SelectSingleNode ("Room").SelectSingleNode ("Furnitures");
		XmlNodeList furnitures = room.SelectNodes ("Furniture");

		foreach (XmlNode furnitureNode in furnitures) {
			string id = furnitureNode.SelectSingleNode("CatalogId").InnerText;
			
			XmlNode positionNode = furnitureNode.SelectSingleNode("Position");
			Vector3 position = new Vector3();
			position.x = (float)Convert.ToDouble(positionNode.Attributes["posX"].Value) * catalog.scale-1.75F;
			position.y = (float)Convert.ToDouble(furnitureNode.Attributes["height"].Value) * catalog.scale / 2.0f;
			position.z = (float)Convert.ToDouble(positionNode.Attributes["posY"].Value) * catalog.scale-2.04F;
			
			float angle = (float)Convert.ToDouble(furnitureNode.Attributes["rotation"].Value);
			
			if (id != null){
				String mid = id;
				GameObject fur = catalog.createFurniture(mid, position, angle, true);
				if (fur != null){
					fur.transform.SetParent(roomT.gameObject.transform);
					roomT.addFurniture(fur.GetComponent<Furniture>());
				}
			}
		}
	}

}

