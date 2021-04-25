using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAttackInfo : MonoBehaviour
{
	public Text nameText, descText;
	public Image icon, border;
	public Image cooldown;

	public void SetItem(InventoryItem item)
	{
		if (item == null)
		{
			nameText.text = "-Empty Slot-";
			descText.text = "";
			icon.sprite = null;
		}
		else
		{
			nameText.text = item.GetDisplayName();
			if (descText)
				descText.text = item.GetDescription();
			Texture2D tex = item.GetTexture();
			icon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
		}
	}

	public void SetSelected(bool selected)
	{
		border.color = selected ? Color.green : Color.gray;
	}

	public void SetCooldown(float parametricRemaining)
	{
		cooldown.fillAmount = parametricRemaining;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
