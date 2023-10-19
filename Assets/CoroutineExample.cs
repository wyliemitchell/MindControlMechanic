using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    float time = 0;
    bool breakChannel = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountingUpTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StopChanneling();
        }
    }

    IEnumerator CountingUpTime()
    {
        Debug.Log("Frame 1");
        yield return null; //Ends code here, will continue next frame.
        yield return new WaitForSeconds(2.5f); // Waits for set time before continuing the rest of the coroutine.
        Debug.Log("Waited 2.5s from WaitForSeconds");

        time = 0;

        while(time < 2.5f)
        {
            yield return null;
            time += Time.deltaTime;
            Debug.Log(time);
            if(breakChannel)
            {
                Debug.Log("BROKE CHANNEL");
                break;
            }
        }

        if(!breakChannel)
        {
            Debug.Log("Timer Finished!");
        }
        else
        {
            breakChannel = false;
        }
    }

    public void StopChanneling()
    {
        breakChannel = true;
    }

    float ReturnValue()
    {
        return 1f;
    }
}
