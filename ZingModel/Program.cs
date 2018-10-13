using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZingModel.Models;
namespace ZingModel
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee emp = Employee.GetEmployee("000150");
            LeaveApp leaveApp = new LeaveApp()
            {

                ApprovalStatus = ApplicationStatusEnum.Open,
                LeaveApplicationType = ApplicationTypeEnum.Leave,
                EmplId = emp.PersonnelNumber,
                RequestedBy = emp.Worker,
                ScheduledLeaveDate = new DateTime(2018, 11, 1),
                ScheduledReturnDate = new DateTime(2018, 11, 5),
                ExitVisaType = ExitVisaTypeEnum.ExitReentryMultiple,
                Comments="The family concernes "

            };
            leaveApp.Insert();
            LeaveApp.Print(leaveApp);
            Console.Read();
        }
    }
}
