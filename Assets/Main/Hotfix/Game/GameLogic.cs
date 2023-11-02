using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Lockstep;

public class GameLogic : MonoBehaviour
{
    // Start is called before the first frame update

    //Âß¼­Ö¡
    public float elapseSeconds;
    //äÖÈ¾Ö¡
    public float realElapseSeconds;

    bool Quited = false;

    void Awake()
    {
        
    }

    void Start()
    {
        Log.Debug("Game Logic is Start!!!");
    }

    void FixedUpdate()
    {
        LockstepManager.Simulate();
    }

    // Update is called once per frame
    void Update()
    {
        LockstepManager.Visualize();
    }

    void LateUpdate()
    {
        LockstepManager.LateVisualize();
    }

    void OnDisable()
    {
        if (Quited)
            return;
        LockstepManager.Deactivate();
    }

    void OnApplicationQuit()
    {
        Quited = true;
        LockstepManager.Quit();
    }
}
