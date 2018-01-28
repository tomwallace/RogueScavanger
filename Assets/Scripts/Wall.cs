using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite DamageSprite;
    public int HitPoints = 4;
    public AudioClip ChopSound1;
    public AudioClip ChopSound2;

    private SpriteRenderer _spriteRenderer;

    public void DamageWall(int loss)
    {
        _spriteRenderer.sprite = DamageSprite;
        HitPoints -= loss;

        SoundManager.instance.RandomizeSfx(ChopSound1, ChopSound2);

        if (HitPoints <= 0)
            gameObject.SetActive(false);
    }

    // Use this for initialization
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}