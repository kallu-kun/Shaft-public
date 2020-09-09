using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveController : MonoBehaviour
{

    private BinaryFormatter binaryFormatter;
    private FileStream saveFile;

    string dataPath;

    [SerializeField]
    private ScoreList scoreList;

    private void Awake()
    {
        binaryFormatter = new BinaryFormatter();
        dataPath = Application.dataPath + "/scores.save";

        LoadData();
    }

    private void InitialiseGameData()
    {
        scoreList.scoreList = new List<Score>();
    }

    public void SaveData()
    {
        saveFile = File.Create(dataPath);
        binaryFormatter.Serialize(saveFile, scoreList.scoreList);
        saveFile.Close();
    }

    public void LoadData()
    {
        if (File.Exists(dataPath))
        {
            saveFile = File.Open(dataPath, FileMode.Open);
            scoreList.scoreList = (List<Score>)binaryFormatter.Deserialize(saveFile);
            saveFile.Close();
        }
        else
        {
            InitialiseGameData();
            SaveData();
        }
    }
}