using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface ICommentService
    {
        Task<BaseServiceResponse> Create(CommentModel model);
        Task<ServiceListResponse<CommentModel>> GetAll(int financialYearId, int departmentId, int gradeId, int roleTypeId, bool IsCEO);
    }
}
