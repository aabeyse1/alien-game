// Author(s): Lucy Rubin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIcon : MonoBehaviour
{
    [SerializeField] private GameObject enterIcon;
    [SerializeField] private GameObject lockedIcon;
    
    private void Start() {
        // deactivate all icons
        enterIcon.SetActive(false);
        lockedIcon.SetActive(false);
    }
    public void SetState(bool active, bool locked) {
        // deactivate all icons
        enterIcon.SetActive(false);
        lockedIcon.SetActive(false);
       
       if (active)
       {
        if (locked) {
            lockedIcon.SetActive(true);
        } else {
            enterIcon.SetActive(true);
        }
       }
    }
   

}
