using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class RandomPalettePicker : PalettePicker
{
    PaletteCatalog paletteCatalog;
    public RandomPalettePicker(PaletteCatalog paletteCatalog)
    {
        this.paletteCatalog = paletteCatalog;
    }

    public Palette Pick()
    {
        int s = paletteCatalog.GetPalettes().ElementAt(
            RandomHelper.nextInt(paletteCatalog.GetPalettes().Count));
        return paletteCatalog.GetPalette(s);
    }
}

