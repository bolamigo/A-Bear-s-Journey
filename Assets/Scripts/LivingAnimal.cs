using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LivingAnimal : MonoBehaviour
{
    public float health = 10; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(health<0){
            Destroy(gameObject);
        }
    }
    public void Damage(GameObject enemy,float damage){
        GetComponent<NavMeshAgent>().velocity = Vector3.Normalize(transform.position-enemy.transform.position)*5;
        health-=damage;
    }
}
