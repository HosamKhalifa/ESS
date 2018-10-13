using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dynamics;
using Microsoft.Dynamics.BusinessConnectorNet;
namespace ZingModel.Models
{
    public class Employee
    {
        const string PERSONNEL_NUMBER = "PersonnelNumber";
        const string WORKER = "recid";
        const string EMPL_COMPANY = "DataAreaId";
        const string EMPL_NAME = "name";
        const string TABLE_NAME = "HcmWorker";
        #region Fields
        public long Worker { get; set; }
        public string PersonnelNumber { get; set; }
        public string Name { get; set; }
        public string EmplCompany { get; set; } // AX Company for create Employee Leaves
        #endregion

        #region Methods
        public static List<Employee> GetEmployees(string _supervisor = "")
        {
            var employees = new List<Employee>();
            Axapta ax;
            AxaptaRecord axRecord;

            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.Logon(null, null, null, null);

                // Create a query using the AxaptaRecord class
                // for the StateAddress table.
                using (axRecord = ax.CreateAxaptaRecord(TABLE_NAME))
                {

                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select * from %1 ");

                    while (axRecord.Found)
                    {
                        var emp = new Employee()
                        {
                            PersonnelNumber = axRecord.get_Field(PERSONNEL_NUMBER).ToString(),
                            Worker = (long)axRecord.get_Field(WORKER),
                            EmplCompany = axRecord.Company,
                            Name = axRecord.Call("name").ToString()
                        };
                        employees.Add(emp);
                        axRecord.Next();
                    }
                    return employees;


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered: {0}", ex.Message);
                throw;
            }


        }
        public static Employee GetEmployee(string personnelNumber)
        {
            Axapta ax;
            AxaptaRecord axRecord;

            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.Logon(null, null, null, null);

                // Create a query using the AxaptaRecord class
                // for the StateAddress table.
                using (axRecord = ax.CreateAxaptaRecord(TABLE_NAME))
                {

                    // Execute the query on the table.
                    axRecord.ExecuteStmt(string.Format("select * from %1 where %1.PersonnelNumber == '{0}'",personnelNumber));

                    if (axRecord.Found)
                    {
                        var emp = new Employee()
                        {
                            PersonnelNumber = axRecord.get_Field(PERSONNEL_NUMBER).ToString(),
                            Worker = (long)axRecord.get_Field(WORKER),
                            EmplCompany = axRecord.Company,
                            Name = axRecord.Call("name").ToString()
                        };
                        return emp;
                    }
                    else
                    {
                        return null;
                    }
                   


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered: {0}", ex.Message);
                return null;
            }
        }
        #endregion

    }
}
