using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DisplayDimensions : MonoBehaviour
{

	public GameObject room;
	private Text text;
	// Use this for initialization

	private string Dimensions(){
		return "" + (int) (Math.Round(room.transform.FindChild("N").localScale.x*20))*5+ "X"+
			(int)(Math.Round(room.transform.FindChild("W").localScale.x*20))*5;
	}
	void Start ()
	{
		text = gameObject.GetComponent<Text> ();
		text.text = Dimensions ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		text.text = Dimensions ();
	}
}

