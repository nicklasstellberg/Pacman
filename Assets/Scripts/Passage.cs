using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;

    // Kun toinen Collider2D törmää tähän GameObjectiin
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Otetaan toisen GameObjectin sijainti
        Vector3 position = other.transform.position;

        // Asetetaan x- ja y-koordinaatit yhtä suuriksi kuin yhteyden sijainnin vastaavat koordinaatit
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;

        // Asetetaan toisen GameObjectin sijainti vastaamaan yhteyden sijaintia
        other.transform.position = position;
    }
}
