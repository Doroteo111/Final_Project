using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //movment
    private float verticalInput;
    private float moveSpeed;

    //speed
    public float walkSpeed = 8f;
    public float runSpeed= 10f;

    public float jumpForce = 10f;

    public float mouseSensitivity;

    [SerializeField] private bool isOnTheGround;
    private Rigidbody _rigidbody;
    private Animator _animator;

    //powerups
    public bool hasPowerupLife;
    public int lives = 3;
    public float timer = 0;
    //UI
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI _timer;
    public GameObject winPanel;
    //public GameObject winPanelLevel2;
    public int Counter;
    private AudioSource _audioSource;
    public AudioClip[] collectables;
    private AudioSource _audioAttack;
    public AudioClip[] attackSounds;
    public AudioClip errorGem;

    [SerializeField] private BoxCollider swordCollider;
    public ParticleSystem dirtParticle;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        // Cursor.lockState = CursorLockMode.Locked; //press [esc] to exit the mode  
        _audioSource = GetComponent<AudioSource>();
        winPanel.SetActive(false);
    }
    

    private void Update()
    {
        //  Time
        CountDown();


        Movment();

        if (Input.GetKeyDown(KeyCode.Mouse0)) //left botton down
        {
            Attack();
        }
        else
        {
            swordCollider.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnTheGround ) //salto con el espacio 
        {
            Jump();
        }

        SaltoEscena();
    }

    private void OnCollisionEnter(Collision otherCollider) //collider ground
    {
        if (otherCollider.gameObject.Equals("Ground"))
        {
            isOnTheGround = true;
        }

  
    }


    private void Movment()
    {
       
        verticalInput = Input.GetAxis("Vertical");
       transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * verticalInput); //right,horizontal

        float mouseX = Input.GetAxis("Mouse X"); //mouse rotation
                                                 
        transform.Rotate(Vector3.up, mouseSensitivity * mouseX * Time.deltaTime);

        if (verticalInput != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && TWOstaminaMaria.instance.hasStamina)
            {
                Run();
            }
            else
            {
                Walk();
            }
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        _animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        TWOstaminaMaria.instance.RegenStamina(15);
        dirtParticle.Stop();
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        _animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        TWOstaminaMaria.instance.RegenStamina(15);
        dirtParticle.Play();
    }
    private void Run()
    {
        moveSpeed = runSpeed;
        _animator.SetFloat("Speed", 1f, 0.1f,Time.deltaTime); //adding the smooth
        TWOstaminaMaria.instance.UseStamina(40);
        dirtParticle.Play();
    }
    private void Jump()
    {
        isOnTheGround = false;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        dirtParticle.Stop();

    }

    private void Attack()
    {
        _animator.SetInteger("Attack_type", Random.Range(1, 3));
        _animator.SetTrigger("Attack");
        dirtParticle.Stop();
        swordCollider.enabled = true;
        ChooseRandomSFX(attackSounds);
    }

    private void TakeDamage()
    {
        lives = 3;
    }

    private void GameOver()
    {
        if(lives <= 0)
        {
            //_animator.SetBool("Death");
        }
    }

    private void ChooseRandomSFX(AudioClip[] sounds)
    {
      int randomIdx = Random.Range(0, sounds.Length);
      _audioSource.PlayOneShot(sounds[randomIdx], 1);
    }


private void GetCoins(Collider other) // Destroy the collectable
    {
        Destroy(other.gameObject);
        Counter++;
        counterText.text = $"Coins: {Counter}";
        _audioSource.PlayOneShot(collectables[1]);

    }

    private void GetPowerUp(Collider other)
    {
        Destroy(other.gameObject);
        lives++;
        livesText.text = $"Lives: {lives}";
        hasPowerupLife = true;
        _audioSource.PlayOneShot(collectables[2]);
    }

    public void GetGemFinish(Collider other)
    {
        if (Counter >= 10)
        {
            Destroy(other.gameObject);
            winPanel.SetActive(true);
            SaltoEscena();
            timer = 60;
            _audioSource.PlayOneShot(collectables[3]);
        }
        else
        {
            _audioSource.PlayOneShot(errorGem);
        }

        if(Counter >= 12)
        {
            Destroy(other.gameObject);
            winPanel.SetActive(true);
            timer = 120;
            _audioSource.PlayOneShot(collectables[3]);
        }
        else
        {
            _audioSource.PlayOneShot(errorGem);
        }
    }

    private void GetHourGlass(Collider other)
    {
        Destroy(other.gameObject);
        _audioSource.PlayOneShot(collectables[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Coin"))
        {
            GetCoins(other);
        }
        else if (other.gameObject.tag.Equals("Power Up"))
        {
            GetPowerUp(other);
        }
        else if (other.gameObject.tag.Equals("Finish Level"))
        {
            GetGemFinish(other);
        }
        else if (other.gameObject.tag.Equals("Timer"))
        {
            GetHourGlass(other);
            timer += 10;
        } else if (other.gameObject.tag.Equals("Enemy"))
        {
            //TODO: comprobar que la espada destruye al enemigo
        }
    }

    private void CountDown()
    {
        timer -= Time.deltaTime;
        _timer.text = "" + timer.ToString("f1");

        if (timer <= 0)
        {
            timer = 0;
        }
    }

    public void SaltoEscena()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("Level2");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene("Credits");
        }
    }
}
