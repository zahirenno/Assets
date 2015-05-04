using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchToEditScene : MonoBehaviour
{

	public Button createButton;
	// Use this for initialization
	void Start ()
	{
		createButton.onClick.AddListener (() => SwitchScene ());
	}

	void SwitchScene(){
		//TODO build the RoomHelper
		DontDestroyOnLoad (GameObject.Find ("PersistantContext"));
		Application.LoadLevel ("j");
	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}

