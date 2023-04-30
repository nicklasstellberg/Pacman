using UnityEngine;

// T�m� on GhostFrightened-luokka, joka perii GhostBehavior-luokan.
public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body; // SpriteRenderer, joka kuvailee aaveen kehoa.
    public SpriteRenderer eyes; // SpriteRenderer, joka kuvailee aaveen silmi�.
    public SpriteRenderer blue; // SpriteRenderer, joka kuvailee aaveen sinist� v�ri�.
    public SpriteRenderer white; // SpriteRenderer, joka kuvailee aaveen valkoista v�ri�.

    public bool eaten { get; private set; } // Muuttuja, joka kertoo, onko aave sy�ty vai ei.

    public override void Enable(float duration) // Ylikirjoitetaan Enable-metodi GhostBehavior-luokasta.
    {
        base.Enable(duration); // Kutsuu yliluokan Enable-metodia.

        this.body.enabled = false; // Asettaa kehon n�kym�tt�m�ksi.
        this.eyes.enabled = false; // Asettaa silm�t n�kym�tt�miksi.
        this.blue.enabled = true; // Asettaa sinisen v�rin n�kyv�ksi.
        this.white.enabled = false; // Asettaa valkoisen v�rin n�kym�tt�m�ksi.

        Invoke(nameof(Flash), duration / 2.0f); // Kutsuu Flash-metodia, joka suoritetaan v�liajalla duration/2.
    }

    public override void Disable() // Ylikirjoitetaan Disable-metodi GhostBehavior-luokasta.
    {
        base.Disable(); // Kutsuu yliluokan Disable-metodia.

        this.body.enabled = true; // Asettaa kehon n�kyv�ksi.
        this.eyes.enabled = true; // Asettaa silm�t n�kyv�ksi.
        this.blue.enabled = false; // Asettaa sinisen v�rin n�kym�tt�m�ksi.
        this.white.enabled = false; // Asettaa valkoisen v�rin n�kym�tt�m�ksi.
    }

    private void Flash() // Metodi, joka v�l�ytt�� aaveen valkoiseksi v�riksi.
    {
        if (!this.eaten) // Tarkistaa, ett� aave ei ole sy�ty.
        {
            this.blue.enabled = false; // Asettaa sinisen v�rin n�kym�tt�m�ksi.
            this.white.enabled = true; // Asettaa valkoisen v�rin n�kyv�ksi.
            this.white.GetComponent<AnimatedSprite>().Restart(); // K�ynnist�� valkoisen animaation uudestaan.
        }

    }

    private void Eaten()
    {
        // Asettaa eaten-muuttujan arvoksi true, jotta tiedet��n, ett� aave on sy�ty.
        this.eaten = true;

        // Asettaa aaveen sijainniksi h�kin sis�osan sijainnin, jotta aave palaa h�kkiin.
        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        // K�ynnist�� h�kin, jotta aave voi tulla sielt� ulos taas tietyn ajan kuluttua.
        this.ghost.home.Enable(this.duration);

        // Piilottaa aaveen v�rj�tyn version ja n�ytt�� aaveen silm�t sen sijaan.
        this.body.enabled = false;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void OnEnable()
    {
        // Asettaa aaveen nopeusmonikertoimen puolikkaaksi, jotta se liikkuu hitaammin kun se on pel�stynyt.
        this.ghost.movement.speedMultiplier = 0.5f;

        // Asettaa eaten-muuttujan arvoksi false, jotta tiedet��n, ett� aave ei ole sy�ty.
        this.eaten = false;
    }

    private void OnDisable()
    {
        // Palauttaa aaveen nopeusmonikertoimen takaisin normaaliksi, kun pel�stymistila on poistunut.
        this.ghost.movement.speedMultiplier = 1.0f;

        // Asettaa eaten-muuttujan arvoksi false, jotta tiedet��n, ett� aave ei ole sy�ty.
        this.eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Tarkistaa, onko aave t�rm�nnyt Pacmaniin ja onko sen pel�stymistila p��ll�.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.enabled)
            {
                // Jos aave on pel�stynyt ja t�rm�� Pacmaniin, aave on sy�ty.
                Eaten();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tarkistaa, onko aave t�rm�nnyt solmuun ja onko sen pel�stymistila p��ll�.
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            // Etsii suunnan, joka on pisimm�ll� et�isyydell� aaveen tavoitesijainnista ja asettaa aaveen liikkeen kohti sit� suuntaa.
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
