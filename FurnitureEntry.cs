using UnityEngine;
using System.Collections;

public class FurnitureEntry {
	public string id;
	public string name;
	public string category;
	public string model;
	public string image;
	public float width, height, depth;
	public float price = 15.0f;
	public float[,] modelRotation = {{1,0,0},{0,1,0},{0,0,1}};
}
