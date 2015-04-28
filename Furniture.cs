using UnityEngine;
using System.Collections;
using System.Xml;

public class Furniture : MonoBehaviour {
	
	private FurnitureEntry type;
	
	public FurnitureEntry getFurnitureType(){
		return type;
	}
	
	public void setFurnitureType(FurnitureEntry type){
		this.type = type;
	}
	
	public Furniture (FurnitureEntry type){
		this.type = type;
	}
	
	public string getXML(){
		
		string result = "";
		
		result += "<Furniture ";
		result += "width=\"" + type.width*100.0F + "\" ";
		result += "depth=\"" + type.depth*100.0F + "\" ";
		result += "height=\"" + type.height*100.0F + "\" ";
		//TODO recheck
		result += "rotation=\"" + (-transform.localEulerAngles.y*3.141592F/180.0F) + "\" ";
		
		result += ">\n";
		
		result += "<CatalogId>" + type.id + "</CatalogId>\n";
		result += "<Name>" + type.name + "</Name>\n";
		result += "<Position ";
		result += "posX=\"" + transform.position.x*100.0F + "\" ";
		result += "posY=\"" + transform.position.z*100.0F + "\" ";
		result += "/>\n";
		
		result += "</Furniture>\n";
		
		return result;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
