using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

public class BinaryDataStream : MonoBehaviour
{
    public static void Save<T>(T serializedObject, string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);


        var filePath = Path.Combine(path, fileName + ".dat");
        var formatter = new BinaryFormatter();

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(fileStream, serializedObject);
            }
        }
        catch (SerializationException e)
        {
            Debug.LogError("Save File Error : " + e.Message);
        }
        catch (IOException e)
        {
            Debug.LogError("I/O error while saving file: " + e.Message);
        }


    }

    public static bool Exist(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        string fullFileName = fileName + ".dat";
        return File.Exists(Path.Combine(path, fullFileName));
    }

    public static T Load<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        var filePath = Path.Combine(path, fileName + ".dat");

        if (!File.Exists(filePath))
            return default;

        var formatter = new BinaryFormatter();

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var obj = formatter.Deserialize(fileStream);
                if (obj is T t)
                    return t;
                return (T)obj;
            }
        }
        catch (SerializationException e)
        {
            Debug.LogError("Load File Error : " + e.Message);
        }
        catch (IOException e)
        {
            Debug.LogError("I/O error while loading file: " + e.Message);
        }

        return default;
    }




}
