using UnityEngine;

[RequireComponent(typeof(Ghost))]

// "GhostBehavior" on abstrakti luokka, joka vaatii "Ghost" komponentin.
public abstract class GhostBehavior : MonoBehaviour
{
    // "Ghost" ominaisuus (property) on julkinen ja sen asettaminen on rajattu yksityiseksi.
    public Ghost ghost { get; private set; }

    // Kesto m‰‰ritell‰‰n liukulukuna.
    public float duration;

    // Awake() metodi suoritetaan heti, kun skripti ladataan ja sen tarkoitus on m‰‰ritell‰ "ghost" ominaisuus ja asettaa skriptin tila pois p‰‰lt‰ (disabled).
    private void Awake()
    {
        this.ghost = GetComponent<Ghost>();
        this.enabled = false;
    }

    // "Enable()" metodi mahdollistaa skriptin tilan muutoksen p‰‰lle.
    public void Enable()
    {
        Enable(this.duration);
    }

    // "Enable(float duration)" metodi mahdollistaa skriptin tilan muutoksen p‰‰lle ja m‰‰rittelee keston, jonka j‰lkeen skriptin tila muutetaan pois p‰‰lt‰.
    public virtual void Enable(float duration)
    {
        this.enabled = true;
        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }

    // "Disable()" metodi muuttaa skriptin tilan pois p‰‰lt‰ ja peruuttaa kestotetut kutsut.
    public virtual void Disable()
    {
        this.enabled = false;

        CancelInvoke();
    }
}
