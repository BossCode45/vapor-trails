using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryList : MonoBehaviour {
    public List<StoredItem> items;

    public InventoryList() {
        this.items = new List<StoredItem>();
    }

    public StoredItem GetItem(string itemName) {
        foreach (StoredItem i in items) {
            if (i.name.Equals(itemName)) {
                return i;
            }
        }
        return null;
    }

    public void Empty() {
        items.Clear();
    }

    public StoredItem GetItem(Item item) {
        return GetItem(item.name);
    }

    public StoredItem GetItem(StoredItem item) {
        return GetItem(item.name);
    }

    public bool HasItem(string itemName) {
        return GetItem(itemName) != null;
    }

    public bool HasItem(Item item) {
        return HasItem(item.name);
    }

    public bool HasItem(StoredItem stored) {
        return HasItem(stored.name);
    }

    public Item GetItemByIndex(int index) {
        return items[index].item;
    }

    public void AddItem(StoredItem s) {
        if (s.item.stackable && HasItem(s)) {
            GetItem(s).count += s.count;
        } else {
            items.Add(s);
        }
    }

    public void AddItem(Item s) {
        AddItem(new StoredItem(s));
    }

    public void AddAll(InventoryList inventoryList) {
        AddAll(inventoryList.items);
    }

    public void AddAll(List<Item> items) {
        foreach (Item i in items) {
            AddItem(i);
        }
    }

    public void AddAll(List<StoredItem> items) {
        foreach (StoredItem i in items) {
            AddItem(i.item);
        }
    }
    
    public SerializableInventoryList MakeSerializableInventory() {
        return new SerializableInventoryList(items);
    }

    public void LoadFromSerializableInventoryList(SerializableInventoryList i) {
        this.items = i.items;
    }

    public void RemoveItem(StoredItem toRemove) {
        if (GetItem(toRemove) == null) {
            Debug.Log("RemoveItem isn't nullsafe you brainlet");
        }
        if (toRemove.item.stackable) {
            GetItem(toRemove).count -= Mathf.Max(toRemove.count, 1);
            if (GetItem(toRemove).count == 0) {
                items.Remove(GetItem(toRemove));
            }
        } else {
            items.Remove(GetItem(toRemove));
        } 

    }
}

[System.Serializable]
public class SerializableInventoryList {
    public List<StoredItem> items;
    
    public SerializableInventoryList(List<StoredItem> items) {
        this.items = items;
    }
}