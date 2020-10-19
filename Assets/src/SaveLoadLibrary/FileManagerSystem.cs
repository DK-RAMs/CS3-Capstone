<<<<<<< Updated upstream
using System.Collections.Generic;
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
                string[] citizenFolders = Directory.GetFileSystemEntries(citizenDir); // 
                Collection<Citizen> citizens = new Collection<Citizen>();
                for (int i = 0; i < citizenFolders.Length; i++)
                {
                    string citizenFolder = citizenFolders[i];
                    if (Directory.Exists(citizenFolders[i]))
                    {
                        
                    }
                    BinaryFormatter bf = new BinaryFormatter();
                    string filename = citizenFolders[i];
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

        public static void SaveCitizens(Town town, Collection<Citizen> citizens)
        {
            
        }

        public static HashSet<Building> LoadBuildings(Town town)
        {
            string buildingDir = Application.persistentDataPath + "/" + town.ID + "/buildingData/";
            if (Directory.Exists(buildingDir))
            {
                string[] buildingFiles = Directory.GetFiles("buildingDir");
                HashSet<Building> buildings = new HashSet<Building>();
                for (int i = 0; i < buildingFiles.Length; i++)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    string fileName = buildingFiles[i];
                    FileStream stream = new FileStream(fileName, FileMode.Open);
                    BuildingData b = bf.Deserialize(stream) as BuildingData;
                    Building building = new Building(b);
                    stream.Close();
                    
                }

                return buildings;
            }

            return null;
        }
        
    }
=======
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    public static class FileManagerSystem // Still need to finish
    {
        public static void SaveTown(Town town)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                string loc = Application.persistentDataPath + "/" + town.ID + "/townData.soe";
                FileStream stream = new FileStream(loc, FileMode.Create);
                TownData T = new TownData(town);
                formatter.Serialize(stream, T);
                stream.Close();
            }
            catch (Exception e)
            {
                Debug.Log("An error occurred while trying to serialize an object");
            }
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
                string[] citizenFiles = Directory.GetFileSystemEntries(citizenDir); // 
                Collection<Citizen> citizens = new Collection<Citizen>();
                for (int i = 0; i < citizenFiles.Length; i++)
                {
                    if (File.Exists(citizenFiles[i]))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        string filename = citizenFiles[i];
                        FileStream stream = new FileStream(filename, FileMode.Open);
                        CitizenData C = bf.Deserialize(stream) as CitizenData;
                        Citizen citizen = new Citizen(C);
                        citizens.Add(citizen);
                        stream.Close();
                    }
                }
                return citizens;
            }
            Debug.LogError("Something wrong happened while trying to load citizen data");
            Application.Quit();
            return null;

        }

        public static void SaveCitizens(Town town, Collection<Citizen> citizens)
        {
            string citizenDir = Application.persistentDataPath + "/" + town.ID + "/citizenData/";
            if (!Directory.Exists(citizenDir))
            {
                Directory.CreateDirectory(citizenDir);
            }

            BinaryFormatter bf = new BinaryFormatter();
        }

        public static void SaveBuildings(Town town)
        {
            string buildingDir = Application.persistentDataPath + "/" + town.ID + "/buildingData/";
            if (!Directory.Exists(buildingDir))
            {
                Directory.CreateDirectory(buildingDir);
            }
        }

        
        
        public static HashSet<Building> LoadBuildings(Town town)
        {
            string buildingDir = Application.persistentDataPath + "/" + town.ID + "/buildingData/";
            if (Directory.Exists(buildingDir))
            {
                string[] buildingFiles = Directory.GetFiles("buildingDir");
                HashSet<Building> buildings = new HashSet<Building>();
                for (int i = 0; i < buildingFiles.Length; i++)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    string fileName = buildingFiles[i];
                    if (File.Exists(fileName))
                    {
                        FileStream stream = new FileStream(fileName, FileMode.Open);
                        if (buildingFiles[i].Contains("hospital"))
                        {
                            HospitalData h = bf.Deserialize(stream) as HospitalData;
                            Hospital hospital = new Hospital(h);
                            Game.town.emergency.Add(hospital);
                            buildings.Add(Game.town.emergency[Game.town.emergency.Count-1]);
                        }
                        else if (buildingFiles[i].Contains("shop"))
                        {
                            SupermarketData s = bf.Deserialize(stream) as SupermarketData;
                            Supermarket supermarket = new Supermarket(s);
                            Game.town.essentials.Add(supermarket);
                            buildings.Add(Game.town.emergency[Game.town.essentials.Count-1]);
                        }
                        else
                        {
                            BuildingData b = bf.Deserialize(stream) as BuildingData;
                            Building building = new Building(b);
                            buildings.Add(building);
                        }
                        stream.Close();
                    }
                }
                
                return buildings;
            }

            return null;
        }
        
    }
>>>>>>> Stashed changes
}