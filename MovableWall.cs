using UnityEngine;
using System.Collections;

public class MovableWall : MovableObject {


	
	public override void move(Ray ray, RaycastHit hit){

		switch (state) {
		case MovableObject.STATE.DISABLED:
			break;
		case MovableObject.STATE.FOCUSED:
			
			GameObject target = this.transform.gameObject;
			
			if (target != null) {
				float t = -ray.origin.y / ray.direction.y;
				
				Vector3 pos = new Vector3 (ray.origin.x + t * ray.direction.x,
				                           0,
				                           ray.origin.z + t * ray.direction.z);
				Vector3 transl = new Vector3 (0, 0, 0);
				
				Vector3 mvDir = this.transform.forward;
				if (this.name.Equals ("N"))
					mvDir = this.transform.parent.forward;
				else if (this.name.Equals ("S"))
					mvDir = -this.transform.parent.forward;
				else if (this.name.Equals ("W"))
					mvDir = -this.transform.parent.right;
				else
					mvDir = this.transform.parent.right;
				
				float z0 = ray.origin.z;
				float z1 = target.transform.position.z;
				float x0 = ray.origin.x;
				float x1 = target.transform.position.x;
				float a = ray.direction.x;
				float b = ray.direction.z;
				float c = mvDir.x;
				float d = mvDir.z;
				
				float u = (d * (x1 - x0) + (z0 - z1) * c) / ((d * a) - (b * c));
				float x = x0 + u * a;
				float z = z0 + u * b;
				
				transl = new Vector3(x - target.transform.position.x,
				                     0,
				                     z - target.transform.position.z);
				
				
				target.transform.Translate (transl, Space.World);

				//this.transform.parent.GetComponent<EditableRoom>().updateXYWH();
			}
			break;
		case MovableObject.STATE.FREE:
			state = MovableObject.STATE.FOCUSED;
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
