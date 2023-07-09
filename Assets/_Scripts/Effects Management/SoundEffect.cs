using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroyEffect");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
