using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    Animator anim;
    Transform destino;
    TableroScript ts;
    enum Estado { Reposo, Andando, Celebrando };
    Estado estado = Estado.Reposo;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        ts = GameObject.Find("TargetTablero").GetComponent<TableroScript>();
    }
    void Update()
    {
        if (estado==Estado.Andando && (Mathf.Abs(Vector3.Distance(transform.position, destino.position)) <= 0.01)){
            estado = Estado.Reposo;
            anim.SetBool("Andando", false);
            ts.CambioDeTurno();
        } else if (estado == Estado.Andando)
        {
            transform.LookAt(destino.position);
        }
    }

    public void MoveToCasilla(Transform _destino) {
        destino = _destino;
        estado = Estado.Andando;
        transform.LookAt(destino.position);
        anim.SetBool("Andando", true);
    }
    
}
