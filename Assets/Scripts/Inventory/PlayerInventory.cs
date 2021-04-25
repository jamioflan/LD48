using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	public int numWeaponSlots = 2;
	public int numArmourSlots = 1;
	public int numMagicSlots = 2;

	public Inventory<Weapon> weapons;
	public Inventory<InventoryItem> junk;

	public Weapon defaultWeapon;
	public int selectedWeaponSlot = 0;
	public Transform weaponOrigin;

	public void CycleNextWeapon()
	{
		selectedWeaponSlot++;
		if(selectedWeaponSlot >= weapons.NumSlots)
		{
			selectedWeaponSlot = 0;
		}
	}

	public void CyclePreviousWeapon()
	{
		selectedWeaponSlot--;
		if (selectedWeaponSlot < 0)
		{
			selectedWeaponSlot = weapons.NumSlots - 1;
		}
	}

	public Weapon GetSelectedWeapon()
	{
		return GetWeaponInSlot(selectedWeaponSlot);
	}

	public Weapon GetWeaponInSlot(int slot)
	{
		return weapons.GetInSlot(slot);
	}

	public void Purchase(InventoryItem item, int toSlot)
	{
		if(item is Weapon weapon)
		{
			item.transform.SetParent(weaponOrigin);
			item.owner = Player.inst;
			junk.InsertJunkItem(weapons.ReplaceItem(weapon, toSlot));
		}

		
	}

	// Start is called before the first frame update
	void Start()
    {
		weapons = new Inventory<Weapon>(numWeaponSlots);
		if(defaultWeapon != null)
		{
			Weapon weapon = Instantiate(defaultWeapon);
			weapons.ReplaceItem(weapon, 0);
			weapon.owner = Player.inst;
		}
	}

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < weapons.NumSlots; i++)
		{
			Weapon weapon = weapons.GetInSlot(i);
			if (weapon != null)
			{
				// Activate weapon prefab
				if (i == selectedWeaponSlot)
				{
					weapon.gameObject.SetActive(true);
					weapon.transform.SetParent(weaponOrigin);
					weapon.transform.localPosition = Vector3.zero;
					weapon.transform.localRotation = Quaternion.identity;
					weapon.transform.localScale = Vector3.one;
				}
				// Deactivate
				else
				{
					weapon.gameObject.SetActive(false);
				}
			}
		}
    }
}
