using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public List<Furniture> furnitures = new List<Furniture>();
	public EditableRoom editableRoom;

	public string getXML(){
		string result = "";

		result += "<Room ";
		result += "posX=\"" + 0 + "\" ";
		result += "posY=\"" + 0 + "\" ";
		result += "width=\"" + editableRoom.width + "\" ";
		result += "depth=\"" + editableRoom.height + "\" ";
		result += ">\n";

		result += "<Furnitures>\n";

		foreach (Furniture furniture in furnitures)
			result += furniture.getXML ();

		result += "</Furnitures>\n";
		result += "</Room>\n";

		return result;
	}

	public bool removeFurniture(Furniture furniture){
		if (!furnitures.Contains (furniture))
			return false;
		furnitures.Remove (furniture);
		Destroy (furniture.gameObject);
		return true;
	}

	public void addFurniture(Furniture furniture){
		GameObject roomContainer = this.gameObject;
		if (roomContainer != null) {
			furniture.transform.parent = roomContainer.transform;
			furnitures.Add (furniture);
		} else {
			Debug.LogError("Room Container NOT FOUND");
		}
	}

	public bool swapFurniture(Furniture from, Furniture to){
		int index = furnitures.IndexOf (from);
		if (index >= 0) {
			to.transform.position = new Vector3 (from.transform.position.x, to.transform.position.y, from.transform.position.z);
			to.transform.eulerAngles = new Vector3 (to.transform.eulerAngles.x, from.transform.eulerAngles.y, to.transform.eulerAngles.z);
			to.transform.parent = gameObject.transform;
			GameObject.Destroy (from.gameObject);
			furnitures[index] = to;
		}
		return true;
	}

	private Vector3 vMax(Vector3 v1, Vector3 v2){
		return new Vector3 (Mathf.Max (v1.x, v2.x),
		                   Mathf.Max (v1.y, v2.y),
		                   Mathf.Max (v1.z, v2.z));
	}

	private Vector3 vMin(Vector3 v1, Vector3 v2){
		return new Vector3 (Mathf.Min (v1.x, v2.x),
		                    Mathf.Min (v1.y, v2.y),
		                    Mathf.Min (v1.z, v2.z));
	}

	public Bounds getRoomBounds(){
		Bounds bounds = new Bounds ();

		foreach (Furniture f in furnitures) {
			Collider[] furnitureColliders = f.gameObject.GetComponents<Collider>();
			foreach (Collider furnitureCollider in furnitureColliders){
				Bounds furnitureBounds = furnitureCollider.bounds;
				bounds.max = vMax (bounds.max, furnitureBounds.max);
				bounds.min = vMin (bounds.min, furnitureBounds.min);
			}
		}

		return bounds;
	}

	public void centerRoom(){
		Vector3 center = getRoomBounds().center;
		this.transform.Translate (new Vector3(-center.x, 0, -center.z));
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
}
