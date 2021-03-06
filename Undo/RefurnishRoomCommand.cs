//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class RefurnishRoomCommand : Command
{
	List<Vector3> positions;
	List<Quaternion> rotations;
	List<string> furnitureIds;
	List<string> furnitureNames;

	public RefurnishRoomCommand (Room oldRoom): base (oldRoom)
	{
		positions = new List<Vector3> ();
		rotations=new List<Quaternion>();
		furnitureIds = new List<string> ();
		furnitureNames = new List<string> ();

		foreach (Furniture f in oldRoom.furnitures) {
			positions.Add (f.gameObject.transform.localPosition);
			rotations.Add(f.gameObject.transform.localRotation);
			furnitureIds.Add (f.getFurnitureType().id);
			furnitureNames.Add (f.gameObject.name);
		}
	}
	public override void Undo ()
	{

		while (room.furnitures.Count>0) {
			room.removeFurniture(room.furnitures[0]);
		}
		for(int i=0;i< furnitureIds.Count;++i){
			GameObject f=Catalog.getCatalog().createFurniture(furnitureIds[i],positions[i],0);
			f.transform.parent = room.gameObject.transform;
			f.transform.localPosition=positions[i];
			f.transform.localRotation=rotations[i];
			f.name=furnitureNames[i];
			room.addFurniture(f.GetComponent<Furniture>());
		}
	}
	public override void Execute ()
	{
		RoomConcretizer.refurnish (room);
	}
}


