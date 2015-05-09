using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class FurniturePreviewCellView : MonoBehaviour, IPointerClickHandler
{

	
	public void OnPointerClick(PointerEventData eventData)
	{
	}

	private FurnitureEntry _furnitureEntry;
	public FurnitureEntry furnitureEntry{
		get { return _furnitureEntry;}
		set { _furnitureEntry = value; updateData();}
	}

	public Text furnitureName;
	public Image previewImage;
	public Image heartImage;
	public Button addToWishListButton;
	public Button cellButton;

	private Color toHSV(Color color){
		Color result = Color.black;

		float min = Mathf.Min (color.r, color.g, color.b);
		float max = Mathf.Max (color.r, color.g, color.b);
		float delta_max = max - min;

		result.b = max;

		if (delta_max == 0) {
			result.r = 0;
			result.g = 0;
		} else {
			result.g = delta_max / max;

			float delta_r = (((max - color.r) / 6.0f) + (delta_max / 2.0f)) / delta_max;
			float delta_g = (((max - color.g) / 6.0f) + (delta_max / 2.0f)) / delta_max;
			float delta_b = (((max - color.b) / 6.0f) + (delta_max / 2.0f)) / delta_max;

			if (color.r == max)
				result.r = delta_b - delta_g;
			else if (color.g == max)
				result.r = (1.0f / 3.0f) + delta_r - delta_b;
			else if (color.b == max)
				result.r = (1.0f / 3.0f) + delta_g - delta_r;

			if (color.r < 0)
				color.r += 1.0f;
			if (color.r > 1)
				color.r -= 1.0f;
		}

		return result;
	}

	private Color toRGB(Color color){
		Color result = Color.black;

		if (color.g == 0) {
			result.r = color.b;
			result.g = color.b;
			result.b = color.b;
		} else {
			float var_h = color.r * 6.0f;
			if (var_h == 6.0f)
				var_h = 0;
			float var_i = Mathf.Floor(var_h);
			float var_1 = color.b * (1.0f - color.g);
			float var_2 = color.b * (1.0f - color.g * (var_h - var_i));
			float var_3 = color.b * (1.0f - color.g * (1.0f - (var_h - var_i)));

			int i = (int)var_i;
			switch (i){
			case 0:
				result.r = color.b;
				result.g = var_3;
				result.b = var_1;
				break;
			case 1:
				result.r = var_2;
				result.g = color.b;
				result.b = var_1;
				break;
			case 2:
				result.r = var_1;
				result.g = color.b;
				result.b = var_3;
				break;
			case 3:
				result.r = var_1;
				result.g = var_2;
				result.b = color.b;
				break;
			case 4:
				result.r = var_3;
				result.g = var_1;
				result.b = color.b;
				break;
			default:
				result.r = color.b;
				result.g = var_1;
				result.b = var_2;
				break;
			}
		}

		return result;
	}

	public void updateData(){
		furnitureName.text = furnitureEntry.name;
		previewImage.sprite = Resources.Load<Sprite> ("Thumbnails/" + furnitureEntry.id);

		Color c = Color.black;
		if (previewImage.sprite != null) {
			int stepSize = 50;
			float ct = 0;

			int padding = 50;
			Vector2 center = new Vector2(previewImage.sprite.texture.width / 2.0f, previewImage.sprite.texture.height / 2.0f);

			for (int x = padding; x < previewImage.sprite.texture.width - padding; x += stepSize) {
				for (int y = padding; y < previewImage.sprite.texture.height - padding; y += stepSize) {
					Color pix = previewImage.sprite.texture.GetPixel (x, y);

					float d = Vector2.Distance(center, new Vector2(x,y));
					float v = Mathf.Exp(-d / 50.0f);

					c += pix * pix.a * v;
					ct += pix.a * v;
				}
			}

			c /= ct;
		}

		Color h1 = toHSV (c);
		h1.g += 0.3f;
		h1.b -= 0.1f;
		c = toRGB (h1);
		transform.FindChild ("Panel").GetComponent<Image> ().color = c;

		Color h = toHSV (c);

		h.r += 0.5f;
		/*if (h.r > 1.0f)
			h.r -= 1.0f;
		if (h.r < 0)
			h.r += 1.0f;*/

		//h.b = 1.0f - h.b;
		if (h.b >= 0.8f) 
			h.b = 0.5f;
		else 
			h.b = 1;
		furnitureName.color = toRGB (h) /*Color.white - c + Color.black*/;
		heartImage.color = furnitureName.color;
		heartImage.color -= new Color (0, 0, 0, 0.3f);
		addToWishListButton.gameObject.GetComponent<Image> ().color = furnitureName.color;
	}

	public void init(){
		furnitureName = transform.FindChild ("Panel").FindChild ("FurnitureName").GetComponent<Text> ();
		previewImage = transform.FindChild ("PreviewImage").GetComponent<Image> ();
		cellButton = transform.GetComponent<Button> ();
		addToWishListButton = transform.FindChild ("Panel").FindChild ("AddToWishListButton").GetComponent<Button> ();
		addToWishListButton.onClick.AddListener(()=>addToWishList());
		heartImage = transform.FindChild ("Heart").GetComponent<Image> ();
		heartImage.rectTransform.sizeDelta = new Vector2 (0, 0);
	}

	public float heartingAnimationTime = 0.5f;
	private bool hearting = false;
	private float heartingAnimationSpeed = 0;
	private float heartingAnimationTarget = 0;
	public void addToWishList(){
		heartingAnimationTarget = ((RectTransform)transform).sizeDelta.x * 2.0f;
		heartingAnimationSpeed = heartingAnimationTarget / heartingAnimationTime;
		heartImage.rectTransform.sizeDelta = new Vector2 (0, 0);
		hearting = true;
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (hearting) {
			heartImage.rectTransform.sizeDelta += new Vector2(heartingAnimationSpeed * Time.deltaTime, 
			                                                  heartingAnimationSpeed * Time.deltaTime);
			if (heartImage.rectTransform.sizeDelta.x > heartingAnimationTarget){
				hearting = false;
				heartImage.rectTransform.sizeDelta = new Vector2(0,0);
			}
		}
	}
}

