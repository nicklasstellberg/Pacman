using UnityEngine;

public class PowerPellet : Pellet
{
    // Ajanjakso, jonka PowerPellet vaikuttaa.
    public float duration = 8.0f;

    // Ylikirjoitetaan Pellet-luokan Eat()-metodi.
    protected override void Eat()
    {
        // Etsit‰‰n GameManager-luokan olio ja kutsutaan sen PowerPelletEaten()-metodia.
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
