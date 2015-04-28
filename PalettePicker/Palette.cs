using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Palette
{
    string name;
    HashSet<string> colors = new HashSet<string>();
    int id;

    public HashSet<string> getColors() { return colors; }
    public string getName() { return name; }
    public void setName(string name) { this.name=name; }
    public int getId() { return id; }

    public Palette(int id)
    {
        this.id = id;
        colors.Add("B1");
        colors.Add("B2");
        colors.Add("C1");
        colors.Add("C2");
        colors.Add("D1");
        colors.Add("D2");
    }

    
}

