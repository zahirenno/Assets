using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class Catalog {

	private static Catalog catalog = null;

	public static Catalog getCatalog(){
		if (catalog == null) {
			catalog = new Catalog();
			catalog.loadCatalog();
		}
		return catalog;
	}

	public float scale = 0.01f;
	public Dictionary<string, FurnitureEntry> entries = new Dictionary<string, FurnitureEntry>();
	public Dictionary<int, FurnitureEntry> entriesById = new Dictionary<int, FurnitureEntry>();
	public Dictionary<string, List<FurnitureEntry>> entriesByCategory = new Dictionary<string, List<FurnitureEntry>> ();

	public static Quaternion QuaternionFromMatrix(float[,] m) {
		return Quaternion.LookRotation (new Vector3 (m [2, 0], m [2, 1], m [2, 2]), 
		                                new Vector3 (m [1, 0], m [1, 1], m [1, 2]));
	}

	public GameObject createFurniture(string id, Vector3 position, float angle, bool inRad = false){
		float _angle = angle;
		if (inRad)
			_angle *= 180.0f / (float)Math.PI;
		
		//Debug.Log ("Assets/Models/" + entries [id].model.Replace (".obj", ".fbx"));

		//Debug.Log (id);
		//Debug.Log (entries [id].model.Replace(".obj", ""));
		GameObject mo = UnityEngine.Resources.Load<GameObject>(entries [id].model.Replace(".obj", ""));

		//GameObject mo = Resources.LoadAssetAtPath<GameObject>("Assets/Models/armchair.fbx");
		
		if (mo != null) {
			
			//use model specific rotation matrix
			Quaternion q = QuaternionFromMatrix(entries[id].modelRotation);
			GameObject instmo = (GameObject)GameObject.Instantiate (mo, position, q);
			instmo.AddComponent<MovableObject>();
			instmo.AddComponent<Highlightable>();
			instmo.AddComponent<Furniture>().setFurnitureType(entries[id]);



			foreach (MeshFilter meshFilter in instmo.GetComponentsInChildren<MeshFilter>()){
				if (meshFilter.mesh.vertexCount > 3){ //TODO Bad workaround for degenerate triangles that cause the MeshCollider to crash
					MeshCollider mc = instmo.AddComponent<MeshCollider>();
					mc.sharedMesh = meshFilter.sharedMesh;
				}
			}
			
			Bounds b = new Bounds();
			b.max = new Vector3(-1000,-1000,-1000);
			b.min = new Vector3(1000,1000,1000);
			
			foreach (MeshFilter submeshFilter in instmo.GetComponentsInChildren<MeshFilter>()){
				submeshFilter.gameObject.transform.localScale = new Vector3(1,1,1);

				Mesh submesh = submeshFilter.mesh;
				
				Bounds sbounds = submesh.bounds;
				
				Vector3 tmax = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
				Vector3 tmin = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
				
				tmax.x = Mathf.Max(b.max.x, sbounds.max.x);
				tmax.y = Mathf.Max(b.max.y, sbounds.max.y);
				tmax.z = Mathf.Max(b.max.z, sbounds.max.z);
				
				tmin.x = Mathf.Min(b.min.x, sbounds.min.x);
				tmin.y = Mathf.Min(b.min.y, sbounds.min.y);
				tmin.z = Mathf.Min(b.min.z, sbounds.min.z);
				
				b.max = tmax;
				b.min = tmin;
			}
			
			Vector3 dims = new Vector3((b.max.x - b.min.x),(b.max.y - b.min.y),(b.max.z - b.min.z));

			//Debug.Log(dims);

			dims = instmo.transform.TransformVector(dims);
			
			float width = entries[id].width, height = entries[id].height, depth = entries[id].depth;
			
			instmo.transform.localScale = new Vector3(
				width / Mathf.Abs(dims.x), 
				height / Mathf.Abs(dims.y), 
				depth / Mathf.Abs(dims.z));
			
			Vector3 bmin = instmo.transform.TransformVector(b.min);
			
			//don't mess with this
			instmo.transform.Rotate(0, -_angle + 2.0f * instmo.transform.eulerAngles.y, 0, Space.World);
			instmo.transform.Translate(0, -instmo.transform.position.y - bmin.y, 0, Space.World);

			instmo.name=id+System.Guid.NewGuid().ToString();
			return instmo;

		}
		return null;
	}
	

	private void loadToDict(string filename, Dictionary<int, FurnitureEntry> entries){
		TextAsset content = Resources.Load<TextAsset> (filename);
		StringReader reader = new StringReader (content.text);

		string s = reader.ReadLine ();
		while (s != null) {
			try {
				string[] fields = s.Split('=');
				if (fields.Length == 2){
					string[] field_id = fields[0].Split('#');
					
					string tagname = field_id[0];
					int code = Convert.ToInt32(field_id[1]);
					string value = fields[1];
					
					if (tagname.Equals("id")){
						FurnitureEntry entry = new FurnitureEntry();
						entry.id = value;
						entries.Add(code, entry);
					}else if (tagname.Equals("name")){
						entries[code].name = value;
					}else if (tagname.Equals("icon")){
						string[] p = value.Split('/');	
						entries[code].image = p[p.Length - 1].Split('.')[0];
					}else if (tagname.Equals("category")) {
						entries[code].category = value;
					}else if (tagname.Equals("depth")){
						entries[code].depth = (float)Convert.ToDouble(value) * scale;
					}else if (tagname.Equals("width")){
						entries[code].width = (float)Convert.ToDouble(value) * scale;
					}else if (tagname.Equals("height")){
						entries[code].height = (float)Convert.ToDouble(value) * scale;
					}else if (tagname.Equals("model")){
						//string[] path = value.Split('/');
						//entries[code].model = path[path.Length - 1];
						entries[code].model = value;
					}else if (tagname.Equals("modelRotation")){
						string[] values = value.Split(' ');
						
						entries[code].modelRotation[0, 0] = (float)Convert.ToDouble(values[0]);
						entries[code].modelRotation[1, 0] = (float)Convert.ToDouble(values[1]);
						entries[code].modelRotation[2, 0] = (float)Convert.ToDouble(values[2]);
						
						entries[code].modelRotation[0, 1] = (float)Convert.ToDouble(values[3]);
						entries[code].modelRotation[1, 1] = (float)Convert.ToDouble(values[4]);
						entries[code].modelRotation[2, 1] = (float)Convert.ToDouble(values[5]);
						
						entries[code].modelRotation[0, 2] = (float)Convert.ToDouble(values[6]);
						entries[code].modelRotation[1, 2] = (float)Convert.ToDouble(values[7]);
						entries[code].modelRotation[2, 2] = (float)Convert.ToDouble(values[8]);
					}
				}
			}catch (Exception e) {
				Debug.Log(e);		
			}finally {
				s = reader.ReadLine();
			}
		}
	}

	private void loadCatalog(){
		loadToDict ("ContributedFurnitureCatalog", entriesById);
		foreach (KeyValuePair<int, FurnitureEntry> entry in entriesById) {
			this.entries.Add (entry.Value.id, entry.Value);

			if (!entriesByCategory.ContainsKey(entry.Value.category))
				entriesByCategory[entry.Value.category] = new List<FurnitureEntry>();
			entriesByCategory[entry.Value.category].Add(entry.Value);
		}
	}
	
}
