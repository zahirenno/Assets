using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {

	public Vector3 center = new Vector3(0,0,0);
	public float theta = 0, psi = 0, ray = 3;
	public float friction = 0.8f;
	public float sensitivity = 3.14f;
	private float v_theta = 0, v_psi = 0;
	public Vector3 lookAt = new Vector3(0,0,0);
	public bool acceptInput = true;

	private Vector2 delta = new Vector2(0,0);
	private bool firm = false;
	public void setDelta(Vector2 delta){
		Debug.Log (delta);

		this.delta += delta;
		firm = true;
	}

	private Vector3 sphericalToCartesian(){
		Vector3 result = center;

		float y = ray * Mathf.Sin (psi);
		float x = ray * Mathf.Cos (psi) * Mathf.Cos (theta);
		float z = ray * Mathf.Cos (psi) * Mathf.Sin (theta);

		result += new Vector3 (x, y, z);

		return result;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		v_theta -= v_theta * friction * Time.deltaTime;
		v_psi -= v_psi * friction * Time.deltaTime;

		if (acceptInput) {
			if (firm){
				v_psi = -delta.y * sensitivity;
				v_theta = -delta.x * sensitivity;

				delta = new Vector2(0,0);
				firm = false;
			}
		}

		theta += Time.deltaTime * v_theta;
		psi += Time.deltaTime * v_psi;

		transform.position = sphericalToCartesian ();
		transform.LookAt (lookAt, transform.up);
	
	}
}
