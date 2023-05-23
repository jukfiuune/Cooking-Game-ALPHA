using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public Slider slider;
    public float MaxHealth = 162;
    public float CurrentHealth = 162;

    void Update ()
    {
        UpdateHealthbar();
        //DealDamage(1);
    }
    public void DealDamage(float damage)
    {
        CurrentHealth = CurrentHealth - damage;
        UpdateHealthbar();

    }    

    public void UpdateHealthbar()
    {
        if(CurrentHealth != null)
        {
            slider.value = CurrentHealth;
            slider.maxValue = MaxHealth;
        }
    }    
}
