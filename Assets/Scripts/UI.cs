using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	public static UI inst;

	// Level Name
	public Text levelNameText;

	// Slots
	public UIAttackInfo[] weaponInfos = new UIAttackInfo[0];
	public UIAttackInfo[] magicInfos = new UIAttackInfo[0];

	// Announcer
	public RectTransform announcer;
	public Text announcerText;
	public float announcerTimeLeft = 0;
	public float announcerBobSpeed = 1.0f;
	public float announcerBobAngle = 2.0f;
	public float announcerSlideIn = 0.0f;
	private Vector3 announcerRestPos = Vector3.zero;
	public float announcerSlideInTime = 0.25f;

	// Coinage
	public Text currencyText;

	// Damage numbers
	public DamageNumbers damageNumbersPrefab;
	public UIHealthbar healthbarPrefab;
	public GameObject playerHealthbarObject;
	public Image playerHealthBar;
	public GameObject bossHealthBarObject;
	public Image bossHealthBar;
	public Text bossName;

	// End of Level Screen
	public GameObject shop;
	public Text levelCompleteText;
	public GridLayout killsGrid;
	public UISlot[] shopSlots;
	public UISlot[] magicSlots;
	public UISlot[] weaponSlots;
	public UISlot[] armourSlots;

	public int selectedShopSlot = -1;
	public int selectedWeaponSlot = -1;
	public int selectedMagicSlot = -1;
	public int selectedArmourSlot = -1;

	public Text shopTutorial;
	public RectTransform nextLevel;

	// Character Select Screen
	public GameObject characterSelectScreen;
	public Image characterIcon;
	public Text charName, charDesc;


	private void Awake()
	{
		inst = this;
		announcer.gameObject.SetActive(true);
	}

	// Shop stuff
	public void OpenShop(LevelData levelCompleted)
	{
		shop.SetActive(true);

		levelCompleteText.text = $@"- Level {levelCompleted.levelNumber + 1} Complete -
{levelCompleted.levelName}
";

		selectedShopSlot = -1;
		InventoryItem[] loot = LootGenerator.inst.GenerateLoot(shopSlots.Length, levelCompleted.levelNumber);
		for (int i = 0; i < shopSlots.Length; i++)
		{
			// Generate a shop item
			shopSlots[i].SetItem(loot[i], UISlot.Type.SHOP, i);
			shopSlots[i].SetSelected(false);
		}

		selectedWeaponSlot = -1;
		for (int i = 0; i < weaponSlots.Length; i++)
		{
			weaponSlots[i].SetItem(Player.inst.inventory.weapons.GetInSlot(i), UISlot.Type.WEAPON, i);
			weaponSlots[i].SetSelected(false);
		}

		selectedMagicSlot = -1;
		for (int i = 0; i < magicSlots.Length; i++)
		{
			magicSlots[i].SetItem(Player.inst.inventory.spells.GetInSlot(i), UISlot.Type.MAGIC, i);
			magicSlots[i].SetSelected(false);
		}

		selectedArmourSlot = -1;
		for (int i = 0; i < armourSlots.Length; i++)
		{
			armourSlots[i].SetItem(Player.inst.inventory.armour.GetInSlot(i), UISlot.Type.ARMOUR, i);
			armourSlots[i].SetSelected(false);
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
					shopTutorial.text = "Please choose an item to purchase first";
				else
					selectedWeaponSlot = slot.id;
				break;
			}
			case UISlot.Type.MAGIC:
			{
				if (selectedShopSlot == -1)
					shopTutorial.text = "Please choose an item to purchase first";
				else
					selectedMagicSlot = slot.id;
				break;
			}
			case UISlot.Type.ARMOUR:
			{
				if (selectedShopSlot == -1)
					shopTutorial.text = "Please choose an item to purchase first";
				else
					selectedArmourSlot = slot.id;
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
					slot.SetSelected(true);
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
				if (shopSlot.item.cost <= Game.inst.currency)
				{
					// Purchase the item
					Game.inst.SpendCurrency(shopSlot.item.cost);
					Player.inst.inventory.Purchase(shopSlot.item, selectedWeaponSlot);
					// Update visual slots
					weaponSlots[selectedWeaponSlot].SetItem(shopSlot.item, UISlot.Type.WEAPON, selectedWeaponSlot);
					shopSlot.SetItem(null, UISlot.Type.SHOP, selectedShopSlot);
				}
				else
				{
					shopTutorial.text = "You can't afford that";
				}

				// Reset selections
				shopSlot.SetSelected(false);
				weaponSlots[selectedWeaponSlot].SetSelected(false);
				selectedShopSlot = -1;
				selectedWeaponSlot = -1;
			}

			// We ready to purchase a spell
			if (shopSlot.item is Spell && selectedMagicSlot != -1)
			{
				if (shopSlot.item.cost <= Game.inst.currency)
				{
					// Purchase the item
					Game.inst.SpendCurrency(shopSlot.item.cost);
					Player.inst.inventory.Purchase(shopSlot.item, selectedMagicSlot);
					// Update visual slots
					magicSlots[selectedMagicSlot].SetItem(shopSlot.item, UISlot.Type.MAGIC, selectedMagicSlot);
					shopSlot.SetItem(null, UISlot.Type.SHOP, selectedShopSlot);
				}
				else
				{
					shopTutorial.text = "You can't afford that";
				}

				// Reset selections
				magicSlots[selectedMagicSlot].SetSelected(false);
				shopSlot.SetSelected(false);
				selectedShopSlot = -1;
				selectedWeaponSlot = -1;
			}

			// We ready to purchase an armour
			if (shopSlot.item is Armour && selectedArmourSlot != -1)
			{
				if (shopSlot.item.cost <= Game.inst.currency)
				{
					// Purchase the item
					Game.inst.SpendCurrency(shopSlot.item.cost);
					Player.inst.inventory.Purchase(shopSlot.item, selectedArmourSlot);
					// Update visual slots
					armourSlots[selectedArmourSlot].SetItem(shopSlot.item, UISlot.Type.ARMOUR, selectedArmourSlot);
					shopSlot.SetItem(null, UISlot.Type.SHOP, selectedShopSlot);
				}
				else
				{
					shopTutorial.text = "You can't afford that";
				}

				// Reset selections
				armourSlots[selectedArmourSlot].SetSelected(false);
				shopSlot.SetSelected(false);
				selectedShopSlot = -1;
				selectedArmourSlot = -1;
			}
		}
	}

	public void CloseShop()
	{
		shop.SetActive(false);
		Game.inst.FinishedShopping();
	}
	// 

	public void SetLevelNameText(string message)
	{
		levelNameText.text = message;
	}

	public void Announce(string message)
	{
		announcerText.text = message;
		announcerTimeLeft = 1.5f;
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

	public void SpawnHealthbar(Creature creature)
	{
		if (creature.GetComponentInChildren<UIHealthbar>() == null)
		{
			UIHealthbar hpBar = Instantiate(healthbarPrefab, Vector3.up * creature.hpbarheight, Quaternion.identity, creature.transform);
			hpBar.transform.localPosition = new Vector3(0f, creature.hpbarheight, 0f);
			hpBar.transform.localRotation = Quaternion.identity;
			//hpBar.transform.localScale = Vector3.one;
			hpBar.creature = creature;
		}
	}

	public void ShowCharacterSelection()
	{
		characterSelectScreen.SetActive(true);

		charName.text = Player.inst.names[charSelection];
		charDesc.text = Player.inst.descs[charSelection];
		characterIcon.sprite = Player.inst.icons[charSelection];
	}

	public int charSelection;
	public void PrevChar()
	{
		charSelection--;
		if (charSelection < 0)
			charSelection = 3;

		charName.text = Player.inst.names[charSelection];
		charDesc.text = Player.inst.descs[charSelection];
		characterIcon.sprite = Player.inst.icons[charSelection];
	}

	public void NextChar()
	{
		charSelection++;
		if (charSelection > 3)
			charSelection = 0;

		charName.text = Player.inst.names[charSelection];
		charDesc.text = Player.inst.descs[charSelection];
		characterIcon.sprite = Player.inst.icons[charSelection];
	}

	public void ConfirmCharacterSelection()
	{
		characterSelectScreen.SetActive(false);
		Player.inst.ConfirmCharacter((Player.Character)charSelection);
		Game.inst.Begin();
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


		// Inventory slots
		for (int i = 0; i < weaponInfos.Length; i++)
		{
			Weapon weapon = Player.inst.inventory.GetWeaponInSlot(i);
			if (weapon == null)
			{
				weaponInfos[i].SetItem(null);
			}
			else
			{
				weaponInfos[i].SetItem(weapon);
				weaponInfos[i].SetSelected(Player.inst.inventory.selectedWeaponSlot == i);
				weaponInfos[i].SetCooldown(Player.inst.attacks.GetAttackCooldownParametric());
			}
		}
		for (int i = 0; i < magicInfos.Length; i++)
		{
			Spell magic = Player.inst.inventory.GetSpellInSlot(i);
			if (magic == null)
			{
				magicInfos[i].SetItem(null);
			}
			else
			{
				magicInfos[i].SetItem(magic);
				magicInfos[i].SetSelected(true);
				magicInfos[i].SetCooldown(magic.GetParametricCooldown());
			}
		}

		nextLevel.localScale = Vector3.one * (0.9f + 0.1f * Mathf.Sin(Time.time));
		nextLevel.localEulerAngles = new Vector3(0f, 0f, Mathf.Cos(Time.time) * 10f);



		// Currency
		currencyText.text = "" + Game.inst.currency;

		// Player HP
		playerHealthbarObject.SetActive(Game.inst.state == Game.State.IN_LEVEL);
		playerHealthBar.fillAmount = Mathf.Clamp01((Player.inst.health + Player.inst.tempHealth) / Player.inst.maxHealth);
		playerHealthBar.color = Player.inst.tempHealth > 0.0f ? Color.yellow : Color.red;

		// Boss HP
		bossHealthBarObject.SetActive(false);
		if (LevelGenerator.inst.currentLevel != null)
		{
			if(LevelGenerator.inst.currentLevel.boss != null)
			{
				Creature boss = LevelGenerator.inst.currentLevel.boss.GetComponent<Creature>();
				if (boss != null)
				{
					bossHealthBarObject.SetActive(true);
					bossName.text = LevelGenerator.inst.currentLevel.bossName;
					bossHealthBar.fillAmount = Mathf.Clamp01((boss.health + boss.tempHealth) / boss.maxHealth);
				}

			}
		}
	}

	float smoothstep(float edge0, float edge1, float x)
	{
		// Scale, bias and saturate x to 0..1 range
		x = Mathf.Clamp((x - edge0) / (edge1 - edge0), 0, 1);
		// Evaluate polynomial
		return x * x * (3 - 2 * x);
	}
}
