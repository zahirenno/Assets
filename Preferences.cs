using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Preferences : MonoBehaviour
{
	public Color darkBackColor;
	public Color primaryColor;
	public Color secondaryColor;
	public Color primaryTextColor;
	public Color primaryListTextColor;
	public Color secondaryListTextColor;
	public Color backgroundColor;
	public Font primaryTextFont;
	public Font secondaryTextFont;

	public Dictionary<string, Color> preDefColors = new Dictionary<string, Color>();

	void Awake(){
		Debug.Log ("pref");
		
		preDefColors.Add ("Red", colorFromHex ("#F44336"));
		preDefColors.Add ("Pink", colorFromHex ("#E91E63"));
		preDefColors.Add ("Purple", colorFromHex ("#9c27b0"));
		preDefColors.Add ("Deep Purple", colorFromHex ("#673AB7"));
		preDefColors.Add ("Indigo", colorFromHex ("#3F51B5"));
		preDefColors.Add ("Blue", colorFromHex ("#2196F3"));
		preDefColors.Add ("Light Blue", colorFromHex ("#03A9F4"));
		preDefColors.Add ("Cyan", colorFromHex ("#00BCD4"));
		preDefColors.Add ("Teal", colorFromHex ("#009688"));
		preDefColors.Add ("Green", colorFromHex ("#4CAF50"));
		preDefColors.Add ("Light Green", colorFromHex ("#8BC34A"));
		preDefColors.Add ("Lime", colorFromHex ("#CDDC39"));
		preDefColors.Add ("Yellow", colorFromHex ("#FFEB3B"));
		preDefColors.Add ("Amber", colorFromHex ("#FFC107"));
		preDefColors.Add ("Orange", colorFromHex ("#FF9800"));
		preDefColors.Add ("Deep Orange", colorFromHex ("#FF5722"));
		preDefColors.Add ("Brown", colorFromHex ("#795548"));
		preDefColors.Add ("Grey", colorFromHex ("#9E9E9E"));
		preDefColors.Add ("Blue Grey", colorFromHex ("#607D8B"));
		preDefColors.Add ("Black", colorFromHex ("#000000"));
		
		primaryColor = preDefColors ["Blue Grey"];
		secondaryColor = preDefColors ["Deep Orange"];
		primaryTextColor = colorFromHex("#FFFFFF");
		primaryListTextColor = colorFromHex ("#000000");
		secondaryListTextColor = Color.gray;
		darkBackColor = colorFromHex ("#222222");

		backgroundColor = Color.gray;
		primaryTextFont = Resources.Load<Font> ("Fonts/Roboto-Regular");
		secondaryTextFont = Resources.Load<Font> ("Fonts/Roboto-Light");
	}

	void Start(){

	}

	private int hexByteToInt(string hexByte){
		char[] hh = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
		List<char> h = new List<char>(hh);
		return h.IndexOf (hexByte [0]) * 16 + h.IndexOf (hexByte [1]);
	}

	private float hexByteToFloat(string hexByte){
		return (float)hexByteToInt (hexByte) / 255.0f;
	}

	private Color colorFromHex(string hex){
		string r_hex = hex.Substring (1, 2);
		string g_hex = hex.Substring (3, 2);
		string b_hex = hex.Substring (5, 2);
		string a_hex = "FF";
		if (hex.Length > 8)
			a_hex = hex.Substring (7, 2);

		return new Color (hexByteToFloat (r_hex),
		                 hexByteToFloat (g_hex),
		                 hexByteToFloat (b_hex),
		                 hexByteToFloat (a_hex));
	}
}

