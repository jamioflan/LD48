using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
	public enum Type
	{
		SHOP,
		WEAPON,
		MAGIC, 
		ARMOUR,
	}

	public Type type;
	public Image icon;
	public InventoryItem item;
	public int id;
	public Text nameText;
	public Text statsText;

	public Image selectedOverlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetSelected(bool selected)
	{
		selectedOverlay.gameObject.SetActive(selected);
	}

	public void SetItem(InventoryItem it, Type t, int i)
	{
		item = it;
		type = t;
		id = i;
		if (item == null)
		{
			icon.sprite = null;
		}
		else
		{
			Texture2D tex = it.GetTexture();
			icon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
			nameText.text = item.displayName;
			statsText.text = item.GetDescription();
		}
	}

	public void OnClick()
	{
		UI.inst.OnSlotClicked(this);
	}
}
