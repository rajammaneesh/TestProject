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
    public class ApprovedApplicantModelFactory :IModelFactory<approvedapplicant>
    {
        public TModel CreateModel<TModel>(approvedapplicant input) where TModel : class
        {
            if (typeof(TModel) == typeof(ContributorSummary))
            {
                return TranslateSummary(input) as TModel;
            }
            if (typeof(TModel) == typeof(DCode.Models.ResponseModels.Contributor.Contributor))
            {
                return Translate(input) as TModel;
            }
            return null;
        }

        private DCode.Models.ResponseModels.Contributor.Contributor Translate(approvedapplicant input)
        {
            var contributor = new DCode.Models.ResponseModels.Contributor.Contributor();
            if (input != null)
            {
                contributor.ApplicantId = input.ID;
                if (input.user != null)
                {
                    //contributorSummary.des = input.user.DESIGNATION;
                    contributor.EmailId = input.user.EMAIL_ID;
                    contributor.FirstName = input.user.FIRST_NAME;
                    contributor.LastName = input.user.LAST_NAME;
                }
                contributor.CompletedHours = input.HOURS_WORKED;
                contributor.CreatedBy = input.CREATED_BY;
                contributor.CreatedOn = input.CREATED_ON;
                //contributor.Designation = input.
                //contributor.Expertise = input.ex
                contributor.Status = input.STATUS;
                contributor.StatusDate = input.STATUS_DATE;
                //contributor.TopRatingsCount = input.
                contributor.UpdatedBy = input.UPDATED_BY;
                contributor.UpdatedOn = input.UPDATED_ON;
                contributor.Duration = CommonHelper.CalculateDuration(input.CREATED_ON);
            }
            return contributor;
        }

        private ContributorSummary TranslateSummary(approvedapplicant input)
        {
            var contributorSummary = new ContributorSummary();
            if (input != null)
            {
                contributorSummary.ApplicantId = input.ID;
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

        public approvedapplicant CreateModel<TModel>(approvedapplicant input, TModel model) where TModel : class
        {
            throw new NotImplementedException();
        }

        public approvedapplicant CreateModel<TModel>(TModel input) where TModel : class
        {
            if (typeof(TModel) == typeof(ReviewTaskRequest))
            {
                return TranslateReviewTask(input as ReviewTaskRequest);
            }
            return null;
        }

        private approvedapplicant TranslateReviewTask(ReviewTaskRequest input)
        {
            var approvedApplicant = new approvedapplicant();
            approvedApplicant.APPLICANT_ID = input.ApplicantId;
            approvedApplicant.ID = input.ApprovedApplicantId;
            approvedApplicant.RATING = input.Rating;
            approvedApplicant.COMMENTS = input.Comments;
            approvedApplicant.WORK_AGAIN = input.WorkAgain;
            return approvedApplicant;
        }

        public IEnumerable<TModel> CreateModelList<TModel>(IEnumerable<approvedapplicant> inputList) where TModel : class
        {
            throw new NotImplementedException();
        }
    }
}
