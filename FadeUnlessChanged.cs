using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeUnlessChanged : MonoBehaviour {

	public float animationTime = 1.0f;
	public float waitTime = 2.0f;

	private string oldValue = "";
	private float alpha;
	private Text textBox;
	private float time = 0;
	private float timeW = 0;
	private float targetAlpha = 0;

	private enum STATE {FADE, WAIT, STANDBY};
	private STATE state = STATE.WAIT;

	// Use this for initialization
	void Start () {
		textBox = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {


		switch (state) {
		case STATE.FADE:
			if (!oldValue.Equals (textBox.text)) {
				state = STATE.STANDBY;
			}
			time += Time.deltaTime;
			if (time > animationTime){
				if (targetAlpha == 1.0f)
					state = STATE.WAIT;
				else 
					state = STATE.STANDBY;
				time = animationTime;
			}
			
			textBox.color = new Color (textBox.color.r, 
			                           textBox.color.g, 
			                           textBox.color.b, 
			                           Mathf.Lerp (1.0f - targetAlpha, targetAlpha, time / animationTime));

			break;
		case STATE.WAIT:
			if (!oldValue.Equals (textBox.text)) {
				timeW = 0;
				oldValue = textBox.text;
			}
			timeW += Time.deltaTime;
			if (timeW > waitTime) {
				targetAlpha = 0;
				time = ((1.0f - Mathf.Abs(targetAlpha - textBox.color.a)) / 1.0f) / animationTime;
				state = STATE.FADE;
			}

			break;
		case STATE.STANDBY:
			if (!oldValue.Equals (textBox.text)) {
				targetAlpha = 1.0f;
				time = ((1.0f - Mathf.Abs(targetAlpha - textBox.color.a)) / 1.0f) / animationTime;
				oldValue = textBox.text;
				timeW = 0;
				
				state = STATE.FADE;
			}
			break;
		}
	
	}
}
