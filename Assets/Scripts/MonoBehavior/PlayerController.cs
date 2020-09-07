using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController: MonoBehaviour
{
    public GameObject DeathParticle;
    public GameObject PickupParticle;
    public Slider HealthBar;
    public LevelManager lm;
    public LineRenderer lr;

    public static KeyCode actionKey = KeyCode.Space;

    [Header("Health variable")]
    public float maxHealth;
    public float currentHealth;
    public float damageCooldown;

    [Header("Basic Movement variables")]
    public float chargeMax;
    public float chargeFalloff;
    public float chargeSpeed;
    public float dashPower;
    public float rotationSpeed;
    public float projectorCoff;
    public float dashCancelThreshold;
    public AnimationCurve projectorDisappear;

    [Header("Alternative Movement variables")]
    public float chargeMultiplier;
    public float aMRotationSpeed;
    bool alternativeMovement;
    

    public float charge = 0;
    bool dashing = false;
    float cancelationTime;
    bool dashCanceled = false;

    [Header("Danger zone variables")]
    public float slowMultiplier;
    public float weakenMultiplier;
    public float damagePerTick;
    public float shakePerTick;
    bool insideDanger = false;

    [Header("TimeWarp zone varbiables")]
    public float slowedCofficient;


    [Header("Boost zone variables")]
    public float chargeBoost;

    CameraShake cs;
    bool dashStart = false;

    private void Awake()
    {
        cs = FindObjectOfType<Camera>().GetComponent<CameraShake>();
        HealthBar.maxValue = maxHealth;
        Time.timeScale = 1;
    }
    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        Health();
        if(alternativeMovement)
        {
            AlternativeMovement();
        }
        else
        {
            BasicMovement();
        }
    }
    private void FixedUpdate()
    {
    }
    void UpdateUI()
    {
        HealthBar.value = currentHealth;
    }

    void Health()
    {
        if(insideDanger)
        {
            cs.Shake(shakePerTick * Time.deltaTime);
            TakeDamage(Time.deltaTime * damagePerTick);
        }
    }
    void AlternativeMovement()
    {
        transform.Translate(new Vector2(0,charge*chargeMultiplier * Time.deltaTime));
        if(Input.GetKey(actionKey))
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * aMRotationSpeed));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * -aMRotationSpeed));
        }
    }

    void BasicMovement()
    {
        // When player is dashing
        if (charge > 0 && !Input.GetKey(actionKey) || dashing)
        {
            //Debug.Log("Dashing");
            if(!dashStart)
            {
                dashStart = true;
                AudioManager.PlaySound("Dash",0.2f,0);
            }
            cancelationTime = 0;
            lr.SetPosition(1, new Vector3(0, 0));
            dashing = true;
            transform.Translate(new Vector2(0, dashPower*charge * Time.deltaTime));

            charge = Mathf.Clamp(charge - chargeFalloff * Time.deltaTime, 0, chargeMax);
            if(charge == 0)
            {
                dashing = false;
                dashStart = false;
            }
        }
        // When player is charging
        else if(Input.GetKey(actionKey) && !dashing && !dashCanceled)
        {
            //Debug.Log("Charging");
            float distance = charge * charge * chargeFalloff * 0.5f;
            Vector3 forward = transform.TransformDirection(Vector3.up);
            Debug.DrawRay(transform.position, forward*distance, Color.red, 0.5f);

            charge = Mathf.Clamp(charge + chargeSpeed * Time.deltaTime, 0, chargeMax);
            if(charge == chargeMax)
            {
                cancelationTime += Time.deltaTime;
                Color tmp = lr.startColor;
                tmp.a = projectorDisappear.Evaluate(cancelationTime/dashCancelThreshold);
                lr.startColor = tmp;
                if (cancelationTime >= dashCancelThreshold)
                {
                    charge = 0;
                    cancelationTime = 0;
                    dashCanceled = true;
                    Color tmp1 = lr.startColor;
                    tmp1.a = projectorDisappear.Evaluate(0);
                    lr.startColor = tmp1;
                }
            }
            lr.SetPosition(1, new Vector3(0, projectorCoff * charge));
        }
        // When player is idle spinning
        else if(charge == 0)
        {
            //Debug.Log("Idle");
            cancelationTime = 0;
            dashing = false;
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotationSpeed));
        }

        if(Input.GetKeyUp(actionKey))
        {
            cancelationTime = 0;
            dashCanceled = false;
        }
    }

    public void Pickup()
    {
        Instantiate(PickupParticle, transform.position, Quaternion.identity);
        Heal(40);
    }

    public void Die()
    {
        Instantiate(DeathParticle,transform.position,Quaternion.identity);
        cs.Shake(2);
        lm.Death();
        HealthBar.value = 0;
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "DangerZone":
                dashPower /= weakenMultiplier;
                insideDanger = true;
                break;
            case "BoostZone":
                cs.Shake(0.5f);
                charge += chargeBoost;
                break;
            case "DeathZone":
                if(!lm.levelComplete)
                {
                    Die();
                }
                break;
            case "ExitZone":
                dashPower /= 5;
                break;
            case "TimeWarpZone":
                Time.timeScale = slowedCofficient;
                break;
            case "AlternativeZone":
                alternativeMovement = true;
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "DangerZone":
                dashPower *= weakenMultiplier;
                insideDanger = false;
                break;
            case "TimeWarpZone":
                Time.timeScale = 1;
                break;
            case "AlternativeZone":
                alternativeMovement = false;
                break;
        }
    }
}
