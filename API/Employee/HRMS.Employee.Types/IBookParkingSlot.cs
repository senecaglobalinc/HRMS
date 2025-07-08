using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
   public interface IBookParkingSlot
    {
        Task<ServiceResponse<BookedParkingSlotDetails>> Create(BookedParkingSlotDetails bookedParkingSlot);       
        Task<ServiceResponse<AvailableSlots>> GetSlotDetails(string PlaceName);
        Task<ServiceResponse<BookedParkingSlotDetails>> GetSlotDetailsByEmailID(string EmailID);
        Task<ServiceResponse<BookedParkingSlotDetails>> ReleaseSlot(string email);
    }
}
