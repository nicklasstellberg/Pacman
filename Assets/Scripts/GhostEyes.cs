using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    // Movement-skriptin viite
    public Movement movement { get; private set; }

    // suuntien katsomisen spritet
    public Sprite up;

    public Sprite down;

    public Sprite left;

    public Sprite right;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        // Haetaan Movement-skripti objektin vanhemmalta
        this.movement = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        // Jos liikkumissuunta on ylös
        if (this.movement.direction == Vector2.up)
        {
            // Asetetaan ylöspäin katsomisen sprite näkyviin jne.
            this.spriteRenderer.sprite = this.up;
        } else if (this.movement.direction == Vector2.down)
        {
            this.spriteRenderer.sprite = this.down;
        } else if (this.movement.direction == Vector2.left)
        {
            this.spriteRenderer.sprite = this.left;
        } else if (this.movement.direction == Vector2.right)
        {
            this.spriteRenderer.sprite = this.right;
        } 
    }
}
