using UnityEngine;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Music Buttons")]
    public GameObject musicOnBtn;
    public GameObject musicOffBtn;

    [Header("SFX Buttons")]
    public GameObject sfxOnBtn;
    public GameObject sfxOffBtn;

    void Start()
    {
        UpdateUI();
    }

    public void MusicOn()
    {
        AudioManager.Instance.SetMusic(true);
        UpdateUI();
    }

    public void MusicOff()
    {
        AudioManager.Instance.SetMusic(false);
        UpdateUI();
    }

    public void SFXOn()
    {
        AudioManager.Instance.SetSFX(true);
        UpdateUI();
    }

    public void SFXOff()
    {
        AudioManager.Instance.SetSFX(false);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (AudioManager.Instance == null) return;

        musicOnBtn.SetActive(AudioManager.Instance.musicEnabled);
        musicOffBtn.SetActive(!AudioManager.Instance.musicEnabled);

        sfxOnBtn.SetActive(AudioManager.Instance.sfxEnabled);
        sfxOffBtn.SetActive(!AudioManager.Instance.sfxEnabled);
    }
}
