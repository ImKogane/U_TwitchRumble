using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveDatas(object item, string dest)
    {
        string path = Path.Combine(Application.persistentDataPath, dest + ".xml");
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
        Debug.Log("New save at :" + Application.persistentDataPath + "/" + dest + ".xml");
    }

    public static T LoadDatas<T>(string dest)
    {
        string path = Path.Combine(Application.persistentDataPath, dest);
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(path);
        T deserialized = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
}