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

        // Jos haamun alkuper�inen k�ytt�ytyminen ei ole GhostHome, disabloi se.
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
        // Pid� z-akselin koordinaatti samana, koska se m��ritt�� piirtosyvyyden.
        position.z = transform.position.z;

        // Aseta haamun sijainti.
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jos t�rm�t��n Pacmaniin, tarkista tilanne.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Jos GhostFrightened-komponentti on p��ll�, ilmoita GameManagerille ett� haamu sy�tiin.
            if (frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            // Muussa tapauksessa ilmoita ett� Pacman sy�tiin.
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }

}
