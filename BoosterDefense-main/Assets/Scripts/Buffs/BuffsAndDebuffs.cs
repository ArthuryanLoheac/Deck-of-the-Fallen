using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum TypeBuffs {
    Heal,
    Fire,
    Invincibility,
    Slow,
    None,
}

public enum TypeMore {
    Stack,
    Reset,
    Add,
    NotAdd
}

public class Buffs {
    public TypeBuffs type;
    public float timeEnd;
    public float nextTimeEffect;
    public float timeBetweenTwo;
    public TypeMore typeMore;
    public float force = 1;
}

[CreateAssetMenu(fileName = "new Imunities", menuName = "Imunities")]
public class Imunities: ScriptableObject{
    public TypeBuffs[] lstImunities;
}

public class BuffsAndDebuffs : MonoBehaviour
{
    [SerializeField]
    List<Buffs> buffs = new List<Buffs>();
    private bool fireFXactive = false;
    private GameObject FX_Fire_Inst;
    public Imunities imunities;

    #region Effect

    private void Effect_Heal(Buffs buff)
    {
        GetComponent<Life>().TakeDamage(buff.force, TypeDamage.Heal);
    }

    private void Effect_Fire(Buffs buff)
    {
        GetComponent<Life>().TakeDamage(buff.force, TypeDamage.Fire);
    }

    #endregion Effect
    public void RemoveAllEffect(TypeBuffs typeBuffs)
    {
        foreach(Buffs buff in buffs.ToList())  {
            if (buff.type == typeBuffs) {
                buffs.Remove(buff);
            }
        }
    }

    private void CheckDeleteEffect(TypeBuffs typeBuffs)
    {
        if (typeBuffs == TypeBuffs.Fire) {
            RemoveAllEffect(TypeBuffs.Slow);
        }
        if (typeBuffs == TypeBuffs.Slow) {
            RemoveAllEffect(TypeBuffs.Fire);
        }
    }

    #region AddEffect

    private void AddEffectNormal(TypeBuffs typeBuffs, float time, float force)
    {
        Buffs newBuffs = new Buffs();
        newBuffs.type = typeBuffs;
        newBuffs.force = force;
        newBuffs.timeBetweenTwo = BuffManager.instance.GetCoolDown(typeBuffs);
        newBuffs.nextTimeEffect = Time.time + newBuffs.timeBetweenTwo;
        newBuffs.timeEnd = Time.time + time;
        newBuffs.typeMore = BuffManager.instance.GetAddType(typeBuffs);
        CheckDeleteEffect(typeBuffs);
        buffs.Add(newBuffs);
    }

    private void AddReset(TypeBuffs typeBuffs, float time, float force)
    {
        if (!isBuffActive(typeBuffs)) {
            AddEffectNormal(typeBuffs, time, force);
            return;
        }
        int buffid = GetFirstIDBuffActive(typeBuffs);
        if (buffs[buffid].timeEnd < Time.time + time)
            buffs[buffid].timeEnd = Time.time + time;
        if (buffs[buffid].force < force)
            buffs[buffid].force = force;
    }
    private void AddStack(TypeBuffs typeBuffs, float time, float force)
    {
        if (!isBuffActive(typeBuffs)) {
            AddEffectNormal(typeBuffs, time, force);
            return;
        }
        int buffid = GetFirstIDBuffActive(typeBuffs);
        if (buffs[buffid].timeEnd < Time.time + time)
            buffs[buffid].timeEnd = Time.time + time;
        buffs[buffid].force += force;
    }

    public void AddEffect(TypeBuffs typeBuffs, float time, float force=1f)
    {
        if (imunities && isBuffInList(typeBuffs, imunities.lstImunities))
            return;
        if (GetComponent<Life>().isDead)
            return;
        switch(BuffManager.instance.GetAddType(typeBuffs)) {
            case TypeMore.Add:
                AddEffectNormal(typeBuffs, time, force);
                break;
            case TypeMore.NotAdd:
                break;
            case TypeMore.Reset:
                AddReset(typeBuffs, time, force);
                break;
            case TypeMore.Stack:
                AddStack(typeBuffs, time, force);
                break;
            default:
                break;
        }
    }

    #endregion AddEffect
    public void ResetBuffs()
    {
        buffs = new List<Buffs>();
    }
    void EffectBuff(Buffs buff)
    {
        switch(buff.type) {
            case TypeBuffs.Heal:
                Effect_Heal(buff);
                break;
            case TypeBuffs.Fire:
                Effect_Fire(buff);
                break;
            default:
                break;
        }
        buff.nextTimeEffect = Time.time + buff.timeBetweenTwo;
    }

    private bool isEndEffect(Buffs buff)
    {
        return buff.timeEnd <= Time.time;
    }

    public bool isBuffActive(TypeBuffs buffsCheck)
    {
        foreach(Buffs buff in buffs) {
            if (buff.type == buffsCheck) return true;
        }
        return false;
    }
    public Buffs GetFirstBuffActive(TypeBuffs buffsCheck)
    {
        foreach(Buffs buff in buffs) {
            if (buff.type == buffsCheck) return buff;
        }
        return null;
    }
    public int GetFirstIDBuffActive(TypeBuffs buffsCheck)
    {
        int i = 0;
        foreach(Buffs buff in buffs) {
            if (buff.type == buffsCheck) return i;
            i++;
        }
        return -1;
    }
    private bool isBuffInList(TypeBuffs buffsCheck, TypeBuffs[] lst)
    {
        foreach(TypeBuffs buff in lst) {
            if (buff == buffsCheck) return true;
        }
        return false;
    }

    private void FX_Update()
    {
        if (isBuffActive(TypeBuffs.Fire) && !fireFXactive) {
            FX_Fire_Inst = Instantiate(BuffManager.instance.FX_Fire, transform.position + new Vector3(0, 0.33f, 0), Quaternion.identity, transform);
            fireFXactive = true;
        }
        if (!isBuffActive(TypeBuffs.Fire) && fireFXactive) {
            FX_Fire_Inst.GetComponent<ParticleSystem>().Stop();
            Destroy(FX_Fire_Inst, 1);
            fireFXactive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Buffs buff in buffs) {
            if (buff.timeBetweenTwo != -1 && buff.nextTimeEffect <= Time.time) {
                EffectBuff(buff);
            }
        }
        buffs.RemoveAll(isEndEffect);
        FX_Update();
    }
}
