using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/* The ReadFile script contains functions for reading the log used throughout
   the project. These include getting amount of satellites and parsing commands.
   
   Created by : Antonio Castro
 */

public class ReadFile : MonoBehaviour {
    public static int yawLocation = 41;                            // Location in array for yaw
    public static int trueAnomalyLocation = 69;                    // Location in array for true anomaly
    public static int semimajorAxisLocation = 65;                  // Location of semimajorAxis
    public static int inclinaitionLocation = 66;                   // Location of inclination
    public static string logFile = "Assets\\Test2.csv";            // Log file location on computer
    public static int numOfSats = ReadFile.numSats();              // Number of satellites acording to the log file
    public static int heartBeat = ReadFile.ourheartBeat();         // Update interval (Hearbeat)
    public static int time = 1;
    public string logFileNew;
    public int timeNew = 1;
    public static int numOfSatsPhysical = 0;

    private void Start()
    {
        logFile = "Assets\\" + logFileNew;
        time = timeNew;
        numOfSats = ReadFile.numSats();
        heartBeat = ReadFile.ourheartBeat();
    }

    public static string[] ReadCommandLine(int line, string file)   // Get entire line of commands for a satellite and splits them by commas
    {
        string[] lines = File.ReadAllLines(file);
        string ourline = lines[line];
        char[] delimiter = { ',' };
        string[] fields = ourline.Split(delimiter);
        return fields;
    }

    public static double[] ReadCommand(string[] commandLine, int commandNum)      // Reads in a single command and slpits it by spaces
    {
        double[] Command = new double[2];
        string ourCommand = commandLine[commandNum];
        char[] delimiter = { ' ' };
        string[] fields = ourCommand.Split(delimiter);
        Command[0] = Convert.ToDouble(fields[1]);
        Command[1] = Convert.ToDouble(fields[2]);
        return Command;
    }

    public static int countLinesInFile(String file)                               // Return number of lines in file
    {
        int count = 0;
        using (StreamReader r = new StreamReader(file))
        {
            while (r.ReadLine() != null)
            {
                count++;
            }
        }
        return count;
    }

    public static int countLinesInFileCut(String file)                               // Return number of lines in file and cut off unfinished logd
    {
        int numSatsHere = numSats();
        int count = 0;
        using (StreamReader r = new StreamReader(file))
        {
            while (r.ReadLine() != null)
            {
                count++;
            }
        }
        numSatsHere = (count-1) % numSatsHere;
        count = count - numSatsHere;
        return count;
    }

    public static string[] ReadCommandLinecsv(int line)                           // Splits lines in log file by , and returns an array of all the fields
    {
        string[] lines = File.ReadAllLines(ReadFile.logFile);
        string ourline = lines[line];
        char[] delimiter = { ',' };
        string[] fields = ourline.Split(delimiter);
        return fields;
    }

    public static int numSats()                                                  // Returns number of sats for this playback
    {
        int numOfSats = 0;
        int numOfSatsPhys = 0;
        string currentSat;
        int numOfLines = ReadFile.countLinesInFile(ReadFile.logFile);
        int satCount = 0;                                                       // Ammount of satelites
        string physicalOffset;
        int physicalCount = 0;
        for (int i = 1; i < numOfLines; i++)
        {
            string[] logLines = ReadCommandLinecsv(i);
            currentSat = logLines[2];
            string checkPhys = currentSat.Substring(0,(2));
            if (checkPhys.Equals("ES"))
            {
                physicalOffset = currentSat.Substring(2, (currentSat.Length - 2));
                physicalCount = int.Parse(physicalOffset);
                if (physicalCount >= numOfSatsPhys)
                {
                    numOfSatsPhys++;
                    numOfSatsPhysical = numOfSatsPhys;
                }
                else
                {
                    return numOfSats + numOfSatsPhys;
                }
            }
            else
            {
                currentSat = currentSat.Substring(3, (currentSat.Length - 3));
                satCount = int.Parse(currentSat);
                if (satCount >= numOfSats)
                {
                    numOfSats++;                                            // Counts num of sats

                }
                else
                {
                    return numOfSats + numOfSatsPhys;
                }
            }
        }
        return numOfSats+numOfSatsPhys;
    }

    public static int ourheartBeat()              // Time between updates
    {
        string lineI = ReadCommandLinecsv(numOfSatsPhysical+ 1)[1];
        string lineF = ReadCommandLinecsv(1 + numOfSats + numOfSatsPhysical)[1];
        char[] delimiter = { ':' };
        int fieldsI = int.Parse(lineI.Split(delimiter)[2].Substring(0,2));
        int fieldsF = int.Parse(lineF.Split(delimiter)[2].Substring(0,2));
        return fieldsF - fieldsI;
    }

}
