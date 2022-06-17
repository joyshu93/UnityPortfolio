using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AITank : MonoBehaviour
{
    //AI
    public Transform target;
    NavMeshAgent nav;
    public bool isChase;

    //Audio
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    public AudioSource shootingAudio;
    public AudioClip fireClip;

    private Rigidbody m_Rigidbody;
    private float m_OriginalPitch;

    //Health
    public Slider m_ThirdPersonSlider;
    public Image m_ThirdPersonFillImage;
    public float m_StartingHealth = 100f;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;
    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead;

    //Fire
    public Rigidbody shell;
    public bool isAttack;
    public Transform fireTransform;


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);

        m_Rigidbody = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    //초기화
    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;

        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();

        Invoke("ChaseStart", 3f);
    }

    void ChaseStart()
    {
        isChase = true;
        isAttack = false;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
    }

    void Update()
    {
        if(nav.enabled)
        {
            nav.SetDestination(target.transform.position);
            nav.isStopped = !isChase;
        }

        EngineAudio();
    }

    void FreezeVelocity()
    {
        if(isChase)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void EngineAudio()
    {
        if (!isChase)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_PitchRange + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_PitchRange + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;

        SetHealthUI();

        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        m_ThirdPersonSlider.value = m_CurrentHealth;

        m_ThirdPersonFillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    private void OnDeath()
    {
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }

    private void Targeting()
    {
        float targetRadius = 0.5f;
        float targetRange = 20f;

        RaycastHit[] rayHits = 
            Physics.SphereCastAll(transform.position,
            targetRadius,
            transform.forward,
            targetRange,
            LayerMask.GetMask("Player"));

        if(rayHits.Length > 0  && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }
  
    IEnumerator Attack()
    {
        //발사
        isAttack = true;

        float distance = Vector3.Distance(target.position, transform.position);

        yield return  new WaitForSeconds(0.4f);

        isChase = false;

        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation);
        shellInstance.velocity = fireTransform.forward * distance;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        yield return new WaitForSeconds(2f);

        isChase = true;
        isAttack = false;
    }

    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

}
