using UnityEngine;

// Määritellään GhostChase-luokka GhostBehavior-luokan periyttäjäksi
public class GhostChase : GhostBehavior
{
    // Kun GhostChase-luokka poistetaan käytöstä
    private void OnDisable()
    {
        // Otetaan käyttöön ghost.scatter-ominaisuus
        this.ghost.scatter.Enable();
    }

    // Kun jokin toinen collider törmää tähän collideriin
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Haetaan noden tiedot colliderista
        Node node = other.GetComponent<Node>();

        // Jos node on löydetty, GhostChase-luokka on käytössä ja ghost.frightened.enabled ei ole päällä
        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            // Alustetaan suunta nollaksi ja minimietäisyys suureksi
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Käydään läpi kaikki saatavilla olevat suunnat nodesta
            foreach (Vector2 availableDirections in node.availableDirections)
            {
                // Lasketaan uusi sijainti suunnan perusteella
                Vector3 newPosition = this.transform.position + new Vector3(availableDirections.x, availableDirections.y, 0.0f);

                // Lasketaan etäisyys kohteeseen
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                // Jos etäisyys on pienempi kuin minimietäisyys
                if (distance < minDistance)
                {
                    // Asetetaan suunnaksi käytetty suunta ja minimietäisyydeksi käytetty etäisyys
                    direction = availableDirections;
                    minDistance = distance;
                }
            }

            // Asetetaan suunta ghost.movement-ominaisuuteen
            this.ghost.movement.SetDirection(direction);
        }
    }
}
