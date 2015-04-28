using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Highlightable : MonoBehaviour {

	public Material highlightedMaterial;
	public bool highlighted = false;

	private Dictionary<MeshRenderer, Material> mrend = new Dictionary<MeshRenderer, Material>();
	private bool oldHighLight = false;

	// Use this for initialization
	void Start () {
		if (highlightedMaterial == null) {
			highlightedMaterial = Resources.Load<Material>("highlighted_object");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (highlighted != oldHighLight) {
			if (highlighted){
				mrend.Clear();
				MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer renderer in renderers){
					mrend.Add(renderer, renderer.material);
					renderer.material = highlightedMaterial;
				}

				renderers = GetComponents<MeshRenderer>();
				foreach (MeshRenderer renderer in renderers){
					if (!mrend.ContainsKey(renderer)){
						mrend.Add(renderer, renderer.material);
						renderer.material = highlightedMaterial;
					}
				}

			}else{

				foreach (KeyValuePair<MeshRenderer, Material> kv in mrend)
					kv.Key.material = kv.Value;

			}
			oldHighLight = highlighted;
		}
	}
}
