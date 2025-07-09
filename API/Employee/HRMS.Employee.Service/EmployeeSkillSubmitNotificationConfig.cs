using HRMS.Common;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Models.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace HRMS.Employee.Service
{

    /// <summary>
    /// Utility class used to construct notification content when an
    /// employee submits skills for approval.
    /// </summary>
    internal class EmployeeSkillSubmitNotificationConfig
    {
        private readonly IConfiguration m_config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSkillSubmitNotificationConfig"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        public EmployeeSkillSubmitNotificationConfig(IConfiguration configuration)
        {
            m_config = configuration;
        }

        /// <summary>
        /// Builds the notification details for the reporting manager
        /// using the provided skill details and associate information.
        /// </summary>
        /// <param name="skillDetails">List of submitted skills.</param>
        /// <param name="employeeDetails">Employee submitting the skills.</param>
        /// <returns>Populated <see cref="NotificationDetail"/> instance.</returns>
        public static NotificationDetail EmpSkillNotificationConfig(List<EmployeeSkillDetails> skillDetails, EmployeeDetails employeeDetails )
        {
            string FilePath = Utility.GetNotificationTemplatePath(NotificationTemplatePaths.currentDirectory,NotificationTemplatePaths.subDirectories_Employee_Skill_Submit);
            StreamReader stream = new StreamReader(FilePath);
            string MailText = stream.ReadToEnd();
            stream.Close();
          
            string generatedHTMLtable = GenerateHTMLTable(skillDetails);

            MailText = MailText.Replace("{ReportingManager}", employeeDetails.ReportingManager);
            MailText = MailText.Replace("{AssociateName}", employeeDetails.EmpName);
            MailText = MailText.Replace("{EmployeeCode}", employeeDetails.EmployeeCode);           
            MailText = MailText.Replace("{table}", generatedHTMLtable);

            NotificationDetail notificationDetail = new NotificationDetail();
            notificationDetail.Subject = employeeDetails.EmpName + " " + "has submitted skill(s) for approval";
            notificationDetail.EmailBody = MailText;          
            return notificationDetail;
        }

        /// <summary>
        /// Generates an HTML table representation of the submitted skills.
        /// </summary>
        /// <param name="skillDetails">List of skills to include in the table.</param>
        /// <returns>Formatted HTML string.</returns>
        public static string GenerateHTMLTable(List<EmployeeSkillDetails> skillDetails)
        {
            string tableHtml = "";
            DataTable dt = new DataTable("SkillsTable");
            dt.Columns.Add(new DataColumn("S.No", typeof(int)));
            dt.Columns.Add(new DataColumn("SkillName", typeof(string)));
            dt.Columns.Add(new DataColumn("Version(s)", typeof(string)));
            dt.Columns.Add(new DataColumn("CurrentProficiencyLevel", typeof(string)));
            dt.Columns.Add(new DataColumn("ProficiencyLevel", typeof(string)));
            dt.Columns.Add(new DataColumn("CurrentLastUsed", typeof(string)));
            dt.Columns.Add(new DataColumn("NewLastUsed", typeof(string)));
            dt.Columns.Add(new DataColumn("CurrentExperience", typeof(string)));
            dt.Columns.Add(new DataColumn("NewExperience", typeof(string)));
            int i = 1;
            foreach (var skill in skillDetails)
            {
               
                DataRow dr = dt.NewRow();
                dr["S.No"] =i;
                dr["SkillName"] = skill.SkillName;
                dr["Version(s)"] = skill.Version;
                dr["CurrentProficiencyLevel"] = skill.CurrentProficiencyLevelCode;
                dr["ProficiencyLevel"] = skill.ProficiencyLevelCode;
                dr["CurrentLastUsed"] = skill.LastUsed;
                dr["NewLastUsed"] = skill.NewLastUsed;
                dr["CurrentExperience"] = skill.Experience;
                dr["NewExperience"] = skill.NewExperience ;
                dt.Rows.Add(dr);
                i++;
            }

            tableHtml += "<table>";
            tableHtml += "<tr  style=white-space:nowrap><th>S.no</th><th>Skill Name</th><th>Version(s)</th><th>Current Proficiency Level</th><th>New Proficiency Level</th><th>Current Last Used</th><th>New Last Used</th><th>Current Experience</th><th>New Experience</th></tr>";

            foreach (DataRow drSkill in dt.Rows)
            {
                tableHtml += "<tr><td>" + drSkill["S.No"] + "</td><td>" + drSkill["SkillName"] + "</td><td style=word-break:break-all;>" + drSkill["Version(s)"] + "</td><td>" + drSkill["CurrentProficiencyLevel"] +"</td><td>"+ drSkill["ProficiencyLevel"] + "</td>" +
                 "<td>"+ drSkill["CurrentLastUsed"] + " </td ><td> " + drSkill["NewLastUsed"] + " </td> <td> " + drSkill["CurrentExperience"] + " </td> <td> " + drSkill["NewExperience"] + " </td> </tr>";
            }
            tableHtml += "</table>";
            return tableHtml;
        }
    }
}