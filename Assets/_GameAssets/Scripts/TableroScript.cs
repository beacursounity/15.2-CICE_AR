using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableroScript : MonoBehaviour {
    Animator anim;
    public GameObject prefabPlayer1;
    public GameObject prefabPlayer2;
    private Transform player1GenPoint;
    private Transform player2GenPoint;
    private GameObject[] posFichas;
    public int[] cells = { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    private const int TURNOS_TOTALES = 9;
    private int numTurnos = 0;
    private bool turnoPlayer1 = true;
    private bool turnoPlayer2 = false;
    private bool player1IsWalking = false;
    private bool player2IsWalking = false;
    private void Start()
    {
        player1GenPoint = GameObject.Find("Player1GenPoint").transform;
        player2GenPoint = GameObject.Find("Player2GenPoint").transform;
        posFichas = GameObject.FindGameObjectsWithTag("Casilla");
    }

    void Update()
    {
        if (numTurnos < TURNOS_TOTALES)
        {
            if (turnoPlayer1)
            {
                if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began)
                {
                    GameObject go = GetTouchedGO();
                    JugarPlayer(go);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    GameObject go = GetClickedGO();
                    JugarPlayer(go);
                }
            }
            else if (turnoPlayer2 && !player2IsWalking)
            {
                player2IsWalking = true;
                Invoke("JugarRival", 1);
            }
        }
    }

    private void JugarPlayer(GameObject go)
    {
        if (go.CompareTag("Casilla") && !player1IsWalking && IsCellEmpty(GetIndexFromCell(go)))
        {
            numTurnos++;
            GameObject newPlayer = Instantiate(prefabPlayer1, player1GenPoint);
            PlayerScript ps = newPlayer.GetComponentInChildren<PlayerScript>();
            ps.MoveToCasilla(go.transform);
            cells[GetIndexFromCell(go)] = 1;
            player1IsWalking = true;
        }
    }

    private void JugarRival()
    {
        numTurnos++;
        int randomPos;
        do
        {
            randomPos = Random.Range(0, 8);
        } while (!IsCellEmpty(randomPos));
        GameObject newEnemy = Instantiate(prefabPlayer2, player2GenPoint);
        PlayerScript ps = newEnemy.GetComponentInChildren<PlayerScript>();
        GameObject randomCell = GameObject.Find("C" + randomPos);
        ps.MoveToCasilla(randomCell.transform);
        cells[randomPos] = 2;
    }

    private int GetIndexFromCell(GameObject go)
    {
        return int.Parse(go.name.Substring(1, 1));
    }

    private GameObject GetTouchedGO()
    {
        GameObject touchedGO = null;
        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            touchedGO = raycastHit.transform.gameObject;
        }
        return touchedGO;
    }

    private GameObject GetClickedGO()
    {
        GameObject touchedGO = null;
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            touchedGO = raycastHit.transform.gameObject;
        }
        return touchedGO;
    }

    private bool IsCellEmpty(int pos)
    {
        bool celdaVacia = true;
        if (cells[pos] != -1)
        {
            celdaVacia = false;
        }
        return celdaVacia;
    }

    public void CambioDeTurno()
    {
        if (!ComprobarTresEnRaya())
        {
            turnoPlayer1 = !turnoPlayer1;
            turnoPlayer2 = !turnoPlayer2;
            player1IsWalking = false;
            player2IsWalking = false;
        } else
        {
            turnoPlayer1 = false;
            turnoPlayer2 = false;
            player1IsWalking = false;
            player2IsWalking = false;
        }
        
    }

    private bool ComprobarTresEnRaya()
    {
        bool hayTresEnRaya = false;
        int ganador = -1;
        if (cells[0] != -1 && cells[0] == cells[1] && cells[0] == cells[2])
        {
            hayTresEnRaya = true;
            ganador = cells[0];
        }
        else if (cells[3] != -1 && cells[3] == cells[4] && cells[3] == cells[5])
        {
            hayTresEnRaya = true;
            ganador = cells[3];
        }
        else if (cells[6] != -1 && cells[6] == cells[7] && cells[6] == cells[8])
        {
            hayTresEnRaya = true;
            ganador = cells[6];
        }
        else if (cells[0] != -1 && cells[0] == cells[3] && cells[0] == cells[6])
        {
            hayTresEnRaya = true;
            ganador = cells[0];
        }
        else if (cells[1] != -1 && cells[1] == cells[4] && cells[1] == cells[7])
        {
            hayTresEnRaya = true;
            ganador = cells[1];
        }
        else if (cells[2] != -1 && cells[2] == cells[5] && cells[2] == cells[8])
        {
            hayTresEnRaya = true;
            ganador = cells[2];
        }
        else if (cells[0] != -1 && cells[0] == cells[4] && cells[0] == cells[8])
        {
            hayTresEnRaya = true;
            ganador = cells[0];
        }
        else if (cells[2] != -1 && cells[2] == cells[4] && cells[2] == cells[6])
        {
            hayTresEnRaya = true;
            ganador = cells[2];
        }
        if (hayTresEnRaya)
        {
            Celebrar(ganador);
        }
        else if (numTurnos > 9)
        {
            print("TABLAS");
        }
        return hayTresEnRaya;
    }

    private void Celebrar(int ganador)
    {
        GameObject[] winners = GameObject.FindGameObjectsWithTag(ganador.ToString());
        foreach (var winner in winners)
        {
            winner.GetComponent<Animator>().SetTrigger("Celebrando");
        }
    }
}
