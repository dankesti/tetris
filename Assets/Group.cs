using UnityEngine;

public class Group : MonoBehaviour
{
    // Tid sen vi körde Update sist
    float senasteUpdate = 0;

    void Start()
    {
        // Om startposition är inte giltig är det slut! 
        if (!ÄrGiltigGridPosition())
        {
            Debug.Log("Du har förlorat!!!!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Kolla om vi har tryckt på vänsterpil knappen
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Flytta gruppen en plats till vänster
            transform.position += new Vector3(-1, 0, 0);

            // Kolla om flyttet är giltig
            if (ÄrGiltigGridPosition())
            {
                // Flyttet är giltig uppdatera gridden
                UppdateraGridden();
            }
            else
            {
                // Ogiltig flytt! Återställ till förragående punkt.
                transform.position += new Vector3(1, 0, 0);
            }
        }
        // Kolla om vi har tryckt på högerpil knappen
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Flytta gruppen en plats till höger
            transform.position += new Vector3(1, 0, 0);

            // Kolla om flyttet är giltig
            if (ÄrGiltigGridPosition())
            {
                // Flyttet är giltig uppdatera gridden
                UppdateraGridden();
            }
            else
            {
                // Ogiltig flytt! Återställ till förragående punkt.
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        // Kolla om vi har tryckt på uppåtpil knappen
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Rotera gruppen 90 grader kring 0,0 punkt i gruppen
            transform.Rotate(0, 0, -90);

            // Kolla om rotationen är giltig
            if (ÄrGiltigGridPosition())
            {
                // Rotationen är giltig. Uppdatera gridden
                UppdateraGridden();
            }   
            else
            {
                // Rotationen ej giltig! Återställ till innan rotationen!
                transform.Rotate(0, 0, 90);
            }
        }
        // Kolla om vi har tryckt på nedåtpil knappen eller om lite tid har gått sen vi körde senaset
        // Update(). Andra kontrollen är för att se om vi ska låsa fast gruppen på spelplan. 
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - senasteUpdate >= 0.5)
        {
            // Flytta gruppen ner en plats
            transform.position += new Vector3(0, -1, 0);

            // Kolla om flyttet är giltig
            if (ÄrGiltigGridPosition())
            {
                // Flyttet är giltig, uppdatera gridden
                UppdateraGridden();
            }
            // Flyttet är inte giltig. Betyder att vi är på botten av spelplan. Dags att köra
            // lite extra kontroller.
            else
            {
                // Flyttet är inte giltig
                transform.position += new Vector3(0, 1, 0);

                // Rensa alla rader som är helt fyllda
                Playfield.RaderaFylldaRader();

                // Skapa upp nästa grupp på spelplan
                FindObjectOfType<Spawner>().SpawnNext();

                // Inaktivera skript på denna grupp. Gruppen kommer sitta fast på spelplan
                // och inte röra på sig längre.
                enabled = false;
            }

            // Sätt en ny tid på senaste Update() anrop
            senasteUpdate = Time.time;
        }
    }

    // Kontrollera om vi har en giltig grid position
    private bool ÄrGiltigGridPosition()
    {
        // För varje punkt inom gruppen
        foreach (Transform child in transform)
        {
            // Avrunda så vi får heltal så array indexarna fungerar rätt
            Vector2 v = AvrundaVector2(child.position);

            // Returnerar false om vi inte är inom spelplans gränser
            if (!Playfield.VectorInomSpelplan(v))
                return false;

            // Returnerar false om gridden redan innehåller en punkt från en annan grupp
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return false;
            }                
        }

        // Grid position är giltig
        return true;
    }

    // Uppdatera gridden med nya positionen av gruppen
    void UppdateraGridden()
    {
        // Ta bort den gamla positionen av gruppen från gridden
        // För varje rad
        for (int y = 0; y < Playfield.höjd; ++y)
        {
            // För varje punkt i raden
            for (int x = 0; x < Playfield.bredd; ++x)
            {
                // Om punkten är inte null. En null punkt behöver inte rensas
                if (Playfield.grid[x, y] != null)
                {
                    // Om punkten i gridden är en punkt från denna grupp
                    if (Playfield.grid[x, y].parent == transform)
                    {
                        // Sätt punkten i gridden till null
                        Playfield.grid[x, y] = null;
                    }                        
                }
            }
        }

        // Lägg till alla punkter i gridden för denna grupp
        foreach (Transform punkt in transform)
        {
            // Avrunda så vi får heltal så array indexarna fungerar rätt
            Vector2 v = AvrundaVector2(punkt.position);

            //Lägg till punkten i gridden
            Playfield.grid[(int)v.x, (int)v.y] = punkt;
        }
    }

    // Avrundar en Vector2 till närmaste integer värde så vi kan räkna enligt gridden för spelplan
    public Vector2 AvrundaVector2(Vector2 vector2)
    {
        return new Vector2(Mathf.Round(vector2.x), Mathf.Round(vector2.y));
    }
}
