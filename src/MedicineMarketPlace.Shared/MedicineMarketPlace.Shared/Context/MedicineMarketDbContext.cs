using MedicineMarketPlace.BuildingBlocks.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace MedicineMarketPlace.Shared.Context
{
    //needds to chage this later
    public class ConcentrixDbContext : EfDbContext
    {
        public ConcentrixDbContext(string connectionString, bool enableEntityFrameworkLogging)
       : base(connectionString, enableEntityFrameworkLogging)
        {
        }

        public ConcentrixDbContext(DbContextOptions<ConcentrixDbContext> options)
        : base(options)
        {
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Location>(_ =>
            {
                _.ToTable(Location.LocationTable);
                _.HasKey(_ => _.Id);
                _.Ignore(_ => _.ModifiedBy);
                _.Ignore(_ => _.ModifiedDate);
            });

            builder.Entity<Coaching>(_ =>
            {
                _.ToTable(Coaching.CoachingTable);
                _.HasKey(_ => _.Id);
            });

            builder.Entity<Decision>(_ =>
            {
                _.ToTable(Decision.DecisionTable);
                _.HasKey(_ => _.Id);
                _.HasOne(_ => _.Coaching).WithMany(_ => _.Decisions).HasForeignKey(_ => _.CoachingId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserAttempt>(_ =>
            {
                _.ToTable(UserAttempt.UserAttemptTable);
                _.HasKey(_ => _.Id);
                _.HasOne(_ => _.Coaching).WithMany().HasForeignKey(_ => _.CoachingId).OnDelete(DeleteBehavior.Restrict);
                _.HasOne(_ => _.User).WithMany().HasForeignKey(_ => _.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserScore>(_ =>
            {
                _.ToTable(UserScore.UserScoreTable);
                _.HasKey(_ => _.UserId);
                _.HasOne(_ => _.User).WithMany().HasForeignKey(_ => _.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<FeedbackQuestion>(_ =>
            {
                _.ToTable(FeedbackQuestion.FeedbackQuestionTable);
                _.HasKey(_ => _.Id);
                _.HasOne(_ => _.Coaching).WithMany().HasForeignKey(_ => _.CoachingId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<FeedbackSubQuestion>(_ =>
            {
                _.ToTable(FeedbackSubQuestion.FeedbackSubQuestionTable);
                _.HasKey(_ => _.Id);
                _.HasOne(_ => _.FeedbackQuestion).WithMany().HasForeignKey(_ => _.FeedbackQuestionId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserFeedback>(_ =>
            {
                _.ToTable(UserFeedback.UserFeedbackTable);
                _.HasKey(_ => new { _.UserId, _.CoachingId });
                _.HasOne(_ => _.User).WithMany().HasForeignKey(_ => _.UserId).OnDelete(DeleteBehavior.Restrict);
                _.HasOne(_ => _.Coaching).WithMany().HasForeignKey(_ => _.CoachingId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MoodQuestion>(_ =>
            {
                _.ToTable(MoodQuestion.MoodQuestionTable);
                _.HasKey(_ => _.Id);
            });

            builder.Entity<UserMood>(_ =>
            {
                _.ToTable(UserMood.UserMoodTable);
                _.HasKey(_ => new { _.UserId, _.MoodQuestionId });
                _.HasOne(_ => _.User).WithMany().HasForeignKey(_ => _.UserId).OnDelete(DeleteBehavior.Restrict);
                _.HasOne(_ => _.MoodQuestion).WithMany().HasForeignKey(_ => _.MoodQuestionId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ScoreCompetency>(_ =>
            {
                _.ToTable(ScoreCompetency.ScoreCompetencyTable);
                _.HasKey(_ => _.Id);
                _.HasOne(_ => _.Coaching).WithMany().HasForeignKey(_ => _.CoachingId).OnDelete(DeleteBehavior.Restrict);
            });
        }*/
    }
}
