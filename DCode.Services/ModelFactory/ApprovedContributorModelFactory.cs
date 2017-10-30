using DCode.Common;
using DCode.Data.DbContexts;
using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Contributor;
using DCode.Services.ModelFactory.CommonFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCode.Services.ModelFactory
{
    public class ApprovedContributorModelFactory //: IModelFactory<approvedapplicant>
    {
        //public TModel CreateModel<TModel>(approvedapplicant input) where TModel : class
        //{
        //    var approvedApplicant = new ApprovedContributor();
        //    if (input != null)
        //    {
        //        approvedApplicant.ApplicantId = input.APPLICANT_ID;
        //        approvedApplicant.ApplicantStatus = EnumHelper.ConvertToEnumApplicantStatus(input.APPLICANT_STATUS);
        //        approvedApplicant.Comments = input.COMMENTS;
        //        approvedApplicant.CreatedBy = input.CREATED_BY;
        //        approvedApplicant.CreatedOn = input.CREATED_ON;
        //        approvedApplicant.HoursWorked = input.HOURS_WORKED;
        //        approvedApplicant.Points = input.POINTS;
        //        approvedApplicant.Rating = input.RATING;
        //        approvedApplicant.WorkAgain = input.WORK_AGAIN;
        //        approvedApplicant.Id = input.ID;
        //    }
        //    return approvedApplicant as TModel;
        //}

        //public approvedapplicant CreateModel<TModel>(approvedapplicant input, TModel model) where TModel : class
        //{
        //    throw new NotImplementedException();
        //}

        //public approvedapplicant CreateModel<TModel>(TModel input) where TModel : class
        //{
        //    if(typeof(TModel) == typeof(ReviewTaskRequest))
        //    {
        //        return TranslateRequest(input as ReviewTaskRequest);
        //    }
        //    return null;
        //}

        //private approvedapplicant TranslateRequest(ReviewTaskRequest input)
        //{
        //    var approvedApplicant = new approvedapplicant();
        //    approvedApplicant.APPLICANT_ID = input.ApplicantId;
        //    approvedApplicant.ID = input.ApprovedApplicantId;
        //    approvedApplicant.RATING = input.Rating;
        //    approvedApplicant.COMMENTS = input.Comments;
        //    approvedApplicant.WORK_AGAIN = input.WorkAgain;
        //    return approvedApplicant;
        //}

        //public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<approvedapplicant> inputList) where TModel : class
        //{
        //    var modelList = new List<ApprovedContributor>();
        //    foreach (var dbApprovedApplicants in inputList)
        //    {
        //        var task = new ApprovedContributor();
        //        task = CreateModel<ApprovedContributor>(dbApprovedApplicants);
        //        modelList.Add(task);
        //    }
        //    return modelList as IEnumerable<TModel>;
        //}
    }
}
