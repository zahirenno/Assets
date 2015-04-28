using UnityEngine;
using System.Collections;

public class SwitchRoomAnimation : MonoBehaviour
{

	// Use this for initialization
	
	public bool keepOn = true;
	public Vector3 start;
	public Vector3 end;
	public float speed = 20.0F;
	private float startTime;
	private float journeyLength;

	void Start ()
	{
		gameObject.transform.position = start;
		startTime = Time.time;
		journeyLength = Vector3.Distance (start, end);
		gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float distCovered = (Time.time - startTime) * speed;
		float frac = distCovered / journeyLength;
		if (frac > 1.0F) {
			gameObject.SetActive(keepOn);
			Component.Destroy(this);
		}
		gameObject.transform.position = Vector3.Lerp (start, end, frac);
	}
}

