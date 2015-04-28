using UnityEngine;
using System.Collections;

public class DeleteRoomAnimation : MonoBehaviour
{

	// Use this for initialization
	public Vector3 endScale;
	public Vector3 startScale;
	private float startTime;
	public float totalTime=0.5F;
	void Start ()
	{
		startTime = Time.time;
		startScale =  gameObject.transform.localScale;
		endScale =new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float frac = (Time.time - startTime) / totalTime;
		gameObject.transform.localScale = Vector3.Lerp (startScale, endScale, frac);
		if (frac > 1.0) {
			GameObject.Destroy(this.gameObject);
		}
	}
}

