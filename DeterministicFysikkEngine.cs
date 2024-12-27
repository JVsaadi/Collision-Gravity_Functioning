using UnityEngine;
using System.Collections.Generic;

public class DeterministicPhysicsEngine : MonoBehaviour
{
    // Tidsskrittet for fysikken (16 = 1000/60)
    private int tidsskritt = 16;

    // Den nåværende tiden
    private int tid = 0;

    // Liste over objekter som skal kjøres fysikken på
    private List<GameObject> objekter = new List<GameObject>();

    // Tidsskritt-teller
    private int tidsskrittTeller = 0;

    // Struktur for å representere posisjon og hastighet med integer-verdier
    private struct Position
    {
        public int x;
        public int y;
    }

    void Start()
    {
        GameObject mittObjekt = GameObject.Find("Slime");
        LeggTilObjekt(mittObjekt);
    }

    void Update()
    {
        // Kjør fysikken med fast tidsskritt
        tidsskrittTeller++;
        if (tidsskrittTeller >= tidsskritt)
        {
            // Kjør fysikken på alle objekter
            foreach (GameObject obj in objekter)
            {
                KjørFysikk(obj);
            }
            tidsskrittTeller = 0;
        }
    }

    // Kjør fysikken på et objekt
    private void KjørFysikk(GameObject obj)
    {
        // Hent objektets posisjon og hastighet
        Position posisjon = new Position();
        posisjon.x = (int)obj.transform.position.x;
        posisjon.y = (int)obj.transform.position.y;

        // Hent objektets akselerasjon
        int akselerasjonX = 0;
        int akselerasjonY = -1; // Tyngdekraft

        // Check if the object is colliding with another object
        DeterministicCollider collider = obj.GetComponent<DeterministicCollider>();
        if (collider != null && collider.IsColliding(collider.other))
        {
            // Set the vertical speed to 0
            akselerasjonY = 0;
        }

        // Oppdater hastighet og posisjon
        posisjon.x += akselerasjonX;
        posisjon.y += akselerasjonY;

        // Oppdater objektets posisjon
        obj.transform.position = new Vector3(posisjon.x, posisjon.y, 0);
    }

    // Legg til et objekt til fysikken
    public void LeggTilObjekt(GameObject obj)
    {
        objekter.Add(obj);
    }
}