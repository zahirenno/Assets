using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

class FurnituresForPaletteColor
{
    Dictionary<PaletteColor, List<string>> furnituresForPaletteColor = new Dictionary<PaletteColor, List<string>>();

    public List<string> GetPossibleFurnituresFor(Palette palette, string color)
    {
        return furnituresForPaletteColor[new PaletteColor(palette, color)];
    }

    public static FurnituresForPaletteColor Load(string path)
    {
        //StreamReader reader = File.OpenText(path);
		TextAsset content = Resources.Load<TextAsset> (path);
		StringReader reader = new StringReader (content.text);

        if (reader == null)
            return null;
        FurnituresForPaletteColor fpc = new FurnituresForPaletteColor();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Contains("id"))
            {
                string[] items = line.Split('\t');
                int id = int.Parse(items[1]);
                Palette palette = new Palette(id);
                palette.setName(items[2]);
                string data;
                while ((data = reader.ReadLine()) != null)
                {
                    if (data == "") { break; }
                    items = data.Split('=');
                    List<string> furCatIds = new List<string>();
                    string[] furs = items[1].Split('\t');
                    for (int i = 0; i < furs.Length; ++i)
                    {
                        furCatIds.Add(furs[i]);
                    }
                    fpc.furnituresForPaletteColor.Add(
                        new PaletteColor(palette, items[0]), furCatIds);
                }
            }
        }
        return fpc;
    }





    public class PaletteColor
    {
        Palette palette;
        string color;

        public PaletteColor(Palette palette, string color)
        {
            this.palette = palette;
            this.color = color;
        }
        public override bool Equals(object obj)
        {
            PaletteColor other = (PaletteColor)obj;
            return palette.getId() == other.palette.getId() &&
                color.Equals(other.color);
        }
        public override int GetHashCode()
        {
            return palette.getId() + 23 * color.GetHashCode();
        }
    }

}

