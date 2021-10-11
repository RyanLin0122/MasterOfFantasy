using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLifeCycle : MonoBehaviour
{
    private void Awake()
    {
        print("Awake");
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        print("OnEnable");
    }
    void Start()
    {
        print("Start");
    }
    private void OnDisable()
    {
        print("OnDisable");
    }

}
