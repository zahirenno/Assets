using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	private float _width = 1.0f;
	public float Width {
		get { return _width;}
		set { _width = value;}
	}

	private float _height = 1.0f;
	public float Height {
		get { return _height;}
		set { _height = value;}
	}


	public float objectScale = 10;
	private void updateScale(){
		transform.localScale = new Vector3 (_width / objectScale, 1, _height / objectScale);
	}

	// Use this for initialization
	void Start () {
		updateScale ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
