using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level texture")] 
    [SerializeField] private Texture2D levelTexture;
    
    [Header("Tiles prefabs")] 
    [SerializeField] private GameObject prefabWallTile;
    [SerializeField] private GameObject prefabRoadTile;

    [Header("Ball and Road paint color")] 
    public Color paintColor;

    [HideInInspector] public List<RoadTile> roadTilesList;
    [HideInInspector] public RoadTile defaultBallRoadPosition;

    private readonly Color colorWall = Color.white;
    private readonly Color colorRoad = Color.black;

    private float unitPerPixel;
    public static int currentLevel;

    private void Awake()
    {
        levelTexture = Resources.Load<Texture2D>($"Levels Textures/level{currentLevel}");
        Generate();
        defaultBallRoadPosition = roadTilesList[0];
    }

    private void Generate()
    {
        unitPerPixel = prefabWallTile.transform.lossyScale.x;
        var halfUnitPerPixel = unitPerPixel / 2f;

        float width = levelTexture.width;
        float height = levelTexture.height;

        var offset = new Vector3(width / 2f, 0f, height / 2f) * unitPerPixel
                     - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                //Get pixel color
                var pixelColor = levelTexture.GetPixel(x, y);

                var spawnPos = new Vector3(x, 0f, y) * unitPerPixel - offset;

                if (pixelColor == colorWall)
                    Spawn(prefabWallTile, spawnPos);
                else if (pixelColor == colorRoad)
                    Spawn(prefabRoadTile, spawnPos);
            }
        }
    }

    private void Spawn(GameObject prefabTile, Vector3 position)
    {
        //fix Y position
        position.y = prefabTile.transform.position.y;

        var obj = Instantiate(prefabTile, position, Quaternion.identity, transform);

        if (prefabTile == prefabRoadTile)
            roadTilesList.Add(obj.GetComponent<RoadTile>());
    }

    public void NextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}