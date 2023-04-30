using UnityEngine;

// Tämä on GhostFrightened-luokka, joka perii GhostBehavior-luokan.
public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body; // SpriteRenderer, joka kuvailee aaveen kehoa.
    public SpriteRenderer eyes; // SpriteRenderer, joka kuvailee aaveen silmiä.
    public SpriteRenderer blue; // SpriteRenderer, joka kuvailee aaveen sinistä väriä.
    public SpriteRenderer white; // SpriteRenderer, joka kuvailee aaveen valkoista väriä.

    public bool eaten { get; private set; } // Muuttuja, joka kertoo, onko aave syöty vai ei.

    public override void Enable(float duration) // Ylikirjoitetaan Enable-metodi GhostBehavior-luokasta.
    {
        base.Enable(duration); // Kutsuu yliluokan Enable-metodia.

        this.body.enabled = false; // Asettaa kehon näkymättömäksi.
        this.eyes.enabled = false; // Asettaa silmät näkymättömiksi.
        this.blue.enabled = true; // Asettaa sinisen värin näkyväksi.
        this.white.enabled = false; // Asettaa valkoisen värin näkymättömäksi.

        Invoke(nameof(Flash), duration / 2.0f); // Kutsuu Flash-metodia, joka suoritetaan väliajalla duration/2.
    }

    public override void Disable() // Ylikirjoitetaan Disable-metodi GhostBehavior-luokasta.
    {
        base.Disable(); // Kutsuu yliluokan Disable-metodia.

        this.body.enabled = true; // Asettaa kehon näkyväksi.
        this.eyes.enabled = true; // Asettaa silmät näkyväksi.
        this.blue.enabled = false; // Asettaa sinisen värin näkymättömäksi.
        this.white.enabled = false; // Asettaa valkoisen värin näkymättömäksi.
    }

    private void Flash() // Metodi, joka väläyttää aaveen valkoiseksi väriksi.
    {
        if (!this.eaten) // Tarkistaa, että aave ei ole syöty.
        {
            this.blue.enabled = false; // Asettaa sinisen värin näkymättömäksi.
            this.white.enabled = true; // Asettaa valkoisen värin näkyväksi.
            this.white.GetComponent<AnimatedSprite>().Restart(); // Käynnistää valkoisen animaation uudestaan.
        }

    }

    private void Eaten()
    {
        // Asettaa eaten-muuttujan arvoksi true, jotta tiedetään, että aave on syöty.
        this.eaten = true;

        // Asettaa aaveen sijainniksi häkin sisäosan sijainnin, jotta aave palaa häkkiin.
        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        // Käynnistää häkin, jotta aave voi tulla sieltä ulos taas tietyn ajan kuluttua.
        this.ghost.home.Enable(this.duration);

        // Piilottaa aaveen värjätyn version ja näyttää aaveen silmät sen sijaan.
        this.body.enabled = false;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void OnEnable()
    {
        // Asettaa aaveen nopeusmonikertoimen puolikkaaksi, jotta se liikkuu hitaammin kun se on pelästynyt.
        this.ghost.movement.speedMultiplier = 0.5f;

        // Asettaa eaten-muuttujan arvoksi false, jotta tiedetään, että aave ei ole syöty.
        this.eaten = false;
    }

    private void OnDisable()
    {
        // Palauttaa aaveen nopeusmonikertoimen takaisin normaaliksi, kun pelästymistila on poistunut.
        this.ghost.movement.speedMultiplier = 1.0f;

        // Asettaa eaten-muuttujan arvoksi false, jotta tiedetään, että aave ei ole syöty.
        this.eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Tarkistaa, onko aave törmännyt Pacmaniin ja onko sen pelästymistila päällä.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.enabled)
            {
                // Jos aave on pelästynyt ja törmää Pacmaniin, aave on syöty.
                Eaten();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tarkistaa, onko aave törmännyt solmuun ja onko sen pelästymistila päällä.
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            // Etsii suunnan, joka on pisimmällä etäisyydellä aaveen tavoitesijainnista ja asettaa aaveen liikkeen kohti sitä suuntaa.
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirections in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirections.x, availableDirections.y, 0.0f);
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirections;
                    maxDistance = distance;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }
}
