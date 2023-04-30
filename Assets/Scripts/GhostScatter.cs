using UnityEngine;

public class GhostScatter : GhostBehavior
{
    // Kun t�m� skripti poistetaan k�yt�st�, kummitus alkaa j�lleen tavoittamaan pelaajaa.
    private void OnDisable()
    {
        this.ghost.chase.Enable();
    }

    // Jos kummitus t�rm�� solmuun, se valitsee satunnaisen suunnan liikkua.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            int index = Random.Range(0, node.availableDirections.Count);

            // Varmistetaan, ett� kummitus ei k��nn� 180 astetta, paitsi jos se on pakko.
            if (node.availableDirections[index] == -this.ghost.movement.direction && node.availableDirections.Count > 1)
            {
                index++;

                if(index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            this.ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
