using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Luodaan muuttujat, joihin tallennetaan pelin hahmot ja tekstikent‰t
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    // Tekstikent‰t
    public Text gameOverText;
    public Text scoreText;
    public Text highScoreText;
    public Text livesText;

    // Pisteiden kerroin haamujen syˆnnin yhteydess‰
    public int ghostMultiplier { get; private set; } = 1;

    // Pelin pisteet, el‰m‰t ja ‰‰net
    public int score { get; private set; }
    public int lives { get; private set; }
    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    private AudioSource currentMunchAudio;

    // Awake()-funktio kutsutaan ennen Start()-funktiota
    void Awake()
    {
        // Soitetaan sireenin ‰‰ni heti pelin k‰ynnistyess‰
        siren.Play();
    }

    // Start()-funktio kutsutaan ensimm‰isell‰ framelilla, kun peli k‰ynnistyy
    private void Start()
    {
        // P‰ivitet‰‰n high score -tekstikentt‰
        UpdateHighScoreText();

        // K‰ynnistet‰‰n uusi peli
        NewGame();

        // Asetetaan nykyinen munch-‰‰ni k‰ytt‰m‰‰n munch1-‰‰nitiedostoa
        currentMunchAudio = munch1;
    }

    // Update()-funktio kutsutaan jokaisella framella
    private void Update()
    {
        // Tarkistetaan, onko pelaajalla en‰‰ el‰mi‰ j‰ljell‰ ja onko pelaaja painanut mit‰‰n n‰pp‰int‰
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            // K‰ynnistet‰‰n uusi peli
            NewGame();
        }

        // P‰ivitet‰‰n high score -tekstikentt‰
        UpdateHighScoreText();
    }

    // Funktio k‰ynnist‰‰ uuden pelin
    private void NewGame()
    {
        // Nollataan pelaajan pisteet ja asetetaan el‰mien m‰‰r‰ kolmeksi
        SetScore(0);
        SetLives(3);

        // Aloita uusi kierros
        NewRound();
    }

    // Funktio aloittaa uuden kierroksen
    private void NewRound()
    {
        // Piilotetaan Game Over -tekstikentt‰
        gameOverText.enabled = false;

        // Aktivoidaan kaikki pisteet uudelleen
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        // Nollataan pelihahmojen tila
        ResetState();
    }

    // Funktio nollaa pelihahmojen tilan
    private void ResetState()
    {
        // Nollataan haamujen kerroin
        ResetGhostMultiplier();

        // K‰yd‰‰n l‰pi kaikki haamut ja asetetaan ne alkutilaan
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        // Asetetaan Pacman alkutilaan
        this.pacman.ResetState();
    }

    // Funktio, joka kutsutaan kun peli p‰‰ttyy
    private void GameOver()
    {
        // N‰ytet‰‰n pelin loppuviesti
        gameOverText.enabled = true;

        // Piilotetaan kaikki haamut
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        // Piilotetaan Pacman
        this.pacman.gameObject.SetActive(false);
    }

    // Asettaa pelaajan el‰mien m‰‰r‰n
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    // Asettaa pelaajan pisteet
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');

        // P‰ivitet‰‰n highscore, jos pelaaja saavuttaa uuden enn‰tyksen
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    // P‰ivitt‰‰ highscore-tekstin
    void UpdateHighScoreText()
    {
        highScoreText.text = $"HighScore: {PlayerPrefs.GetInt("HighScore", 0)}";
    }

    // Kutsutaan kun haamu syˆd‰‰n
    public void GhostEaten(Ghost ghost)
    {
        // Lis‰t‰‰n pelaajan pisteisiin haamun pisteet kerrottuna kerroinlukijalla
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);

        // Kasvatetaan kerrointa
        this.ghostMultiplier++;
    }

    // Kutsutaan kun Pacman syˆd‰‰n
    public void PacmanEaten()
    {
        // Piilotetaan Pacman
        this.pacman.gameObject.SetActive(false);

        // V‰hennet‰‰n pelaajan el‰mien m‰‰r‰‰
        SetLives(this.lives - 1);

        // Jos pelaajalla on el‰mi‰ j‰ljell‰, palautetaan peli alkutilaan muutaman sekunnin kuluttua
        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        // Jos pelaajalla ei ole el‰mi‰ j‰ljell‰, n‰ytet‰‰n pelin loppuviesti
        else
        {
            GameOver();
        }
    }

    // Kutsutaan kun tavallinen pelletti syˆd‰‰n
    public void PelletEaten(Pellet pellet)
    {
        // Piilotetaan syˆty pelletti
        pellet.gameObject.SetActive(false);

        // Lis‰t‰‰n pelaajan pisteisiin pelletin pisteet
        SetScore(this.score + pellet.points);

        // Soitetaan nykyinen "munch" -‰‰niefekti ja vaihdetaan toiseen ‰‰niefektiin
        currentMunchAudio.Play();
        currentMunchAudio = (currentMunchAudio == munch1) ? munch2 : munch1;

        // Jos kaikki pelletit on syˆty, aloitetaan uusi kierros
        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        // Asetetaan haamut pelokkaiksi annetun keston ajaksi
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        // K‰sitell‰‰n voimapelletin syˆminen samalla tavalla kuin tavallisen pelletin syˆminen
        PelletEaten(pellet);

        // Peruutetaan kaikki peliajastimet
        CancelInvoke();

        // Aloitetaan uusi ajastin, joka nollaa haamujen kerroin-tilan pelokkuusajan j‰lkeen
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    // Tarkistaa, onko peliss‰ j‰ljell‰ syˆm‰ttˆmi‰ pellettej‰
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    // Nollaa haamujen kerroin
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
