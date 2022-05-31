using UnityEngine;

public class Playfield : MonoBehaviour
{
    // Bredd p� spelplan
    public static int bredd = 10;

    // H�jd p� spelplan
    public static int h�jd = 20;

    // 2D array som h�ller koll p� vad som �r fast p� spelplan
    public static Transform[,] grid = new Transform[bredd, h�jd];

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Kollar om den aktiva gruppen �r p� spelplanet. Beh�ver ha den i Playfield klassen s� vi 
    // kan anv�nda bredden p� spelplan variabel
    public static bool VectorInomSpelplan(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < bredd &&
                (int)pos.y >= 0);
    }

    // G�r genom hela gridden, letar efter fyllda rader och raderar dem. Flyttar �ven ner
    // raderna efter helt ifyllda rader �r borttagen fr�n gridden.
    public static void RaderaFylldaRader()
    {
        for (int y = 0; y < h�jd; ++y)
        {
            if (�rRadenFylld(y))
            {
                TabortRad(y);
                FlyttaNerAllaRader(y + 1);

                // Tar n�sta rad ovanf�r den vi har hanterat sist
                --y;
            }
        }
    }

    // Efter vi ta bort en rad m�ste vi flytta ner alla rader ovan ner en rad. Skickar in
    // vilken rad som var borttagen.
    private static void FlyttaNerAllaRader(int y)
    {
        // F�r varje rad i gridden
        for (int i = y; i < h�jd; ++i)
        {
            FlyttaNerRaden(i);
        }            
    }

    //Flyttar ner alla punkter i en rad om punkten �r inte null.
    private static void FlyttaNerRaden(int y)
    {
        // F�r varje punkt i raden
        for (int x = 0; x < bredd; ++x)
        {
            // Om punkten �r inte null
            if (grid[x, y] != null)
            {
                // Flytta ner punkten i gridden
                grid[x, y - 1] = grid[x, y];

                // S�tt gamla punkten till ej ifylld
                grid[x, y] = null;

                // Uppdaterar vart grafikvekor finns. Anv�nds f�r rendering.
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // Kollar om en rad �r helt fylld
    private static bool �rRadenFylld(int y)
    {
        for (int x = 0; x < bredd; ++x)
        {
            // Om raden inte �r ifylld �r inte hela raden fylld. Returnerar false
            if (grid[x, y] == null)
                return false;
        }

        // Alla punkter i raden �r ifylld s� hela raden �r ifylld
        return true;
    }

    // Raderar en hel rad av objekt p� spelplan. Detta anv�nds ovan efter man har kollat om 
    // alla punkter �r ifylld.
    private static void TabortRad(int y)
    {
        for (int x = 0; x < bredd; ++x)
        {
            // Raderar grafiken p� scenen f�r punkten
            Destroy(grid[x, y].gameObject);

            // S�tter gridden till null eftersom det inte finns n�gon punkt h�r l�ngre
            grid[x, y] = null;
        }
    }
}
