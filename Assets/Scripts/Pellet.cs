using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10; // pistemäärä, jonka pelaaja saa syödessään tämän pelletin

    // Eat-metodi kutsuu GameManager-luokan PelletEaten-metodia, joka käsittelee pelletin syönnin
    protected virtual void Eat()
    {
        FindObjectOfType<GameManager>().PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tarkistetaan, onko törmännyt objekti Pacmaniin, joka on määritelty "Pacman"-layerissa
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat(); // Jos Pacman osuu pellettiin, kutsutaan Eat-metodia
        }
    }
}
