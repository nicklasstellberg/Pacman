using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;  // Viittaus liukus‰‰timeen, jolla asetetaan ‰‰nenvoimakkuus.

    private void Start()
    {
        LoadValues();  // Ladataan tallennetut arvot, kun skripti k‰ynnistyy.
    }

    // Metodi, joka tallentaa ‰‰nenvoimakkuuden.
    public void VolumeSlider(float volume)
    {
        SaveVolumeButton();
    }

    // Metodi, joka tallentaa liukus‰‰timen asettaman ‰‰nenvoimakkuuden PlayerPrefs-tietokantaan.
    public void SaveVolumeButton()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadValues();  // Ladataan tallennetut arvot.
    }

    // Metodi, joka lataa tallennetut ‰‰nenvoimakkuusasetukset ja p‰ivitt‰‰ liukus‰‰timen sek‰ ‰‰nil‰hteen arvot.
    void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;  // P‰ivitet‰‰n liukus‰‰timen arvo tallennetulla arvolla.
        AudioListener.volume = volumeValue;  // P‰ivitet‰‰n ‰‰nil‰hteen arvo tallennetulla arvolla.
    }
}
