﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnState : StateChangeReactor {

	public GameFlag gameFlag;
	public bool enableOnState;

	override public void React(bool fakeSceneLoad) {
		if (enableOnState) {
			this.gameObject.SetActive(GlobalController.HasFlag(gameFlag));
		} else {
			this.gameObject.SetActive(!GlobalController.HasFlag(gameFlag));
		}
	}
	
}
