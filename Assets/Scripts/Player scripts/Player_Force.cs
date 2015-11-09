using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_Force : NetworkBehaviour
{
    Vector3 impact = Vector3.zero;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (impact.magnitude > 0.2F)
            character.Move(impact * Time.deltaTime);

        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    public void AddImpact(Vector3 dir, float force)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        dir.Normalize();
        if (dir.y < 0)
            dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force;
        GetComponent<TP_Motor>().gravity = 0;
    }
}
