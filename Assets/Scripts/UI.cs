using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	public static UI inst;

	// Level Name
	public Text levelNameText;

	// Magic Slots
	[System.Serializable]
	public class MagicItemSlot
	{
		public Text magicItemName;
		public Image magicItemSprite;
		public Image magicItemCooldown;
	}
	public MagicItemSlot[] magicInfo = new MagicItemSlot[0];

	// Announcer
	public RectTransform announcer;
	public Text announcerText;
	public float announcerTimeLeft = 0;
	public float announcerBobSpeed = 1.0f;
	public float announcerBobAngle = 2.0f;
	public float announcerSlideIn = 0.0f;
	private Vector3 announcerRestPos = Vector3.zero;
	public float announcerSlideInTime = 0.25f;

	// Damage numbers
	public DamageNumbers damageNumbersPrefab;

	// End of Level Screen
	public Text levelCompleteText;
	public GridLayout killsGrid;
	public UISlot[] shopSlots;
	public UISlot[] magicSlots;
	public UISlot[] weaponSlots;
	public UISlot armourSlot;

	public int selectedShopSlot = -1;
	public int selectedWeaponSlot = -1;

	public Text shopTutorial;


	private void Awake()
	{
		inst = this;
		announcer.gameObject.SetActive(true);
	}

	// Shop stuff
	public void OpenShop(LevelData levelCompleted)
	{
		selectedShopSlot = -1;
		InventoryItem[] loot = LootGenerator.inst.GenerateLoot(shopSlots.Length, levelCompleted.levelNumber);
		for (int i = 0; i < shopSlots.Length; i++)
		{
			// Generate a shop item
			shopSlots[i].SetItem(loot[i], UISlot.Type.SHOP, i);
		}

		selectedWeaponSlot = -1;
		for (int i = 0; i < weaponSlots.Length; i++)
		{
			weaponSlots[i].SetItem(Player.inst.inventory.weapons.GetInSlot(i), UISlot.Type.WEAPON, i);
			weaponSlots[i].SetSelected(false);
		}

		//for (int i = 0; i < magicSlots.Length; i++)
		//{
			//weaponSlots[i].SetItem(Player.inst.inventory.ma.GetInSlot(i));
		//}

	}

	public void OnSlotClicked(UISlot slot)
	{
		switch(slot.type)
		{
			case UISlot.Type.WEAPON:
			{
				if (selectedShopSlot == -1)
					shopTutorial.text = "Please choose an item to purchase";
				else
					selectedWeaponSlot = slot.id;
				break;
			}
			case UISlot.Type.SHOP:
			{
				if (slot.item == null)
					shopTutorial.text = "Already purchased";
				else
				{
					shopTutorial.text = "Select a slot to replace";
					selectedShopSlot = slot.id;
				}
				break;
			}
		}

		// Now compare selections
		if(selectedShopSlot != -1)
		{
			UISlot shopSlot = shopSlots[selectedShopSlot];
			// We ready to purchase a weapon
			if(shopSlot.item is Weapon && selectedWeaponSlot != -1)
			{
				// Purchase the item
				Player.inst.inventory.Purchase(shopSlot.item, selectedWeaponSlot);
				// Update visual slots
				weaponSlots[selectedWeaponSlot].SetItem(shopSlot.item, UISlot.Type.WEAPON, selectedWeaponSlot);
				shopSlot.SetItem(null, UISlot.Type.SHOP, selectedShopSlot);
				// Reset selections
				selectedShopSlot = -1;
				selectedWeaponSlot = -1;
			}
		}
	}

	public void CloseShop()
	{

	}
	//

	public void SetLevelNameText(string message)
	{
		levelNameText.text = message;
	}

	public void Announce(string message)
	{
		announcerText.text = message;
		announcerTimeLeft = 3.0f;
	}

	public void ConfirmInventoryChoices()
	{

	}

	public void SpawnDamageNumbers(int amount, Vector3 position)
	{
		SpawnDamageNumbers(amount, Color.red, position);
	}

	public void SpawnDamageNumbers(int amount, Color colour, Vector3 position)
	{
		DamageNumbers numbers = Instantiate(damageNumbersPrefab);
		numbers.transform.position = position;
		numbers.Init(amount, colour);
	}

	// Start is called before the first frame update
	void Start()
    {
		announcerRestPos = announcer.localPosition;

	}

    // Update is called once per frame
    void Update()
    {
		// Announcer
		announcerTimeLeft -= Time.deltaTime;
		if(announcerTimeLeft > 0.0f)
		{
			announcerSlideIn += Time.deltaTime / announcerSlideInTime;
			if (announcerSlideIn >= 1.0f)
				announcerSlideIn = 1.0f;
			announcerText.rectTransform.localEulerAngles = new Vector3(0f, 0f, 10f + announcerBobAngle * Mathf.Sin(Time.time * announcerBobSpeed));
		}
		else
		{
			announcerSlideIn -= Time.deltaTime / announcerSlideInTime;
			if (announcerSlideIn <= 0.0f)
				announcerSlideIn = 0.0f;


		}

		float slideIn = smoothstep(0.0f, 1.0f, announcerSlideIn);
		announcer.localPosition = announcerRestPos + new Vector3((1.0f - slideIn) * 3000f, 0f, 0f);


	}

	float smoothstep(float edge0, float edge1, float x)
	{
		// Scale, bias and saturate x to 0..1 range
		x = Mathf.Clamp((x - edge0) / (edge1 - edge0), 0, 1);
		// Evaluate polynomial
		return x * x * (3 - 2 * x);
	}
}
