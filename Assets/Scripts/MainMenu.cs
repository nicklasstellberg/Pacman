using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // K‰ynnist‰‰ pelin, lataamalla seuraavan tason aktiivisen tason indeksin perusteella.
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Lopettaa sovelluksen suorituksen.
    public void QuitGame ()
    {
        Application.Quit();
    }
}
