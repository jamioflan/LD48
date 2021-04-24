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

	public Weapon GetWeaponInSlot(int slot)
	{
		return weapons.GetInSlot(slot);
	}

	public void Purchase(InventoryItem item, int toSlot)
	{
		if(item is Weapon weapon)
		{
			junk.InsertJunkItem(weapons.ReplaceItem(weapon, toSlot));
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		weapons = new Inventory<Weapon>(numWeaponSlots);
		if(defaultWeapon != null)
		{
			weapons.ReplaceItem(Instantiate(defaultWeapon), 0);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
