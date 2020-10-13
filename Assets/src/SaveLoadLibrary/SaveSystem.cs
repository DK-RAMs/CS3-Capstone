using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    public static class SaveSystem
    {
        public static void SaveTown(Town town)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string loc = Application.persistentDataPath + "/" + town.ID+ "/townData.soe";
            FileStream stream = new FileStream(loc, FileMode.Create);
        }

        public static TownData LoadTown(string townID)
        {
            string loc = Application.persistentDataPath + "/" + townID + "/townData.soe";
            if (File.Exists(loc))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(loc, FileMode.Open);
                return bf.Deserialize(stream) as TownData;
            }
            Debug.LogError("Error! Save file does not exist");
            return null;
        }
    }
}