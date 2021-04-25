using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory<T> where T : InventoryItem
{
	public T[] slots = null;

	public int NumSlots { get { return slots.Length; } }

	public Inventory(int numSlots)
	{
		slots = new T[numSlots];
	}

	public T ReplaceItem(T newT, int index)
	{
		T oldT = slots[index];
		if (oldT != null)
		{
			oldT.gameObject.SetActive(false);
			oldT.transform.parent = null;
		}
		slots[index] = newT;
		if (newT != null)
		{
			newT.gameObject.SetActive(true);
		}
		return oldT;
	}

	public T GetInSlot(int slot)
	{
		return slots[slot];
	}

	// Shuffle down
	public void InsertJunkItem(T newT)
	{
		for (int i = slots.Length - 2; i >= 0; i--)
		{
			slots[i + 1] = slots[i];
		}

		slots[0] = newT;
	}
}
