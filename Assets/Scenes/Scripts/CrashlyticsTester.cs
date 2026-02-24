using System;
using UnityEngine;

public class CrashlyticsTester : MonoBehaviour 
{
    int updatesBeforeException;

    void Start() 
    {
        updatesBeforeException = 0;
    }

    void Update()
    {
        throwExceptionEvery60Updates();
    }

    void throwExceptionEvery60Updates()
    {
        if (updatesBeforeException > 0)
        {
            updatesBeforeException--;
        }
        else
        {
            updatesBeforeException = 60;
            throw new System.Exception("Тестовый краш для проверки Firebase");
        }
    }
}