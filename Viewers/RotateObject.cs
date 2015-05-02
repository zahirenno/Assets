using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	private Vector3 delta = new Vector3 (0, 0, 0);
	private Vector3 v_delta = new Vector3 (0, 0, 0);
	public float friction = 0.8f;
	public float sensitivity = 3;
	private bool firm = false;
	public void setDelta(Vector3 delta){
		this.v_delta = delta * sensitivity;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		v_delta -= v_delta * friction * Time.deltaTime;

		delta += v_delta * Time.deltaTime;

		transform.RotateAround(transform.position, new Vector3(1,0,0), delta.y);
		transform.RotateAround(transform.position, new Vector3(0,1,0), -delta.x);

		delta = new Vector3(0,0,0);
	}
}

