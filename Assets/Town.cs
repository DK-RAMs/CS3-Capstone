using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using CitizenLibrary;
using UnityEngine;

public class Town : MonoBehaviour
{
    private string id, mayor;
    private static int day, time;
    private static long updateTickRate, dayLength; // For now, the length of a day will be 1 minute (60 seconds). The updateTickRate will be
    private Collection<Building> buildings;
    private CitizenWorkerThread[] workerThreads;
    private static double baseDetalHappiness, baseCitisenRisk, averageHappiness, averageHealth;
    private double money, deltaMoney;
    private Collection<Policy> policies;
    private Stopwatch timer;
    

    public Town(string id, int day, int time, long dayLength, long updateTickRate, double baseDetalHappiness, double baseCitisenRisk)
    {
        this.id = id;
        Town.day = day;
        Town.time = time;
        Town.dayLength = dayLength;
        Town.updateTickRate = updateTickRate;
        Town.baseDetalHappiness = baseDetalHappiness;
        Town.baseCitisenRisk = baseCitisenRisk;
    }

    public void start()
    {
        
    }
    
    public void update()
    {
        if (timer.ElapsedMilliseconds >= updateTickRate)
        {
            timer.Reset();
            incrementTime();
            timer.Start();

        }
    }

    private void incrementTime()
    {
        time++;
        if (time > 23)
        {
            time = 0;
            day++;
        }
        Thread.Sleep(100); 
    }
    
    #region Setters & Getters
    public long UpdateTickRate
    {
        get => updateTickRate;
        set => Town.updateTickRate = value;
    }

    public static int Time
    {
        get => time;
    }

    public static int Day
    {
        get => day;
    }

    public static double BaseDetalHappiness
    {
        get => baseDetalHappiness;
    }

    public static double BaseCitisenRisk
    {
        get => baseCitisenRisk;
    }

    public void updateDeltaHappiness(double factor)
    {
        baseDetalHappiness += factor;
    }
    
    #endregion
}
