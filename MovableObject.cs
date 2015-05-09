using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour {

	public bool plane = true;

	protected enum STATE {FOCUSED, FREE, DISABLED};
	protected STATE state = STATE.FREE;
	protected  float planey = 0;

	public virtual void endMove(){
		state = STATE.FREE;
	}

	public virtual void place(Vector2 screenPos, bool focused = true){
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (screenPos.x, screenPos.y, 0));
		planey = 0;
		float t = (this.planey - ray.origin.y) / ray.direction.y;
		Vector3 translation = new Vector3(ray.origin.x + t * ray.direction.x - this.transform.position.x,
		                                  0,
		                                  ray.origin.z + t * ray.direction.z - this.transform.position.z);
		this.transform.Translate(translation, Space.World);
	}

	public virtual void move(Ray ray, RaycastHit hit){

		switch (state) {
		case STATE.DISABLED:
			break;
		case STATE.FOCUSED:
			float t = (this.planey - ray.origin.y) / ray.direction.y;
			Vector3 translation = new Vector3(ray.origin.x + t * ray.direction.x - this.transform.position.x,
			                                  0,
			                                  ray.origin.z + t * ray.direction.z - this.transform.position.z);
			this.transform.Translate(translation, Space.World);

			break;
		case STATE.FREE:
			state = STATE.FOCUSED;
			this.planey = hit.point.y;
			break;
		}


	}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
