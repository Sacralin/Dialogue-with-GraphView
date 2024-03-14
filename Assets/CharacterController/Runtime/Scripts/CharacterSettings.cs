using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSettings : MonoBehaviour
{
    [Header("Perspectives")]
    [Tooltip("Toggles the ability to use the first person perspective")]
    public bool firstPersonPerspective;
    [Tooltip("Toggles the ability to use the third person perspective")]
    public bool thirdPersonPerspective;
    [Tooltip("Toggles the top down perspective. This cannot be used with the first or third person perspectives!")]
    public bool topDownPerspective;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
