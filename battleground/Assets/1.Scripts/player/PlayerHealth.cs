﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//플레이어의 생명력을 담당
//피격시 피격이펙트를 표시하거나 UI업데이트를 한다.
//죽었을 경우 모든 동작 스크립트 동작을 멈춘다.
public class PlayerHealth : HealthBase
{
    public float health;
    public int criticalHealth = 30;
    public Transform healthHUD;
    public SoundList deathSound;
    public SoundList hitSound;
    public GameObject hurtPrefab; //피격
    public float decayFactor = 0.8f; //감쇠

    private float totalHealth = 100;         
    private Slider healthBar;   
    private Text healthLabel;  
    private bool critical;

    private BlinkHUD criticalHUD;
    private HurtHUD hurtHUD;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        health = totalHealth;
        healthBar = healthHUD.Find("HealthBar/Bar").GetComponent<Slider>();
        healthLabel = healthHUD.Find("HealthBar/Label").GetComponent<Text>();
        healthLabel.text = "" + (int)health;
      
        criticalHUD = healthHUD.Find("Bloodframe").GetComponent<BlinkHUD>();
        hurtHUD = this.gameObject.AddComponent<HurtHUD>();
        hurtHUD.Setup(healthHUD, hurtPrefab, decayFactor, transform);    
    }

    private void Update()
    {
        
    }

    void UpdateHealthBar()
    {
        healthLabel.text = "" + (int)health;
        healthBar.value = health / totalHealth;   
    }

    void Kill()
    {
        isDead = true;
        gameObject.layer = TagAndLayer.GetLayerByName(TagAndLayer.LayerName.Default);
        gameObject.tag = TagAndLayer.TagName.Untagged;
        healthHUD.gameObject.SetActive(false);
        healthHUD.parent.Find("WeaponHUD").gameObject.SetActive(false);
        myAnimator.SetBool(FC.AnimatorKey.Aim, false);
        myAnimator.SetBool(FC.AnimatorKey.Cover, false);
        myAnimator.SetFloat(FC.AnimatorKey.Speed, 0);
        foreach(GenericBehaviour behaviour in GetComponentsInChildren<GenericBehaviour>())
        {
            behaviour.enabled = false;
        }

        SoundManager.Instance.PlayOneShotEffect((int)deathSound, transform.position, 5f);
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart = null, GameObject origin = null)
    {
        health -= (int)damage;

        UpdateHealthBar();

        if (hurtPrefab && healthHUD)
        {
            hurtHUD.DrawHurtUI(origin.transform, origin.GetHashCode());
        }

        if (health <= 0)
        {
            Kill();
        }
        else if (health <= criticalHealth && !critical)
        {
            critical = true;
            criticalHUD.StartBlink();
        }
        SoundManager.Instance.PlayOneShotEffect((int)hitSound, location, 1f);
    }
}
