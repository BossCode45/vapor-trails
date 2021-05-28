using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Scene Container", menuName = "Runtime/Save Container")]
public class SaveContainer : ScriptableObject {

    // this exists so multiple save containers can reference the same save
    // so dev saves left over in levels can use the runtime save in the final build
    [SerializeField] RuntimeSaveWrapper runtime;

    [SerializeField] List<Item> startingItems;
    [SerializeField] List<GameState> startingGameStates;

    public Save GetSave() {
        return runtime.save;
    }

    public void WipeSave() {
        runtime.inventory.Clear();
        runtime.loadedOnce = false;
        SaveContainer newSave = Resources.Load("ScriptableObjects/Runtime/Save Containers/New") as SaveContainer;
        runtime.save = newSave.GetSave();
        this.startingGameStates = newSave.startingGameStates;
        this.startingItems = newSave.startingItems;
        runtime.save.Initialize();
    }

    public void OnSceneLoad() {
        if (!runtime.save.firstLoadHappened) {
            runtime.save.Initialize();
            foreach (Item i in startingItems) {
                GlobalController.AddItem(new StoredItem(i), quiet:true);
            }
            GlobalController.AddStates(startingGameStates);
            runtime.save.firstLoadHappened = true;
        }
        // player is not getting items
        if (!runtime.loadedOnce) {
            LoadRuntime();
        }
    }

    void SaveRuntime() {
        runtime.save.playerItems = runtime.inventory;
    }

    void LoadRuntime() {
        runtime.inventory = runtime.save.playerItems;
        runtime.loadedOnce = true;
    }

    public bool RuntimeLoadedOnce() {
        return runtime.loadedOnce;
    }

    public InventoryList GetInventory() {
        return runtime.inventory;
    }

    public void LoadFromSlot(int slot) {
        runtime.loadedOnce = false;
        runtime.save = BinarySaver.LoadFile(slot);
    }

    public void WriteToDiskSlot(int slot) {
        SaveRuntime();
        BinarySaver.SaveFile(runtime.save, slot);
    }

    public void SyncImmediateStates(int slot) {
        // load a copy of the current save parallel to runtime, add/remove immediate states as necessary, and save
        // don't add any of the current runtime properties
        if (BinarySaver.HasFile(slot)) {
            Save diskSave = BinarySaver.LoadFile(slot);

            // prune old states
            List<string> toPrune = new List<string>();
            foreach (string diskState in diskSave.gameStates) {
                if ((Resources.Load("ScriptableObjects/Game States/"+diskState) as GameState).writeImmediately) {
                    if (!runtime.save.gameStates.Contains(diskState)) {
                        toPrune.Add(diskState);
                    }
                }
            }
            foreach (string s in toPrune) {
                diskSave.gameStates.Remove(s);
            }

            // add new states
            foreach (string stateName in runtime.save.gameStates) {
                if ((Resources.Load("ScriptableObjects/Game States/"+stateName) as GameState).writeImmediately) {
                    diskSave.gameStates.Add(stateName);
                }
            }
            // save the non-runtime save loaded from disk
            BinarySaver.SaveFile(diskSave, slot);
        }
    }
}
