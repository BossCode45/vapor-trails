using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    static readonly float INPUT_TOLERANCE = 0.1f;

    static bool frozenInputs = false;

    public static bool HasHorizontalInput() {
        return Mathf.Abs(Input.GetAxis(Inputs.H_AXIS)) > INPUT_TOLERANCE;
    }

    public static float HorizontalInput() {
        return Input.GetAxis(Inputs.H_AXIS);
    }

    public static float VerticalInput() {
        return Input.GetAxis(Inputs.V_AXIS);
    }

    public static bool ButtonDown(string buttonName) {
        return Input.GetButtonDown(buttonName);
    }

    public static bool ButtonUp(string buttonName) {
        return Input.GetButtonUp(buttonName);
    }

    public static void FreezeInputs() {
        frozenInputs = true;
    }

    public static void UnfreezeInputs() {
        frozenInputs = false;
    }
}