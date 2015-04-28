using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ZoomSlider : MonoBehaviour {


	void setAplha(float a){
		List<Image> images = new List<Image> ();

		images.Add(transform.FindChild ("Background").GetComponent<Image>());
		images.Add(transform.FindChild ("Fill Area").FindChild ("Fill").GetComponent<Image> ());
		images.Add(transform.FindChild ("Handle Slide Area").FindChild ("Handle").GetComponent<Image> ());

		foreach (Image image in images) { 
			image.color = new Color (image.color.r, image.color.g, image.color.b, a);
		}
	}

	void onPointerDown (float v){
		targetAlpha = 1.0f;

		Camera.main.fieldOfView = 60.0f - (v - 0.5f) * 90.0f; 
	}

	// Use this for initialization
	void Start () {
		GetComponent<Slider> ().onValueChanged.AddListener ((float v) => onPointerDown (v));
	}

	public float animationTime = 0.5f;
	private float targetAlpha = 0;
	private float currentAlpha = 0;
	// Update is called once per frame
	void Update () {

		if (targetAlpha > currentAlpha)
			currentAlpha += (Time.deltaTime / animationTime);
		else if (targetAlpha < currentAlpha)
			currentAlpha -= (Time.deltaTime / animationTime);
		currentAlpha = Mathf.Clamp (currentAlpha, 0, 1);

		setAplha (currentAlpha);

		targetAlpha = 0;

	}
}
