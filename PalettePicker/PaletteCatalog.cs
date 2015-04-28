using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

class PaletteCatalog
{
    private Dictionary<int, Palette> palettes = new Dictionary<int, Palette>();

    private HashSet<int> keys = new HashSet<int>();
    public Palette GetPalette(int paletteId) { return palettes[paletteId]; }

    public HashSet<int> GetPalettes() { return keys; }

    public void AddPalette(Palette palette)
    {
        palettes.Add(palette.getId(), palette);
        keys.Add(palette.getId());
    }

    public static PaletteCatalog Load(string catalogPath)
    {
        PaletteCatalog cat = new PaletteCatalog();
        //StreamReader reader = File.OpenText(catalogPath);
		TextAsset content = Resources.Load<TextAsset> (catalogPath);
		StringReader reader = new StringReader (content.text);

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Contains("id"))
            {
                string[] items = line.Split('\t');
                int id = int.Parse(items[1]);
                Palette p = new Palette(id);
                p.setName(items[2]);
                cat.AddPalette(p);
            }
        }
        return cat;
    }
}

