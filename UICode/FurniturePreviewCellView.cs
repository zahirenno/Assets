using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FurniturePreviewCellView : MonoBehaviour
{
	private FurnitureEntry _furnitureEntry;
	public FurnitureEntry furnitureEntry{
		get { return _furnitureEntry;}
		set { _furnitureEntry = value; updateData();}
	}

	public Text furnitureName;
	public Image previewImage;
	public Button addToWishListButton;
	public Button cellButton;

	public void updateData(){
		furnitureName.text = furnitureEntry.name;
		previewImage.sprite = Resources.Load<Sprite> (furnitureEntry.image);
	}

	public void init(){
		furnitureName = transform.FindChild ("Panel").FindChild ("FurnitureName").GetComponent<Text> ();
		previewImage = transform.FindChild ("PreviewImage").GetComponent<Image> ();
		cellButton = transform.GetComponent<Button> ();
		addToWishListButton = transform.FindChild ("Panel").FindChild ("AddToWishListButton").GetComponent<Button> ();
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

