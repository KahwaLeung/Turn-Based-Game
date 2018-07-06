﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

    //重写设定属性数值
    public override void ResetAttribute()
    {
        hp = 50;
        attack = 8;
        magic = 17;
        defence = 2;
    }

    private void Awake()
    {
        ResetAttribute();
        animator = this.GetComponent<Animator>();
    }

}