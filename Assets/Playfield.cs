using UnityEngine;

public class Playfield : MonoBehaviour
{
    // Bredd på spelplan
    public static int bredd = 10;

    // Höjd på spelplan
    public static int höjd = 20;

    // 2D array som håller koll på vad som är fast på spelplan
    public static Transform[,] grid = new Transform[bredd, höjd];

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Kollar om den aktiva gruppen är på spelplanet. Behöver ha den i Playfield klassen så vi 
    // kan använda bredden på spelplan variabel
    public static bool VectorInomSpelplan(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < bredd &&
                (int)pos.y >= 0);
    }

    // Går genom hela gridden, letar efter fyllda rader och raderar dem. Flyttar även ner
    // raderna efter helt ifyllda rader är borttagen från gridden.
    public static void RaderaFylldaRader()
    {
        for (int y = 0; y < höjd; ++y)
        {
            if (ÄrRadenFylld(y))
            {
                TabortRad(y);
                FlyttaNerAllaRader(y + 1);

                // Tar nästa rad ovanför den vi har hanterat sist
                --y;
            }
        }
    }

    // Efter vi ta bort en rad måste vi flytta ner alla rader ovan ner en rad. Skickar in
    // vilken rad som var borttagen.
    private static void FlyttaNerAllaRader(int y)
    {
        // För varje rad i gridden
        for (int i = y; i < höjd; ++i)
        {
            FlyttaNerRaden(i);
        }            
    }

    //Flyttar ner alla punkter i en rad om punkten är inte null.
    private static void FlyttaNerRaden(int y)
    {
        // För varje punkt i raden
        for (int x = 0; x < bredd; ++x)
        {
            // Om punkten är inte null
            if (grid[x, y] != null)
            {
                // Flytta ner punkten i gridden
                grid[x, y - 1] = grid[x, y];

                // Sätt gamla punkten till ej ifylld
                grid[x, y] = null;

                // Uppdaterar vart grafikvekor finns. Används för rendering.
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // Kollar om en rad är helt fylld
    private static bool ÄrRadenFylld(int y)
    {
        for (int x = 0; x < bredd; ++x)
        {
            // Om raden inte är ifylld är inte hela raden fylld. Returnerar false
            if (grid[x, y] == null)
                return false;
        }

        // Alla punkter i raden är ifylld så hela raden är ifylld
        return true;
    }

    // Raderar en hel rad av objekt på spelplan. Detta används ovan efter man har kollat om 
    // alla punkter är ifylld.
    private static void TabortRad(int y)
    {
        for (int x = 0; x < bredd; ++x)
        {
            // Raderar grafiken på scenen för punkten
            Destroy(grid[x, y].gameObject);

            // Sätter gridden till null eftersom det inte finns någon punkt här längre
            grid[x, y] = null;
        }
    }
}
