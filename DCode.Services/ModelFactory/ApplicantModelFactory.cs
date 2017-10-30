using DCode.Data.DbContexts;
using DCode.Models.ResponseModels.Contributor;
using DCode.Services.ModelFactory.CommonFactory;
using DCode.Models.ResponseModels.Contributor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCode.Common;
using DCode.Models.ResponseModels.Requestor;

namespace DCode.Services.ModelFactory
{
    public class ApplicantModelFactory //: IModelFactory<applicant>
    {

        //public TModel CreateModel<TModel>(user input) where TModel : class
        //{
        //    if (typeof(TModel) == typeof(ContributorSummary))
        //    {
        //        return TranslateSummary(input) as TModel;
        //    }
        //    else if (typeof(TModel) == typeof(PermissionsSummary))
        //    {
        //        return TranslatePermissionsSummary(input) as TModel;
        //    }
        //    else
        //    {
        //        return Translate(input) as TModel;
        //    }
        //}

        //private Models.ResponseModels.Contributor.Contributor Translate(user input)
        //{
        //    var contributor = new Models.ResponseModels.Contributor.Contributor();
        //    if (input != null)
        //    {
        //        contributor.ApplicantId = input.ID;
        //        contributor.CompletedHours = (input.approvedapplicants != null && input.approvedapplicants.Count > 0) ? input.approvedapplicants.Where(x => x.APPLICANT_ID == input.ID).FirstOrDefault().HOURS_WORKED : 0;
        //        contributor.CreatedBy = input.CREATED_BY;
        //        contributor.CreatedOn = input.CREATED_ON;
        //        //contributor.Designation = input.
        //        contributor.EmailId = input.EMAIL_ID;
        //        //contributor.Expertise = input.ex
        //        contributor.FirstName = input.FIRST_NAME;
        //        contributor.LastName = input.LAST_NAME;
        //        contributor.Status = input.STATUS;
        //        contributor.StatusDate = input.STATUS_DATE;
        //        //contributor.TopRatingsCount = input.
        //        contributor.UpdatedBy = input.UPDATED_BY;
        //        contributor.UpdatedOn = input.UPDATED_ON;
        //        contributor.Duration = CommonHelper.CalculateDuration(input.CREATED_ON);
        //    }
        //    return contributor;
        //}

        //private ContributorSummary TranslateSummary(user input)
        //{
        //    var contributorSummary = new ContributorSummary();
        //    if (input != null)
        //    {
        //        contributorSummary.ApplicantId = input.ID;
        //        //contributorSummary.Designation = input.DESIGNATION;
        //        contributorSummary.EmailId = input.EMAIL_ID;
        //        contributorSummary.FirstName = input.FIRST_NAME;
        //        contributorSummary.LastName = input.LAST_NAME;
        //    }
        //    return contributorSummary;
        //}

        //private PermissionsSummary TranslatePermissionsSummary(applicant input)
        //{
        //    var permissionsSummary = new PermissionsSummary();
        //    if (input != null)
        //    {
        //        permissionsSummary.ApplicantId = input.ID;
        //        //contributorSummary.Designation = input.DESIGNATION;
        //        //permissionsSummary.EmailId = input.EMAIL_ID;
        //        permissionsSummary.FirstName = input.FIRST_NAME;
        //        permissionsSummary.LastName = input.LAST_NAME;
        //        permissionsSummary.Name = input.FIRST_NAME + Constants.Space + input.LAST_NAME;
        //        permissionsSummary.Designation = input.DESIGNATION;
        //        permissionsSummary.Skills = input.EXPERTISE;
        //        permissionsSummary.Status = input.STATUS;
        //    }
        //    return permissionsSummary;
        //}

        //public applicant CreateModel<TModel>(applicant input, TModel model) where TModel : class
        //{
        //    throw new NotImplementedException();
        //}

        //public applicant CreateModel<TModel>(TModel input) where TModel : class
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<applicant> inputList) where TModel : class
        //{
        //    var modelList = new List<Models.ResponseModels.Contributor.Contributor>();
        //    foreach (var dbApplicant in inputList)
        //    {
        //        var contributor = new Models.ResponseModels.Contributor.Contributor();
        //        contributor = CreateModel<Models.ResponseModels.Contributor.Contributor>(dbApplicant);
        //        modelList.Add(contributor);
        //    }
        //    return modelList as IEnumerable<TModel>;
        //}
    }
}
