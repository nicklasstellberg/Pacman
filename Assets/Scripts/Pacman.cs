using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Movement))]

public class Pacman : MonoBehaviour
{
    public Movement movement { get; private set;}

    // Suunta-napit
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    private void Awake()
    {
        this.movement = GetComponent<Movement>(); // Haetaan Movement-komponentti
        // Lisätään kuuntelija ylös-napille, joka kutsuu MoveUp-metodia jne.
        upButton.onClick.AddListener(MoveUp);
        downButton.onClick.AddListener(MoveDown);
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
    }

    public void MoveUp()// Metodi, joka kutsutaan ylös-nappia painettaessa jne.
    {
        this.movement.SetDirection(Vector2.up); // Asetetaan Movement-komponentin suunnaksi ylös jne.
    }

    public void MoveDown()
    {
        this.movement.SetDirection(Vector2.down);
    }

    public void MoveLeft()
    {
        this.movement.SetDirection(Vector2.left);
    }

    public void MoveRight()
    {
        this.movement.SetDirection(Vector2.right);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))// Jos painetaan "W" tai nuolta ylös jne.
        {
            this.movement.SetDirection(Vector2.up); // Asetetaan Movement-komponentin suunnaksi ylös jne.
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.movement.SetDirection(Vector2.left);
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.movement.SetDirection(Vector2.right);
        }
        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState() // Metodi, joka nollaa Pacmanin tilan ja aktivoi sen
    {
        this.movement.ResetState(); // Kutsutaan Movement-komponentin ResetState-metodia
        this.gameObject.SetActive(true);
    }
}
