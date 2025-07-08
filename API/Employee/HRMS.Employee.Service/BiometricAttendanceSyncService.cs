using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class BiometricAttendanceSyncService : IBiometricAttendanceSyncService
    {
        #region Global Varibles
        private readonly IDbService _dbService;
        private readonly EmployeeDBContext _employeeDBContext;
        #endregion

        #region Constructor
        public BiometricAttendanceSyncService(EmployeeDBContext employeeDBContext, IDbService dbService)
        {
            _employeeDBContext = employeeDBContext;
            _dbService = dbService;
        }
        #endregion

        #region GetBiometricAttendance
        public async Task<ServiceListResponse<BiometricAttendance>> GetBiometricAttendance(DateTime dateToSync, string location)
        {
            ServiceListResponse<BiometricAttendance> result = new ServiceListResponse<BiometricAttendance>();
            try
            {
                var biometricAttendanceList = await _dbService.GetAll<BiometricAttendance>("SELECT * FROM public.\"BiometricAttendance\" where \"CreatedDate\" = @dateToSync and \"Location\" = @location", new { dateToSync, location });
                result.IsSuccessful = true;
                result.Items = biometricAttendanceList;
                result.Message = "";
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "An error occurred while fetching the Biometric Attendance data.\nException Message:" + ex.Message + "\nException StackTrace:" + ex.StackTrace;
            }
            return result;
        }
        #endregion

        #region DeleteBiometricAttendance
        public async Task<BaseServiceResponse> DeleteBiometricAttendance(DateTime dateFromSync, DateTime dateToSync, string location)
        {
            BaseServiceResponse result = new BaseServiceResponse();
            try
            {
                var deleteBiometricAttendance = await _dbService.EditData("DELETE FROM public.\"BiometricAttendance\" WHERE \"CreatedDate\" >= @dateFromSync and  \"CreatedDate\" <= @dateToSync AND \"Location\" = @location AND \"UserID\" NOT IN (select \"EmployeeCode\" from \"ExcludedAssociates\" where \"IsActive\" = true)", new { dateFromSync, dateToSync, location });
                result.IsSuccessful = true;
                result.Message = "Biometric Attendance data deleted successfully";
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "An error occurred while deleting the Biometric Attendance data.\nException Message:" + ex.Message + "\nException StackTrace:" + ex.StackTrace;
            }
            return result;
        }
        #endregion

        #region WriteBulkData
        public async Task<BaseServiceResponse> WriteBulkData(DateTime dateFromSync, DateTime dateToSync, List<BiometricAttendance> entities)
        {
            BaseServiceResponse result = new BaseServiceResponse();
            try
            {
                var delReslult = await this.DeleteBiometricAttendance(dateFromSync, dateToSync, "WFO");

                if (delReslult.IsSuccessful)
                {
                    List<BioMetricAttendance> newList = new List<BioMetricAttendance>();
                    foreach (var entity in entities)
                    {
                        BioMetricAttendance bioMetricAttendance = new BioMetricAttendance();
                        bioMetricAttendance.AsscociateId = entity.UserId;
                        bioMetricAttendance.AsscociateName = entity.UserName;
                        if (!string.IsNullOrWhiteSpace(entity.Punch1_Date))
                        {
                            bioMetricAttendance.Punch1_Date = Common.Utility.GetDateTimeInIST(DateTime.ParseExact(entity.Punch1_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));                           
                        }
                        if (!string.IsNullOrWhiteSpace(entity.Punch2_Date))
                        {
                            bioMetricAttendance.Punch2_Date = Common.Utility.GetDateTimeInIST(DateTime.ParseExact(entity.Punch2_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        }
                        bioMetricAttendance.InTime = entity.Punch1_Time;
                        bioMetricAttendance.OutTime = entity.Punch2_Time;
                        bioMetricAttendance.WorkingShift = entity.WorkingShift;
                        bioMetricAttendance.EarlyIn = entity.EarlyIn;
                        bioMetricAttendance.EarlyIn_HHMM = entity.EarlyIn_HHMM;
                        bioMetricAttendance.LateIn = entity.LateIn;
                        bioMetricAttendance.LateIn_HHMM = entity.LateIn_HHMM;
                        bioMetricAttendance.EarlyOut = entity.EarlyOut;
                        bioMetricAttendance.EarlyOut_HHMM = entity.EarlyOut_HHMM;
                        bioMetricAttendance.WorkTime = entity.WorkTime;
                        bioMetricAttendance.WorkTime_HHMM = entity.WorkTime_HHMM;
                        bioMetricAttendance.SUMMARY = entity.SUMMARY;
                        bioMetricAttendance.Location = "WFO";
                        newList.Add(bioMetricAttendance);
                    }
                    _employeeDBContext.BioMetricAttendenceDetail.AddRange(newList);
                    await _employeeDBContext.SaveChangesAsync();
                    result.IsSuccessful = true;
                    result.Message = "Biometric Attendance data inserted successfully";
                }
                else
                {
                    result.IsSuccessful = true;
                    result.Message = "An error occurred. Please contact administrator\n" + delReslult.Message;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "An error occurred while inserting the Biometric Attendance data.\nException Message:" + ex.Message + "\nException StackTrace:" + ex.StackTrace;
            }
            return result;
        }
        #endregion

        #region GetExcludedAssociates
        public async Task<ServiceListResponse<ExcludedAssociates>> GetExcludedAssociates()
        {
            ServiceListResponse<ExcludedAssociates> result = new ServiceListResponse<ExcludedAssociates>();
            try
            {
                var excludedAssociatesList = await _dbService.GetAll<ExcludedAssociates>("SELECT * FROM public.\"ExcludedAssociates\" where \"IsActive\" = True");
                result.IsSuccessful = true;
                result.Items = excludedAssociatesList;
                result.Message = "";
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.Message = "An error occurred while fetching the Excluded Associates data.\nException Message:" + ex.Message + "\nException StackTrace:" + ex.StackTrace;
            }
            return result;
        }
        #endregion
    }
}
