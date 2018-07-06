using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
    
    //伤害类型
    public enum DamageType
    {
        Physics,
        Magic
    }

    //角色属性
    public int hp;
    public int attack;
    public int magic;
    public int defence;

    //攻击力偏差
    private const int attackOffset = 3;

    //动画管理
    public Animator animator;

    //设定属性数值函数
    public abstract void ResetAttribute();

    //承受伤害函数
    public void ReceiveDamage(int damage)
    {
        damage -= defence;
        damage = damage < 0 ? 0 : damage;
        hp -= damage;
    }

    //造成伤害函数
    public int CauseDamage(DamageType damageType)
    {
        if (damageType == DamageType.Physics)
        {
            return Random.Range(attack - attackOffset, attack + attackOffset);
        }
        else
        {
            return Random.Range(magic - attackOffset, magic + attackOffset);
        }
    }

}