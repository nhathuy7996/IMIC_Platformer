using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] EasyTween StartButtn;
    // Start is called before the first frame update
    void Start()
    {
        StartButtn.OpenCloseObjectAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
