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
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled =false;
        GetComponent<Rigidbody>().velocity = Vector3.Normalize(transform.position-enemy.transform.position)*10;
        health-=damage;
        IEnumerator Waker()
        {
            yield return new WaitForSeconds(1f);// Wait for one second

            navMeshAgent.enabled = true;
        }
        StartCoroutine(Waker());
    }
}
