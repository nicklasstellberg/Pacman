using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false; // Muuttuja, joka kertoo onko peli pysäytetty
    public GameObject PauseMenuCanvas; // Pelin pysäytysvalikon kanvas

    // Start-metodi kutsutaan ennen ensimmäistä framea
    void Start()
    {
        Time.timeScale = 1f; // Asetetaan ajan kiihtyvyys normaaliksi
    }

    // Update-metodi kutsutaan jokaisen framen yhteydessä
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Jos Esc-näppäintä painetaan
        {
            if (Paused) // Jos peli on jo pysäytetty
            {
                Play(); // Jatketaan peliä
            }
            else // Muussa tapauksessa
            {
                Stop(); // Pysäytetään peli
            }
        }
    }

    // Pysäyttää pelin ja avaa pelin pysäytysvalikon
    public void Stop()
    {
        PauseMenuCanvas.SetActive(true); // Näytetään pelin pysäytysvalikko
        Time.timeScale = 0f; // Asetetaan ajan kiihtyvyys nollaksi, jolloin peli pysähtyy
        Paused = true; // Asetetaan muuttuja Paused totuusarvoksi true
    }

    // Jatkaa peliä ja piilottaa pelin pysäytysvalikon
    public void Play()
    {
        PauseMenuCanvas.SetActive(false); // Piilotetaan pelin pysäytysvalikko
        Time.timeScale = 1f; // Asetetaan ajan kiihtyvyys normaaliksi, jolloin peli jatkuu
        Paused = false; // Asetetaan muuttuja Paused totuusarvoksi false
    }

    // Metodi käynnistää uuden pelin
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    // Palaa päävalikkoon ja asettaa ajan kiihtyvyyden normaaliksi
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0); // Ladataan ensimmäinen Scene
        Time.timeScale = 1f; // Asetetaan ajan kiihtyvyys normaaliksi, jolloin peli jatkuu
        Paused = false; // Asetetaan muuttuja Paused totuusarvoksi false
    }
}
