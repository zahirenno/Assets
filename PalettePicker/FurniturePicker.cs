using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;


class FurniturePicker
{
	protected PalettePicker palettePicker;
	protected FurnituresForPaletteColor sofa1pc;
	protected FurnituresForPaletteColor sofa2pc;
	
	
	protected string SOFA2 ="sofa2";
	protected string SOFA1 ="sofa1";
	
	protected string SOFA2_ABSTRACT = "(Renouzate#sofa2)";
	protected string SOFA1_ABSTRACT = "(Renouzate#sofa1)";
	
	protected string SOFA2_CONCRETE = "(sofa2#[0-9]*)";
	protected string SOFA1_CONCRETE = "(sofa1#[0-9]*)";
	
	public void SetPalettePicker(PalettePicker palettePicker)
	{
		this.palettePicker = palettePicker;
		sofa1pc = FurnituresForPaletteColor.Load("Palettes/sofa1Rep");
		sofa2pc = FurnituresForPaletteColor.Load("Palettes/sofa2Rep");
	}
	
	public string ConcretizeRoom(string xmlRoom)
	{
		Palette p = palettePicker.Pick();
		
		string[] separators = new string[] { "<Furniture " };
		string[] splits = xmlRoom.Split(separators, StringSplitOptions.RemoveEmptyEntries);
		string result = splits[0];
		for (int i = 1; i < splits.Length; ++i)
		{
			result += separators[0];
			string color = p.getColors().ElementAt(RandomHelper.nextInt(p.getColors().Count));
			
			if (splits[i].Contains(SOFA1))
			{
				List<string> furlist = sofa1pc.GetPossibleFurnituresFor(p, color);
				string furId = furlist.ElementAt(RandomHelper.nextInt(furlist.Count));
				Regex rgx=new Regex(SOFA1_ABSTRACT+"|"+SOFA1_CONCRETE);
				result += rgx.Replace(splits[i], furId);
			}
			else if (splits[i].Contains(SOFA2))
			{
				List<string> furlist = sofa2pc.GetPossibleFurnituresFor(p, color);
				string furId = furlist.ElementAt(RandomHelper.nextInt(furlist.Count));
				Regex rgx=new Regex(SOFA2_ABSTRACT+"|"+SOFA2_CONCRETE);
				result += rgx.Replace(splits[i], furId);
			}
			else
			{
				result += splits[i];
			}
		}
		return result;
	}
	
	public void ConcretizeRoom(Room room){
		Palette p = palettePicker.Pick ();
		for(int i=0;i<room.furnitures.Count;++i){
			GameObject nf;
			Furniture f=room.furnitures[i];
			if(f.getFurnitureType().id.Contains(SOFA1)){
				nf=Catalog.getCatalog().createFurniture(getNewFurnitureId(sofa1pc,p),
				                                        new Vector3(0,0,0),0);
			}else if(f.getFurnitureType().id.Contains(SOFA2)){
				nf=Catalog.getCatalog().createFurniture(getNewFurnitureId(sofa2pc,p),
				                                        new Vector3(0,0,0),0);
			}else{
				nf=Catalog.getCatalog().createFurniture(f.getFurnitureType().id,
				                                        new Vector3(0,0,0),0);
			}
			nf.transform.parent = room.gameObject.transform;
			room.swapFurniture(f,nf.GetComponent<Furniture>());
		}
		
		
	}
	private string getNewFurnitureId(FurnituresForPaletteColor fpc,Palette p){
		string color = p.getColors().ElementAt(RandomHelper.nextInt(p.getColors().Count));
		List<string> furlist = fpc.GetPossibleFurnituresFor(p, color);
		return furlist.ElementAt(RandomHelper.nextInt(furlist.Count));
	}
}

