using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFX : MonoBehaviour
{
    private GameObject fxObject;

    // Start is called before the first frame update
    void Start()
    {
        fxObject = this.gameObject;
    }

    public void DestroyThisFX()
    {
        Destroy(fxObject);
    }
}
