using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public bool indirectLight = false;

	// Use this for initialization
	void Start () {
		if (indirectLight) {

			GameObject lightObj = new GameObject ("Indirect");

			lightObj.transform.SetParent (this.transform);
			lightObj.transform.forward = -this.transform.forward;
			lightObj.transform.Rotate (25, 0, 0);

			Light lightComp = lightObj.AddComponent<Light> ();

			lightComp.color = GetComponent<Renderer> ().material.color;
			lightComp.bounceIntensity = 0;
			lightComp.intensity = 0.1f;
			lightComp.type = LightType.Directional;

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (indirectLight) {
			Light lightComp = GetComponentInChildren<Light> ();
			if (lightComp != null)
				lightComp.color = GetComponent<Renderer> ().material.color;
		}
	}
}
