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
    public class TaskApplicantModelFactory : IModelFactory<taskapplicant>
    {
        
        public TModel CreateModel<TModel>(taskapplicant input) where TModel : class
        {
            if (typeof(TModel) == typeof(ContributorSummary))
            {
                return TranslateSummary(input) as TModel;
            }
            else if (typeof(TModel) == typeof(Models.ResponseModels.Contributor.Contributor))
            {
                return Translate(input) as TModel;
            }
            return null;
        }

        private Models.ResponseModels.Contributor.Contributor Translate(taskapplicant input)
        {
            var contributor = new Models.ResponseModels.Contributor.Contributor();
            if (input != null)
            {
                contributor.ApplicantId = input.APPLICANT_ID;
                contributor.TaskApplicantId = input.ID;
                //contributor.CompletedHours = (input.approvedapplicants != null && input.approvedapplicants.Count > 0) ? input.approvedapplicants.Where(x => x.APPLICANT_ID == input.ID).FirstOrDefault().HOURS_WORKED : 0;
                contributor.CreatedBy = input.CREATED_BY;
                contributor.CreatedOn = input.CREATED_ON;
                if (input.user != null)
                {
                    contributor.Designation = input.user.DESIGNATION;
                    contributor.EmailId = input.user.EMAIL_ID;
                    contributor.FirstName = input.user.FIRST_NAME;
                    contributor.LastName = input.user.LAST_NAME;
                    //contributor.Expertise = input
                }
                contributor.Status = input.STATUS;
                contributor.StatusDate = input.STATUS_DATE;
                //contributor.TopRatingsCount = input.
                contributor.UpdatedBy = input.UPDATED_BY;
                contributor.UpdatedOn = input.UPDATED_ON;
                contributor.Duration = CommonHelper.CalculateDuration(input.CREATED_ON);
            }
            return contributor;
        }

        private ContributorSummary TranslateSummary(taskapplicant input)
        {
            var contributorSummary = new ContributorSummary();
            if (input != null)
            {
                contributorSummary.ApplicantId = input.APPLICANT_ID;
                if (input.user != null)
                {
                    //contributorSummary.des = input.user.DESIGNATION;
                    contributorSummary.EmailId = input.user.EMAIL_ID;
                    contributorSummary.FirstName = input.user.FIRST_NAME;
                    contributorSummary.LastName = input.user.LAST_NAME;
                }
            }
            return contributorSummary;
        }

        public taskapplicant CreateModel<TModel>(taskapplicant input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public taskapplicant CreateModel<TModel>(TModel input) where TModel : class
        {
            return null;
        }

        

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<taskapplicant> inputList) where TModel : class
        {
            throw new NotImplementedException();
        }
    }
}
