using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleButton : MonoBehaviour
{

	public Sprite onSprite = null, offSprite = null;
	public bool state = false;

	void onToggle(){
		state = !state;
		if (state)
			transform.FindChild("Image").GetComponent<Image> ().sprite = onSprite;
		else
			transform.FindChild("Image").GetComponent<Image> ().sprite = offSprite;
	}

	// Use this for initialization
	void Start ()
	{
		this.GetComponent<Button> ().onClick.AddListener (() => onToggle ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

