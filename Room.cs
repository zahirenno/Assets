using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public List<Furniture> furnitures = new List<Furniture>();
	public EditableRoom editableRoom;

	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}

	void SendEmail (string _address, string _subject, string _body)
	{
		string email = _address;
		string subject = MyEscapeURL(_subject);
		string body = MyEscapeURL(_body);
		
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}


	public void generateOrder(){
		Dictionary<FurnitureEntry, int> items = new Dictionary<FurnitureEntry, int> ();

		foreach (Furniture furniture in furnitures) {
			if (!items.ContainsKey(furniture.getFurnitureType()))
				items.Add(furniture.getFurnitureType(), 0);
			++items[furniture.getFurnitureType()];
		}

		string result = "<html><head></head><body>";

		result += "<img src=\"http://upload.wikimedia.org/wikipedia/commons/thumb/c/c5/Ikea_logo.svg/200px-Ikea_logo.svg.png\" align=\"middle\" width=100% />";

		result += "<table border=\"1\" width=\"100%\">";

		result += "<tr>";
		
		result += "<td>" + "Item Code" + "</td>";
		result += "<td>" + "Unit Price" + "</td>";
		result += "<td>" + "Qty" + "</td>";
		result += "<td>" + "Total" + "</td>";
		
		result += "</tr>";

		foreach (KeyValuePair<FurnitureEntry, int> kv in items) {
			result += "<tr>";

			result += "<td>" + kv.Key.id + "</td>";
			result += "<td>" + "$" + kv.Key.price.ToString("0.00") + "</td>";
			result += "<td>" + kv.Value + "</td>";
			result += "<td>" + "$" + (kv.Key.price * (float)kv.Value).ToString("0.00") + "</td>";

			result += "</tr>";
		}

		result += "</table>";

		result += "</body></html>";



		SendEmail ("elie.karouz@gmail.com", "Order", result);

		//System.IO.File.WriteAllText ("C:/Users/Zahi Renno/Documents/gggg.html", result);

	}

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
