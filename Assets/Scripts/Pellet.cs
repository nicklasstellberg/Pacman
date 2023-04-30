using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10; // pistem��r�, jonka pelaaja saa sy�dess��n t�m�n pelletin

    // Eat-metodi kutsuu GameManager-luokan PelletEaten-metodia, joka k�sittelee pelletin sy�nnin
    protected virtual void Eat()
    {
        FindObjectOfType<GameManager>().PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tarkistetaan, onko t�rm�nnyt objekti Pacmaniin, joka on m��ritelty "Pacman"-layerissa
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat(); // Jos Pacman osuu pellettiin, kutsutaan Eat-metodia
        }
    }
}
