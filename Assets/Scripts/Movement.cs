using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    // Liikkumisnopeus
    public float speed = 8f;
    // Liikkumisnopeuden monistaja, k�ytet��n pelin mekaniikassa
    public float speedMultiplier = 1f;
    // Liikkumissuunta kun peli aloitetaan
    public Vector2 initialDirection;
    // Esteiden kerros, jotta tarkistetaan, voiko hahmo liikkua kyseiseen suuntaan
    public LayerMask obstacleLayer;

    // Hahmon Rigidbody-komponentti, k�ytet��n liikkumisessa
    public new Rigidbody2D rigidbody { get; private set; }
    // Hahmon liikkumissuunta
    public Vector2 direction { get; private set; }
    // Seuraava liikkumissuunta, jos pelaaja on asettanut sen ennen kuin suunta on mahdollinen
    public Vector2 nextDirection { get; private set; }
    // Hahmon aloituspaikka
    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        // Haetaan hahmon Rigidbody-komponentti
        rigidbody = GetComponent<Rigidbody2D>();
        // Tallennetaan aloituspaikka
        startingPosition = transform.position;
    }

    private void Start()
    {
        // Alustetaan hahmon tila pelin alussa
        ResetState();
    }

    public void ResetState()
    {
        // Palautetaan liikkumisnopeuden monistaja oletusarvoon
        speedMultiplier = 1f;
        // Asetetaan liikkumissuunta hahmon aloitussuunnaksi
        direction = initialDirection;
        // Nollataan seuraava liikkumissuunta
        nextDirection = Vector2.zero;
        // Asetetaan hahmo aloituspaikalle
        transform.position = startingPosition;
        // Otetaan Rigidbody-komponentin fysiikkasimulaatio k�ytt��n
        rigidbody.isKinematic = false;
        // Otetaan t�m� skripti k�ytt��n
        enabled = true;
    }

    private void Update()
    {
        // Yritet��n liikkua seuraavaan suuntaan, jos sellainen on asetettu
        // jotta liikkuminen on herkemp��
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        // Liikutetaan hahmoa sen nykyisen liikkumissuunnan mukaan
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Aseta suunta vain, jos kyseisess� suunnassa oleva ruutu on k�ytett�viss�
        // muuten asetamme sen seuraavana suuntana, joten se asetetaan automaattisesti, kun se tulee saataville
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        // Jos ei ole t�rm�yst�, niin kyseisess� suunnassa ei ole estett�
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

}
