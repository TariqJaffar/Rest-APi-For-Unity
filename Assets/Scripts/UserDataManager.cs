using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance;
    public int EmployeeIndex; 
    public delegate void DataRaiseEvent(int Ind);//Make a Static Delegate to raise the event and manage the data With button And employee Index Number Pass as a Parameters  
    public static DataRaiseEvent EmpData;  
    public TextMeshProUGUI NameEmp;
    public TextMeshProUGUI AgeEmp;
    public TextMeshProUGUI IDEmp;
    public TextMeshProUGUI SalaryEmp;
    public List <string> Name =new List<string>();
    public  List< string> Age=new List<string>();   
    public List<string> ID = new List<string>();
    public List<string> Salary = new List<string>();
    [Header ("For Create User")]
  
    public TMP_InputField NameOFEmp;
    public TMP_InputField AgeOFEmp;
    public TMP_InputField SalaryOFEmp;
    public Button CreateEmployee;
    [Header("To Update User Data")]
    public TMP_InputField UpdateNameOFEmp;
    public TMP_InputField UpdateAgeOFEmp;
    public TMP_InputField UpdateSalaryOFEmp;
    public TMP_InputField EmployeeID;

    public Button UpdateEmployee;

    public void Awake()
    {
        Instance = this;// Using the singleton design pattern i make the class static and put the refference of the class in it so i can call any function of this class anywhere with class name
        EmpData += DataArrangement;//i put the refference of the Function into my delegate 

    }
    //Make a function to Pass The Refference in Button And see the Next User Data with condition to not exceed the limit of employee 
    public void NextBUtton()
    {
        
        if (EmployeeIndex <= Name.Count)
        {
            EmployeeIndex++;

            EmpData.Invoke(EmployeeIndex);
        }
        else
        {
            Debug.Log("Index Out Of range");
        }



    }
    //Make a function to Pass The Refference in Button And see the previous User Data with condition to not exceed the limit of employee 
    public void PrevBUtton()
    {
        if (EmployeeIndex >= 0)
        {
            EmployeeIndex--;

            EmpData.Invoke(EmployeeIndex);
        }
        else
        {
            Debug.Log("Index Out Of range");
        }
    }


    public void DataArrangement(int EmployeeInd)
    {
        NameEmp.text = Name[EmployeeInd];
        AgeEmp.text = Age[EmployeeInd];
        IDEmp.text = ID[EmployeeInd];
        SalaryEmp.text = Salary[EmployeeInd];
    }


}
