using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        // Hakee komponentit Awake-vaiheessa.
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        // Alustaa tilan Start-vaiheessa.
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        // Jos haamun alkuperäinen käyttäytyminen ei ole GhostHome, disabloi se.
        if (home != initialBehavior)
        {
            home.Disable();
        }

        // Jos initialBehavior ei ole null, aktivoi se.
        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Pidä z-akselin koordinaatti samana, koska se määrittää piirtosyvyyden.
        position.z = transform.position.z;

        // Aseta haamun sijainti.
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jos törmätään Pacmaniin, tarkista tilanne.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Jos GhostFrightened-komponentti on päällä, ilmoita GameManagerille että haamu syötiin.
            if (frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            // Muussa tapauksessa ilmoita että Pacman syötiin.
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }

}
