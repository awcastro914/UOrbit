using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The DynamicInfo Script alters the TextMesh attached to each of the satellites
  to allow for an easy display of kepler parameters and other status updates with
  the satellite. Each of the fields in the text mesh can be altered using the editor
  through the use of checkboxes in the editor.
   
   Created by : Antonio Castro
 */

public class DynamicInfo : MonoBehaviour {
    private string name;                                              //Name of our satellite
    TextMesh ourInfo;                                                 //Text mesh attached to the satellite
    Vector3 cubeLocation;                                             //Location of cube to follow
    private Vector3 rotationOffset;                                   //Rotational offset of text mesh
    private Vector3 offset;                                           //Linear offset of textmesh
    public bool nameSat = true;                                       //Checkbox to display satellite name  
    public bool x = true;                                             //Checkbox to display x value
    public bool y = true;                                             //Checkbox to display y value           
    public bool z = true;                                             //Checkbox to display z value
    public bool rotaton_Speed = false;                                //Checkbox to display rotaion speed
    public bool inclination = false;                                  //Checkbox to display inclination
    public bool true_Anomaly = false;                                 //Checkbox to display true anomaly
    private bool newLine = false;                                     //Flag to separate information on new lines if needed 
    private float initRot;                                            //Following variables through line 31 are used to calculate rotational speed of cube
    private float initRotTime;                                        
    private float finalRot;                                           
    private float finalRotTime;                                       
    private int toggle = 0;                                           
    private float inclinationValue;                                   //Inclination value to display
    private float trueAnomalyValue;                                   //True anomaly after a set time
    private float trueAnomalyinit;                                    //True anomaly initially 
    private float counter;                                            //Used to keep track of time
    float trueAnomalyValueIncrement = 0;                              //Overlap over 360 degrees
    // Use this for initialization
    void Start () {
        name = gameObject.name.Substring(0,1);                                                                                  //Get name of our satellite
        ourInfo = gameObject.GetComponent(typeof(TextMesh)) as TextMesh;                                                        //Get textMesh component of our satellite
        int didCon;                                                                                                             //Check for valid satellite
        if (int.TryParse(name, out didCon))
        {
            string[] commandLine = ReadFile.ReadCommandLine(int.Parse(name) + 1, ReadFile.logFile);                             //Get initial location of satellites information in log file
            string[] commandLine2 = ReadFile.ReadCommandLine(int.Parse(name) + 1 + ReadFile.numOfSats * 3, ReadFile.logFile);       //Get next line location of satellites information in log file
            string inclinationVal = commandLine[ReadFile.inclinaitionLocation];                                                 //Get inclination value for the satellite
            inclinationValue = EasyOrbit.CheckString(inclinationVal);                                                           //Inclination from string to float
            string trueAnomalyValInit = commandLine[ReadFile.trueAnomalyLocation];                                              //True anomlay initial location
            string trueAnomalyValFinal = commandLine2[ReadFile.trueAnomalyLocation];                                            //True anomaly final location
            trueAnomalyinit = EasyOrbit.CheckString(trueAnomalyValInit);                                                        //String to float 
            trueAnomalyValue = ((EasyOrbit.CheckString(trueAnomalyValFinal) - trueAnomalyinit)/(float)ReadFile.heartBeat);      //String to float
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        int didConvert;
        if (int.TryParse(name, out didConvert))                                                                                 //Only continue if satelite has a valid ID
        {
            if (Time.time < (ReadFile.heartBeat * 2))
            {
            }
            else
            {
                if (Time.time <= EasyOrbit.loops)                                                                                   //Execute only if time of simlulation is not over
                {
                    counter = counter + Time.deltaTime;                                                                             //Update counter to be equal to current time
                    trueAnomalyValueIncrement = (trueAnomalyValue * counter) + trueAnomalyinit;                                     //Keep track of true anomaly of satellite throughout execution
                    while (trueAnomalyValueIncrement > 360 || trueAnomalyValueIncrement < 0)                                        //Account for rollover through 360
                    {
                        if (trueAnomalyValueIncrement > 360)
                        {
                            trueAnomalyValueIncrement = trueAnomalyValueIncrement - 360;
                        }
                        if (trueAnomalyValueIncrement < 0)
                        {
                            trueAnomalyValueIncrement = trueAnomalyValueIncrement + 360;
                        }
                    }
                }
            }
                                                                                                                                //Keep Track of current rotation speed of satellite throughout execution
            if (toggle == 0)                                                                                                    
            {
                initRot = GameObject.Find(name).transform.rotation.eulerAngles.y;                                               //Get initial rotational position of satellite
                initRotTime = Time.time;                                                                                        //Get initial time of this position
                toggle = 1;                                                                                                    
            }
            if (toggle == 1 && ((Time.time -0.5) > initRotTime))    
            {
                finalRot = GameObject.Find(name).transform.eulerAngles.y;                                                       //Get a final rotational position of satellite
                finalRotTime = Time.time;                                                                                       //Get a final time of this position
                toggle = 0;
            }
            float finalRotSpeed = ((finalRot - initRot) / (finalRotTime - initRotTime));                                        //Get rotational diffrence over time diffrence to get degrees per second
            Transform target = GameObject.Find(name).transform;                                                                 //Get the transform location of satellite to follow
            offset[1] = 15;                                                                                                     //Create transform offset for text
            rotationOffset[1] = 90;                                                                                             //Create rotational offset for text
            ourInfo.text = "";                                                                                                  //Clear all text from the textmesh
            if (nameSat == true)                                                                                                //Check if Name should be displayed
            {
                ourInfo.text = "SatNumber " + name;
                newLine = true;                                                                                                 
            }
            
            ourInfo.text = SatString(x,newLine,"X : " + GameObject.Find(name).transform.position.x, ourInfo.text);              //Check if X should be displayed
            ourInfo.text = SatString(y, newLine,"Y : " + GameObject.Find(name).transform.position.y, ourInfo.text);             //Check if Y should be displayed
            ourInfo.text = SatString(z, newLine, "Z : " + GameObject.Find(name).transform.position.z, ourInfo.text);            //Check if Z should be displayed
            ourInfo.text = SatString(rotaton_Speed, newLine, "Rot Speed : " + finalRotSpeed, ourInfo.text);                     //Check if Rotation Speed should be displayed
            ourInfo.text = SatString(inclination, newLine, "Inclination : " + inclinationValue, ourInfo.text);                  //Check if Inclination should be displayed
            ourInfo.text = SatString(true_Anomaly, newLine, "True Anomaly : " + trueAnomalyValueIncrement, ourInfo.text);       //Check if True Anomaly should be displayed
            transform.position = target.position + offset;                                                                      //Textmesh location is location of satellite + offest
            transform.rotation = Quaternion.Euler(rotationOffset);                                                              //Keep rotation of textmesh constant for viewing
        }


    }


    private string SatString(bool check, bool newLinee, string addon, string satString)                                         //Adds lines to the textmesh
    {
        if(check == true)                                                                                                       //Check to see if box is checked if it is add terms
        {
            if(newLinee == true)                                                                                                //Check to see if a new line is needed
            {
                satString = satString + System.Environment.NewLine + addon;                                                     //Combine new term to old string with new line
                newLine = true;
            }
            else
            {
                satString = satString + addon;                                                                                  //Combine new term to old string without new line
                newLine = true;

            }
        }
        return satString;
    }
}
