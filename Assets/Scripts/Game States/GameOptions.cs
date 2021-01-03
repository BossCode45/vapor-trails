using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class GameOptions {
    public bool shortHop = true;
    public bool gameJournalist = false;
    public bool slideDrop = false;
    public int inputBuffer = 8;
    
    bool fullscreen = true;

    public void Load() {
        shortHop = LoadBool("ShortHop");
        gameJournalist = LoadBool("GameJournalist");
        slideDrop = LoadBool("SlideDrop");
        GlobalController.pc.GetComponent<Animator>().SetBool("LedgeDrop", slideDrop);
        inputBuffer = LoadInt("InputBuffer");
        fullscreen = LoadBool("Fullscreen");
    
        Application.runInBackground = LoadBool("RunInBackground");
        QualitySettings.vSyncCount = LoadInt("VSync");
    
        if (fullscreen) {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        } else {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    // player pref changes are done via scripts attached to buttons
    // SettingsSlider and SettingsToggle
    public void Apply() {
        PlayerPrefs.Save();
        Load();
    }

    public static bool LoadBool(string boolName, bool defaultValue = false) {
        return PlayerPrefs.GetInt(boolName, defaultValue ? 1 : 0) == 1;
    }

    static int LoadInt(string intName, int defaultValue = 0) {
        return PlayerPrefs.GetInt(intName, defaultValue);
    }
}
