using UnityEngine;
using System.Collections;
using System.IO;

public class RoomConcretizer {
	
	private static PaletteCatalog cat = null;
	private static FurniturePicker furPicker = null;
	
	public static string pickConcreteFurnitures(string xmlRoom){
		
		if (cat == null)
			cat = PaletteCatalog.Load ("Palettes/paletteCatalog");
		
		if (furPicker == null)
			furPicker = new FurniturePicker ();
		furPicker.SetPalettePicker (new RandomPalettePicker (cat));
		return furPicker.ConcretizeRoom (xmlRoom);
	}
	
	public static void refurnish(Room room){
		
		if (cat == null)
			cat = PaletteCatalog.Load ("Palettes/paletteCatalog");
		
		if (furPicker == null)
			furPicker = new FurniturePicker ();
		furPicker.SetPalettePicker (new RandomPalettePicker (cat));
		furPicker.ConcretizeRoom (room);
	}
}
