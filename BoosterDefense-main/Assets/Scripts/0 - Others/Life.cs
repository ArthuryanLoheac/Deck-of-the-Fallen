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
    public bool isDead;
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
            if (hp < hpMax && isDead == false) {
                HPBar.SetActive(true);
            } else {
                HPBar.SetActive(false);
            }
        }
        fillHpBar.fillAmount = (float)hp / (float)hpMax;
    }

    private void DeathAlly()
    {
        this.transform.GetChild(0).GetChild(0).GetComponent<Animator>().SetBool("Dead", true); ;
        this.GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject, .8f);
    }
    private void DeathEnnemy()
    {
        this.transform.GetChild(0).GetComponent<Animator>().SetBool("Dead", true);
        this.GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject, 1.39f);
    }

    private void Death()
    {
        isDead = true;
        RessourceManager.instance.AddRessource(RessourceType.gold, coinsDropped);
        
        if (GetComponent<Base>() != null)
            GameManager.instance.Defeat();

        GetComponent<BuffsAndDebuffs>().ResetBuffs();
        

        if (GetComponent<IACollectRessources>() || GetComponent<IAAttackMonster>() || GetComponent<IAReparatorBuildings>())
            DeathAlly();
        else if (GetComponent<Enemy>() || GetComponent<EnemyKamikaze>())
            DeathEnnemy();
        else 
            Destroy(gameObject);

        UpdateHealthBar();
    }
    // Update is called once per frame
    void Update()
    {
        if (hp <= 0){
            if (!isDead)
                Death();
        } else {
            UpdateHealthBar();
            if (isBase) {
                BaseLifeManager.instance.life = hp;
                BaseLifeManager.instance.lifeMax = hpMax;
            }
        }
    }

    void OnDestroy()
    {
        if (PlaneManager.instance.surfaces[0] && GetComponent<NavMeshAgent>() == null)
            PlaneManager.instance.BakeSurface();
    }

     private void ActiveImageRecursive(GameObject obj)
    {
        if (obj.GetComponent<Image>()) obj.GetComponent<Image>().enabled = true;
        if (obj.GetComponent<Slider>()) obj.GetComponent<Slider>().enabled = true;
        for (int i = 0; i < obj.transform.childCount; i++) {
            Transform child = obj.transform.GetChild(i);
            if (child.GetComponent<Image>()) child.GetComponent<Image>().enabled = true;
            if (child.GetComponent<Slider>()) child.GetComponent<Slider>().enabled = true;
            
            ActiveImageRecursive(child.gameObject);
        }
    }

    private void StartBase()
    {
        HPBar = GameObject.FindGameObjectWithTag("HealthBarBase");
        fillHpBar = HPBar.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        UpdateHealthBar();

        //Active the visibility of the bar
        ActiveImageRecursive(HPBar);
        TimerCoolDown.instance.setIconWait(IconWaveType.Wait);
    }

    void Start()
    {
        isDead = false;
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
