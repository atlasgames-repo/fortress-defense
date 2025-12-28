using UnityEngine;

public class test : MonoBehaviour
{
    //int[] number = new int[5];
    public tutorials tutorials;
    int j;

    void Start()
    {
        /*number[0] = 1;
        number[1] = 1000;
        number[2] = 3;*/
    }

    void Update()
    {
        /*int i = System.Array.IndexOf(number, 1000);
        Debug.Log(i);*/
        for(int i = 0; i < 50; i++)
        {
            j = tutorials.infoT[i].LevelNumber;

            if(j == GlobalValue.levelPlaying)
            {
                //Debug.Log("we did it");
            }
        }

        //num1[k] = tutorials.infoT[k].LevelNumber;
    }
}
