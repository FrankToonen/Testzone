using UnityEngine;
using System.Collections;

public class CharacterSwitch : MonoBehaviour {
  
  private GameObject CurrentCarrier;
    public Camera MainCamera;

//check if any Player is touching flag
  void OnTriggerStay(Collider other) {
      if (other.gameObject.tag == "Player")
      {
            //check if player is grabbing flag
            if (other.gameObject.GetComponent<TP_Controller>().PlayerAction == true && other.gameObject.GetComponent<CharacterStats>().hasFlag == false)
            {
                CurrentCarrier = other.gameObject;
                CurrentCarrier.GetComponent<CharacterStats>().hasFlag = true;
            }
            
        }
    }

    //When player stops touching flag
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CharacterStats>().hasFlag == true)
            {
                MainCamera.GetComponent<TextMesh>().text = "";
                other.gameObject.GetComponent<CharacterStats>().hasFlag = false;
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
