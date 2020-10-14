using UnityEngine;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    public static class FileManagerSystem
    {
        public static void SaveTown(Town town)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string loc = Application.persistentDataPath + "/" + town.ID+ "/townData.soe";
            FileStream stream = new FileStream(loc, FileMode.Create);
            TownData T = new TownData(town);
            stream.Close();
        }

        public static Town LoadTown(string townID)
        {
            string loc = Application.persistentDataPath + "/" + townID + "/townData.soe";
            if (File.Exists(loc))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(loc, FileMode.Open);
                TownData T = bf.Deserialize(stream) as TownData;
                stream.Close();
                return new Town(T);
            }
            Debug.LogError("Error! Save file does not exist");
            return null;
        }

        public static Collection<Citizen> LoadCitizens(Town town)
        {
            string citizenDir = Application.persistentDataPath + "/" + town.ID + "/citizenData/";
            if (Directory.Exists(citizenDir))
            {
                string[] citizenFiles = Directory.GetFiles(citizenDir);
                Collection<Citizen> citizens = new Collection<Citizen>();
                for (int i = 0; i < citizenFiles.Length; i++)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    string filename = citizenDir;
                    FileStream stream = new FileStream(filename, FileMode.Open);
                    CitizenData C = bf.Deserialize(stream) as CitizenData;
                    Citizen citizen = new Citizen(C);
                    stream.Close();
                    citizens.Add(citizen);
                }
                return citizens;
            }
            Debug.LogError("Something wrong happened while trying to load citizen data");
            Application.Quit();
            return null;

        }
        
    }
}