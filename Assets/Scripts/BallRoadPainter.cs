using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class BallRoadPainter : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BallMovement ballMovement;
    [SerializeField] private MeshRenderer ballMeshRenderer;

    [SerializeField] private TextMeshProUGUI levelStatus;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button refreshLevel;
    [SerializeField] private Button menu;

    public int paintedRoadTiles;

    private void Start()
    {
        levelStatus.SetText($"LEVEL {LevelManager.currentLevel}");
        
        //paint ball
        ballMeshRenderer.material.color = levelManager.paintColor;
        
        //paint default ball tile
        Paint(levelManager.defaultBallRoadPosition, .5f, 0f);
        
        //paint ball road
        ballMovement.onMoveStart += OnBallMoveStartHandler; 
    }

    private void OnBallMoveStartHandler(List<RoadTile> roadTiles, float totalDuration)
    {
        var stepDuration = totalDuration / roadTiles.Count;
        for (var i = 0; i < roadTiles.Count; i++)
        {
            var roadTile = roadTiles[i];
            if (!roadTile.isPainted)
            {
                var duration = totalDuration / 2f;
                var delay = i * (stepDuration / 2f);
                Paint(roadTile, duration, delay);
            }
            
            //Check if level completed
            if (paintedRoadTiles != levelManager.roadTilesList.Count) continue;
            levelStatus.SetText("LEVEL COMPLETE");
            refreshLevel.gameObject.SetActive(false);
            menu.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
        }
    }
    private void Paint(RoadTile roadTile, float duration, float delay)
    {
        roadTile.meshRenderer.material.DOColor(levelManager.paintColor, duration).SetDelay(delay);
        roadTile.isPainted = true;
        paintedRoadTiles++;
    }
}
