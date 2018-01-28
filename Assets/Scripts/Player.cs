using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;
    public Text FoodText;
    public AudioClip MoveSound1;
    public AudioClip MoveSound2;
    public AudioClip EatSound1;
    public AudioClip EatSound2;
    public AudioClip DrinkSound1;
    public AudioClip DrinkSound2;
    public AudioClip GameOverSound;

    private Animator _animator;
    private int _food;

    public void LoseFood(int loss)
    {
        _animator.SetTrigger("playerHit");
        _food -= loss;
        FoodText.text = "-" + loss + " Food: " + _food;
        CheckIfGameOver();
    }

    // Use this for initialization
    protected override void Start()
    {
        _animator = GetComponent<Animator>();

        _food = GameManager.instance.PlayerFoodPoints;

        FoodText.text = "Food: " + _food;

        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        _food--;
        FoodText.text = "Food: " + _food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(MoveSound1, MoveSound2);
        }

        CheckIfGameOver();

        GameManager.instance.PlayersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(WallDamage);
        _animator.SetTrigger("playerChop");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.instance.PlayersTurn)
            return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement
        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
    }

    private void OnDisable()
    {
        GameManager.instance.PlayerFoodPoints = _food;
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
        {
            GameManager.instance.GameOver();
            SoundManager.instance.PlaySingle(GameOverSound);
            SoundManager.instance.MusicSource.Stop();
        }
            
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            _food += PointsPerFood;
            FoodText.text = "+" + PointsPerFood + " Food: " + _food;
            SoundManager.instance.RandomizeSfx(EatSound1, EatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            _food += PointsPerSoda;
            FoodText.text = "+" + PointsPerSoda + " Food: " + _food;
            SoundManager.instance.RandomizeSfx(DrinkSound1, DrinkSound2);
            other.gameObject.SetActive(false);
        }
    }
}