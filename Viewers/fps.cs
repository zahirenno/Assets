using UnityEngine; 
using System.Collections;

public class fps : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}

		Vector3 dirF = new Vector3 (this.transform.forward.x, 0, this.transform.forward.z);
		Vector3 dirR = new Vector3 (this.transform.right.x, 0, this.transform.right.z);

		float speed = 5;
		if (Input.GetKey (KeyCode.W)) {
			this.transform.Translate (Time.deltaTime * speed * dirF, Space.World);
		} else if (Input.GetKey (KeyCode.S)) {
			this.transform.Translate (-Time.deltaTime * speed * dirF, Space.World);
		} 

		if (Input.GetKey (KeyCode.A)) {
			this.transform.Translate (-Time.deltaTime * speed * dirR, Space.World);
		} else if (Input.GetKey (KeyCode.D)) {
			this.transform.Translate (Time.deltaTime * speed * dirR, Space.World);
		}
	}
	void Start ()
	{
	}
}