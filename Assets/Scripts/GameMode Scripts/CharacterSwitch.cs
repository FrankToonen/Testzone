using UnityEngine;
using System.Collections;

public class CharacterSwitch : MonoBehaviour {
    
  private GameObject CurrentCarrier;
    public Camera MainCamera;

  void OnTriggerStay(Collider other) {
      if (other.gameObject.tag == "Player")
      {
            if (other.gameObject.GetComponent<TP_Controller>().PlayerAction == true) {
                CurrentCarrier = other.gameObject;
            }
        }
    }

  void Update()
  {
      if (CurrentCarrier == null)
      {
          this.gameObject.transform.position = this.gameObject.transform.position;

        }
        else
      {
          this.gameObject.transform.position = CurrentCarrier.gameObject.GetComponent<CharacterStats>().flagPouch.transform.position;
        }
    }
  
}
