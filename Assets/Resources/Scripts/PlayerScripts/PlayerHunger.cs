using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    public SCPlayerMovement playerController;

    public Slider HungerBar;

    public int HungerDrain;

    public int StarvingHealthDrain;
    
    private void Start() {
        InvokeRepeating("reduceHunger", 2, 1);
    }

    private void Update() {
        updateHunger();
    }

    public void updateHunger(){
        HungerBar.value = playerController.hunger;
    }

    public void reduceHunger(){
        if(playerController.hunger > 0){
            playerController.hunger -= HungerDrain;
        }else{
            playerController.d.DealDamage(StarvingHealthDrain);
        }
    }
}
