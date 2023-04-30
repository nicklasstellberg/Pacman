using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;

    // Metodi suoritetaan, kun t‰m‰ k‰ytt‰ytymismalli aktivoidaan
    private void OnEnable()
    {
        StopAllCoroutines(); // Lopetetaan kaikki aiemmin k‰ynniss‰ olleet rutiinit
    }

    // Metodi suoritetaan, kun t‰m‰ k‰ytt‰ytymismalli deaktivoidaan
    private void OnDisable()
    {
        // Tarkistetaan, onko peliobjekti edelleen aktiivinen, jotta v‰ltyt‰‰n virheelt‰, kun objekti tuhotaan
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition()); // Aloita "ulosk‰ynnin" animointi
        }
    }

    // Metodi suoritetaan, kun aave tˆrm‰‰ sein‰‰n
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // K‰‰nnet‰‰n aaveen suuntaa joka kerta, kun se osuu sein‰‰n luodaksemme vaikutelman pomppivasta aaveesta kodissaan
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    // Metodi, joka suorittaa "ulosk‰ynnin" animoinnin rutiinin
    private IEnumerator ExitTransition()
    {
        // Sammutetaan liike, jotta voimme animoida aaveen manuaalisesti
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rigidbody.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f; // Animoinnin kesto
        float elapsed = 0f;

        // Animaatio aaveen l‰htˆpisteeseen
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Animaatio ulosk‰ynnist‰
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Valitaan satunnainen suunta vasemmalle tai oikealle ja otetaan liike taas k‰yttˆˆn
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        ghost.movement.rigidbody.isKinematic = false;
        ghost.movement.enabled = true;
    }
}
