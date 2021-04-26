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
	public Text costText;

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
			switch(t)
			{
				case Type.ARMOUR: nameText.text = "-Empty Armour Slot-"; break;
				case Type.WEAPON: nameText.text = "-Empty Weapon Slot-"; break;
				case Type.MAGIC: nameText.text = "-Empty Magic Slot-"; break;
				case Type.SHOP: nameText.text = "-Empty-"; break;
			}
			statsText.text = "";
			if (costText != null)
				costText.text = "";
		}
		else
		{
			Texture2D tex = it.GetTexture();
			icon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
			nameText.text = item.GetDisplayName();
			statsText.text = item.GetDescription();
			if(costText != null)
				costText.text = "" + item.cost;
		}
	}

	public void OnClick()
	{
		UI.inst.OnSlotClicked(this);
	}
}
