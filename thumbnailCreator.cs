using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class thumbnailCreator : MonoBehaviour {

	public ReflectionProbe probe;

	Catalog catalog;

	// Use this for initialization
	void Start () {
		catalog = Catalog.getCatalog ();

		ids.Clear ();
		ids.AddRange (catalog.entries.Values);

	}

	GameObject go = null;
	List<FurnitureEntry> ids = new List<FurnitureEntry>();
	int i = 0;

	// Update is called once per frame
	void Update () {
		if (go != null)
			GameObject.Destroy (go);
		
		if (i < ids.Count) {
			go = catalog.createFurniture(ids[i].id, new Vector3(0,0,0), 160.0f);

			//Camera.main.transform.LookAt(go.transform.position);
			Camera.main.fieldOfView = Mathf.Asin(Mathf.Max (ids[i].width, ids[i].height, ids[i].depth) / 2.0f / 3.0f) * 2.0f * 180.0f / 3.14f;

			++i;
			probe.RenderProbe();
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination){
		destination = source;

		Texture2D output = new Texture2D (source.width, source.height, TextureFormat.ARGB32, false);
		output.ReadPixels (new Rect (0, 0, source.width, source.height), 0, 0);
		byte[] data = output.EncodeToPNG ();

		System.IO.File.WriteAllBytes ("c:\\Users\\Zahi Renno\\Pictures\\sh\\" + ids [i-1].id + ".png", data);
	}
}
