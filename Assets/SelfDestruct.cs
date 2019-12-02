using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private float timer = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0) {
            Destroy(this.gameObject);
        }
        timer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Boooooom");

    }
}
