using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {

	public interface IZoomCameraListener{
		void onZoomRevertEnd (ZoomCamera sender);
	}

	public IZoomCameraListener listener = null;

	private float animationPosition = 0;
	public float animationTime = 0.3f;
	public float zoomFov = 15.0f;

	public Camera camera;

	private Vector3 directionSnapshot;
	private float fovSnapshot;

	private Vector3 directionTarget;
	private float fovTarget;

	public void focusOnObject(Transform target){
		if (!reverted)
			revert ();
		reverted = false;

		directionSnapshot = new Vector3 (this.camera.transform.forward.x,
		                                this.camera.transform.forward.y,
		                                this.camera.transform.forward.z);
		fovSnapshot = this.camera.fieldOfView;

		Vector3 pos = target.position + new Vector3 (0, 0.3f, 0);

		directionTarget = (pos - this.camera.transform.position).normalized;
		fovTarget = zoomFov;

		animationPosition = 0;
	}

	bool reverted = false;
	public void revert(){
		if (reverted)
			return;

		directionTarget = directionSnapshot;
		fovTarget = fovSnapshot;

		directionSnapshot = new Vector3 (this.camera.transform.forward.x,
		                                 this.camera.transform.forward.y,
		                                 this.camera.transform.forward.z);
		fovSnapshot = this.camera.fieldOfView;

		animationPosition = 0;

		reverted = true;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		animationPosition += Time.deltaTime / animationTime;
		animationPosition = Mathf.Clamp(animationPosition, 0.0f, 1.0f);
		this.camera.transform.forward = Vector3.Lerp (this.directionSnapshot, this.directionTarget, animationPosition);
		this.camera.fieldOfView = Mathf.Lerp(this.fovSnapshot, this.fovTarget, animationPosition);

		if (animationPosition >= 1.0f && reverted) {
			reverted = false;
			//this.camera.GetComponent<TouchyCamera>().onZoomRevertEnd();
			if (listener != null)
				listener.onZoomRevertEnd(this);
		}
	}
}
