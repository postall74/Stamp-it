using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerate : MonoBehaviour
{
    public Texture2D[] map;
    
    public ColorToPrefab[] ColorMapping;
    public Transform Player;
    GameObject Floor;
    public GameObject FloorCube;
    public Color FloorColor;
    int Lvl;

    public Text LevelText;
    // Start is called before the first frame update
    void Start()
    {
        Lvl = PlayerPrefs.GetInt("Level", 0);
        LevelText.text = "LEVEL : " + (Lvl);
        GenerateLevel();
    }

    public void GenerateLevel() {

        CreateFloor();
        for (int x = 0; x < map[Lvl].width; x++) {
            for (int y = 0; y < map[Lvl].height; y++) {
                Vector3 pos = new Vector3(x, -0.5f, y);
                GameObject BaseTile = Instantiate(FloorCube, pos, transform.rotation);
         //       BaseTile.transform.SetParent(Floor.transform);
                GenerateTiles(x,y);
               // Debug.Log(" " + map[Lvl]);
            }
        }
    }

    public void GenerateTiles(int x, int y) {
        Color PixelColor = map[Lvl].GetPixel(x, y);
        if (PixelColor.a == 0) {
            
            return;
        }
        foreach (ColorToPrefab colormappings in ColorMapping) {
          
            if (colormappings.color.Equals(PixelColor)) {
                
                Vector3 pos = new Vector3(x, 0, y);

                if (colormappings.prefab.name.Equals("Enemy"))
                {

                    Player.position = new Vector3(x, 0.5f, y);
                }
                else
                {
                    Instantiate(colormappings.prefab, pos, transform.rotation);
                }
            }

        }
    }

    public void CreateFloor() {
        Floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Floor.transform.position = new Vector3((map[Lvl].width/ 2f)-0.5f, -0.9f, (map[Lvl].height/2f)-0.5f);
        Floor.transform.localScale = new Vector3(map[Lvl].width + 0.5f, 0.5f, map[Lvl].height + 0.5f);
        Floor.transform.GetComponent<MeshRenderer>().material.SetColor("_Color", FloorColor);
    }

    public Vector2 MoveArea() {
        Vector2 dimen = new Vector2(map[Lvl].width, map[Lvl].height);
        return dimen;
    }
}
