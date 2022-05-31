using UnityEngine;

public class Group : MonoBehaviour
{
    // Tid sen vi k�rde Update sist
    float senasteUpdate = 0;

    void Start()
    {
        // Om startposition �r inte giltig �r det slut! 
        if (!�rGiltigGridPosition())
        {
            Debug.Log("Du har f�rlorat!!!!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Kolla om vi har tryckt p� v�nsterpil knappen
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Flytta gruppen en plats till v�nster
            transform.position += new Vector3(-1, 0, 0);

            // Kolla om flyttet �r giltig
            if (�rGiltigGridPosition())
            {
                // Flyttet �r giltig uppdatera gridden
                UppdateraGridden();
            }
            else
            {
                // Ogiltig flytt! �terst�ll till f�rrag�ende punkt.
                transform.position += new Vector3(1, 0, 0);
            }
        }
        // Kolla om vi har tryckt p� h�gerpil knappen
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Flytta gruppen en plats till h�ger
            transform.position += new Vector3(1, 0, 0);

            // Kolla om flyttet �r giltig
            if (�rGiltigGridPosition())
            {
                // Flyttet �r giltig uppdatera gridden
                UppdateraGridden();
            }
            else
            {
                // Ogiltig flytt! �terst�ll till f�rrag�ende punkt.
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        // Kolla om vi har tryckt p� upp�tpil knappen
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Rotera gruppen 90 grader kring 0,0 punkt i gruppen
            transform.Rotate(0, 0, -90);

            // Kolla om rotationen �r giltig
            if (�rGiltigGridPosition())
            {
                // Rotationen �r giltig. Uppdatera gridden
                UppdateraGridden();
            }   
            else
            {
                // Rotationen ej giltig! �terst�ll till innan rotationen!
                transform.Rotate(0, 0, 90);
            }
        }
        // Kolla om vi har tryckt p� ned�tpil knappen eller om lite tid har g�tt sen vi k�rde senaset
        // Update(). Andra kontrollen �r f�r att se om vi ska l�sa fast gruppen p� spelplan. 
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - senasteUpdate >= 0.5)
        {
            // Flytta gruppen ner en plats
            transform.position += new Vector3(0, -1, 0);

            // Kolla om flyttet �r giltig
            if (�rGiltigGridPosition())
            {
                // Flyttet �r giltig, uppdatera gridden
                UppdateraGridden();
            }
            // Flyttet �r inte giltig. Betyder att vi �r p� botten av spelplan. Dags att k�ra
            // lite extra kontroller.
            else
            {
                // Flyttet �r inte giltig
                transform.position += new Vector3(0, 1, 0);

                // Rensa alla rader som �r helt fyllda
                Playfield.RaderaFylldaRader();

                // Skapa upp n�sta grupp p� spelplan
                FindObjectOfType<Spawner>().SpawnNext();

                // Inaktivera skript p� denna grupp. Gruppen kommer sitta fast p� spelplan
                // och inte r�ra p� sig l�ngre.
                enabled = false;
            }

            // S�tt en ny tid p� senaste Update() anrop
            senasteUpdate = Time.time;
        }
    }

    // Kontrollera om vi har en giltig grid position
    private bool �rGiltigGridPosition()
    {
        // F�r varje punkt inom gruppen
        foreach (Transform child in transform)
        {
            // Avrunda s� vi f�r heltal s� array indexarna fungerar r�tt
            Vector2 v = AvrundaVector2(child.position);

            // Returnerar false om vi inte �r inom spelplans gr�nser
            if (!Playfield.VectorInomSpelplan(v))
                return false;

            // Returnerar false om gridden redan inneh�ller en punkt fr�n en annan grupp
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return false;
            }                
        }

        // Grid position �r giltig
        return true;
    }

    // Uppdatera gridden med nya positionen av gruppen
    void UppdateraGridden()
    {
        // Ta bort den gamla positionen av gruppen fr�n gridden
        // F�r varje rad
        for (int y = 0; y < Playfield.h�jd; ++y)
        {
            // F�r varje punkt i raden
            for (int x = 0; x < Playfield.bredd; ++x)
            {
                // Om punkten �r inte null. En null punkt beh�ver inte rensas
                if (Playfield.grid[x, y] != null)
                {
                    // Om punkten i gridden �r en punkt fr�n denna grupp
                    if (Playfield.grid[x, y].parent == transform)
                    {
                        // S�tt punkten i gridden till null
                        Playfield.grid[x, y] = null;
                    }                        
                }
            }
        }

        // L�gg till alla punkter i gridden f�r denna grupp
        foreach (Transform punkt in transform)
        {
            // Avrunda s� vi f�r heltal s� array indexarna fungerar r�tt
            Vector2 v = AvrundaVector2(punkt.position);

            //L�gg till punkten i gridden
            Playfield.grid[(int)v.x, (int)v.y] = punkt;
        }
    }

    // Avrundar en Vector2 till n�rmaste integer v�rde s� vi kan r�kna enligt gridden f�r spelplan
    public Vector2 AvrundaVector2(Vector2 vector2)
    {
        return new Vector2(Mathf.Round(vector2.x), Mathf.Round(vector2.y));
    }
}
