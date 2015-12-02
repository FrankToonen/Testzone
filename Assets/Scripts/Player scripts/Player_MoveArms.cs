using UnityEngine;
using System.Collections;

public class Player_MoveArms : MonoBehaviour
{
    Vector3 lookAt = Vector3.zero;
    Transform arms, cam;

    void FindArms()
    {

        Transform bot = transform.FindChild("Bot");
        if (bot != null)
        {
            arms = bot.FindChild("Arms");
            int count = transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject.GetComponent<ParticleSystem>() != null || child.name == "IndicatorCone(Clone)")
                {
                    child.transform.parent = arms;
                }
            }
        }
        cam = transform.FindChild("Camera");
    }

    void Update()
    {
        if (arms == null)
        {
            FindArms();
        } else
        {
            lookAt = cam.position + new Vector3(0, -3, 0);
            arms.LookAt(lookAt);
        }
    }
}
