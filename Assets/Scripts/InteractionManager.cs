using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Playables;


public class InteractionManager : MonoBehaviour
{


    public int index;
    

   
    [SerializeField] private ChallengeManager challengManager;

    public void OnMouseDown()
    {
        challengManager.OptionSelect(index);

    }

   







    
}
