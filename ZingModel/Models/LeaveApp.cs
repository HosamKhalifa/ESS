using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dynamics;
using Microsoft.Dynamics.BusinessConnectorNet;
using System.Web.Script.Serialization;

namespace ZingModel.Models
{
    class LeaveApp
    {
        #region Constants
        const string TABLE_NAME = "PAYFLeaveApplication";
        const string APPLICATION_ID = "ApplicationId";
        const string CREATE_DATETIME = "CreatedDateTime";
        const string LEAVE_APP_TYPE = "ApplicationType";
        const string LEAVE_CODE = "DefaultLeaveCode";
        const string EMPLOYEE_ID = "EmplId";
        const string REQUESTED_BY = "PAYFRequestBy";
        const string SCH_LEAVE_DATE = "ScheduledLeaveDate";
        const string SCH_RETURN_DATE = "ScheduledReturnDate";
        const string DAYS = "TotalLeaveDays";
        const string APPROVAL_STATUS = "ApprovalStatus";
        const string EXIT_VISA_TYPE = "ExitVisaType";
        const string COMMENTS = "Comments";
        const string COMMENTS_APPROVAL = "CommentsApproval";
        #endregion

        #region Fields
        public string ApplicationId { get; set; } //Leave application id
        public DateTime CreatedDateTime { get; set; }
        public string LeaveApplicationType { get; set; }//Enum {Leave,Termination,Resignation,Escape,Death}
        public string DefaultLeaveCode { get; set; }//Lookup table PayfLeaveCode
        public string EmplId { get; set; }
        public long RequestedBy { get; set; } //Worker Id 
        public DateTime ScheduledLeaveDate { get; set; }//From date;
        public DateTime ScheduledReturnDate { get; set; }//To Date
        public int Days { get; set; }//Computed on Employee Calender
        public string ApprovalStatus { get; set; }//Enum{Open,ReportAsReady,Approved,Rejected,Submitted,ChangeRequest}
        public string ExitVisaType { get; set; }//Enum {None,ExitReentry,FinalExit,ExitReentryMultiple}
        public string Comments { get; set; }
        public string CommentsApproval { get; set; }
        #endregion

        #region Methods
        public static List<LeaveApp> GetLeaveApps()
        {
            var leaveApps = new List<LeaveApp>();
            Axapta ax;
            AxaptaRecord axRecord;

            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Axapta();
                ax.Logon(null, null, null, null);

                // Create a query using the AxaptaRecord class
                using (axRecord = ax.CreateAxaptaRecord(TABLE_NAME))
                {
                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select * from %1 ");

                    while (axRecord.Found)
                    {
                        var leaveApp = new LeaveApp()
                        {
                            ApplicationId = axRecord.get_Field(APPLICATION_ID).ToString(),
                            CreatedDateTime = (DateTime)axRecord.get_Field(CREATE_DATETIME),
                            LeaveApplicationType = axRecord.get_Field(LEAVE_APP_TYPE).ToString(),
                            DefaultLeaveCode = axRecord.get_Field(LEAVE_CODE).ToString(),
                            EmplId = axRecord.get_Field(EMPLOYEE_ID).ToString(),
                            RequestedBy = (long)axRecord.get_Field(REQUESTED_BY),
                            ScheduledLeaveDate = (DateTime)axRecord.get_Field(SCH_LEAVE_DATE),
                            ScheduledReturnDate = (DateTime)axRecord.get_Field(SCH_RETURN_DATE),
                            Days = (int)axRecord.get_Field(DAYS),
                            ApprovalStatus = axRecord.get_Field(APPROVAL_STATUS).ToString(),
                            ExitVisaType = axRecord.get_Field(EXIT_VISA_TYPE).ToString(),
                            Comments = axRecord.get_Field(COMMENTS).ToString(),
                            CommentsApproval = axRecord.get_Field(COMMENTS_APPROVAL).ToString()
                            
                        };
                        leaveApps.Add(leaveApp);
                        axRecord.Next();
                    }
                    return leaveApps;


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered: {0}", ex.Message);
                throw;
            }
        }
        public static void Print(LeaveApp item)
        {
            var jSON = new JavaScriptSerializer().Serialize(item);
            Console.WriteLine(jSON);
        }

        public void Update()
        {
            Axapta ax;
            AxaptaRecord axRecord;
            try
            {
                ax = new Axapta();
                ax.Logon(null, null, null, null);
                using (axRecord = ax.CreateAxaptaRecord(TABLE_NAME))
                {
                    string sql = string.Format("select forupdate * from %1 where %1.{0} == '{1}'", APPLICATION_ID, this.ApplicationId);
                    axRecord.ExecuteStmt(sql);
                    if (axRecord.Found)
                    {
                        ax.TTSBegin();
                        axRecord.set_Field(LEAVE_APP_TYPE, this.LeaveApplicationType);
                        axRecord.set_Field(SCH_LEAVE_DATE, this.ScheduledLeaveDate);
                        axRecord.set_Field(SCH_RETURN_DATE, this.ScheduledReturnDate);
                        axRecord.set_Field(APPROVAL_STATUS, this.ApprovalStatus);
                        axRecord.set_Field(EXIT_VISA_TYPE, this.ExitVisaType);
                        axRecord.set_Field(COMMENTS, this.Comments);
                        axRecord.set_Field(COMMENTS_APPROVAL, this.CommentsApproval);
                        axRecord.Update();
                        ax.TTSCommit();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public  void Insert()
        {
            Axapta ax;
            AxaptaRecord axRecord;
            try
            {
                ax = new Axapta();
                ax.Logon(null, null, null, null);
                using (axRecord = ax.CreateAxaptaRecord(TABLE_NAME))
                {
                  
                    ax.TTSBegin();
                  
                    axRecord.Call("ESS_WriteValues",
                                   this.LeaveApplicationType,
                                   this.EmplId,
                                   this.RequestedBy,
                                   this.ScheduledLeaveDate,
                                   this.ScheduledReturnDate,
                                   this.ApprovalStatus,
                                   this.ExitVisaType,
                                   this.Comments,
                                   this.CommentsApproval);
                    axRecord.Insert();
                    ax.TTSCommit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        #endregion
    }
}
