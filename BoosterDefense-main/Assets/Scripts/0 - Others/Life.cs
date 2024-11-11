using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public enum TypeDamage {
    Normal,
    Heal,
    Fire,
    Cold
}

[RequireComponent(typeof(BuffsAndDebuffs))]
public class Life : MonoBehaviour
{
    public float hp;
    [HideInInspector]public float hpMax;
    public GameObject HPBarprefabs;
    private GameObject HPBar;
    private Image fillHpBar;
    private float sizeY;
    [Header("Coins")]
    [HideInInspector]public int coinsDropped;
    [Header("Pop Up")]
    public GameObject DamagePopUp;

    private bool isBase;


    public void TakeDamage(float amount, TypeDamage typeDamage = TypeDamage.Normal)
    {
        if (GetComponent<BuffsAndDebuffs>().isBuffActive(TypeBuffs.Invincibility))
            return;
        if (typeDamage == TypeDamage.Heal)
            amount *= -1;
        hp -= amount;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + sizeY, transform.position.z);
        Instantiate(DamagePopUp, vec, Quaternion.identity).GetComponent<SetDamagePopUp>().SetValueText(amount, typeDamage);
    }

    private void UpdateHealthBar()
    {
        if (!isBase){
            if (hp < hpMax) {
                HPBar.SetActive(true);
            } else {
                HPBar.SetActive(false);
            }
        }
        fillHpBar.fillAmount = (float)hp / (float)hpMax;
    }

    private void Death()
    {
        if (PlaneManager.instance.surfaces[0] != null) {
            RessourceManager.instance.AddRessource(RessourceType.gold, coinsDropped);
        }
        if (GetComponent<Base>() != null)
            GameManager.instance.Defeat();
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if(hp <= 0){
            Death();
        } else {
            UpdateHealthBar();
        }
    }

    void OnDestroy()
    {
        if (PlaneManager.instance.surfaces[0] && GetComponent<NavMeshAgent>() == null)
            PlaneManager.instance.BakeSurface();
    }

    private void StartBase()
    {
        HPBar = GameObject.FindGameObjectWithTag("HealthBarBase");
        fillHpBar = HPBar.transform.GetChild(2).GetComponent<Image>();
        UpdateHealthBar();

        //Active the visibility of the bar
        HPBar.GetComponent<Canvas>().enabled = true;
    }

    void Start()
    {
        hpMax = hp;
        isBase = GetComponent<Base>() != null;
        sizeY = GetComponent<Collider>().bounds.size.y + 1f;

        if (isBase) {
            //si c'est la base défini les parametres
            StartBase();
        } else {
            //défini parametre de base
            Vector3 vec = new Vector3(transform.position.x, transform.position.y + sizeY, transform.position.z);
            HPBar = Instantiate(HPBarprefabs, vec, Quaternion.identity, transform);
            fillHpBar = HPBar.transform.GetChild(2).GetComponent<Image>();
            UpdateHealthBar();
        }
    }
}
