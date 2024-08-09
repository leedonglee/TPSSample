using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLazer : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject Muzzle;

    void Start()
    {
        StartCoroutine(LazerStart());
    }

    IEnumerator LazerStart() 
    {
        while (true)
        {
            Instantiate(Bullet, Muzzle.transform.position, Muzzle.transform.rotation);
            yield return new WaitForSeconds(7f);
        }
    }

}
