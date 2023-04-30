using UnityEngine;

// M��ritell��n GhostChase-luokka GhostBehavior-luokan periytt�j�ksi
public class GhostChase : GhostBehavior
{
    // Kun GhostChase-luokka poistetaan k�yt�st�
    private void OnDisable()
    {
        // Otetaan k�ytt��n ghost.scatter-ominaisuus
        this.ghost.scatter.Enable();
    }

    // Kun jokin toinen collider t�rm�� t�h�n collideriin
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Haetaan noden tiedot colliderista
        Node node = other.GetComponent<Node>();

        // Jos node on l�ydetty, GhostChase-luokka on k�yt�ss� ja ghost.frightened.enabled ei ole p��ll�
        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            // Alustetaan suunta nollaksi ja minimiet�isyys suureksi
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // K�yd��n l�pi kaikki saatavilla olevat suunnat nodesta
            foreach (Vector2 availableDirections in node.availableDirections)
            {
                // Lasketaan uusi sijainti suunnan perusteella
                Vector3 newPosition = this.transform.position + new Vector3(availableDirections.x, availableDirections.y, 0.0f);

                // Lasketaan et�isyys kohteeseen
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                // Jos et�isyys on pienempi kuin minimiet�isyys
                if (distance < minDistance)
                {
                    // Asetetaan suunnaksi k�ytetty suunta ja minimiet�isyydeksi k�ytetty et�isyys
                    direction = availableDirections;
                    minDistance = distance;
                }
            }

            // Asetetaan suunta ghost.movement-ominaisuuteen
            this.ghost.movement.SetDirection(direction);
        }
    }
}
